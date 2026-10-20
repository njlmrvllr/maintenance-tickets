import { computed, inject, Injectable, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { toAppError } from '../../shared/infrastructure/app-error.mapper';
import { ChangeTicketStatusCommand } from '../domain/model/change-ticket-status.command';
import { CreateTicketCommand } from '../domain/model/create-ticket.command';
import { CreatedWithin, TicketFilter } from '../domain/model/ticket-filter';
import { TicketHistoryEntry } from '../domain/model/ticket-history-entry.entity';
import { Ticket } from '../domain/model/ticket.entity';
import { TicketStatus } from '../domain/model/ticket-status';
import { TicketsApi } from '../infrastructure/tickets-api';
import { NotificationsStore } from './notifications.store';

/**
 * Application-layer store for the Maintenance bounded context.
 * State lives in private signals and is exposed as read-only signals.
 */
@Injectable({ providedIn: 'root' })
export class TicketsStore {
  private readonly api = inject(TicketsApi);
  private readonly notifications = inject(NotificationsStore);

  private readonly ticketsSignal = signal<Ticket[]>([]);
  private readonly selectedTicketIdSignal = signal<number | null>(null);
  private readonly historySignal = signal<TicketHistoryEntry[]>([]);
  private readonly filterSignal = signal<TicketFilter>(TicketFilter.empty);
  private readonly lastUpdatedSignal = signal<Date | null>(null);

  private readonly loadingTicketsSignal = signal(false);
  private readonly loadingHistorySignal = signal(false);
  private readonly savingSignal = signal(false);
  private readonly loadErrorSignal = signal<string | null>(null);
  private readonly actionErrorSignal = signal<string | null>(null);

  readonly tickets = this.ticketsSignal.asReadonly();
  readonly selectedTicketId = this.selectedTicketIdSignal.asReadonly();
  readonly history = this.historySignal.asReadonly();
  readonly filter = this.filterSignal.asReadonly();
  readonly lastUpdated = this.lastUpdatedSignal.asReadonly();
  readonly isLoadingTickets = this.loadingTicketsSignal.asReadonly();
  readonly isLoadingHistory = this.loadingHistorySignal.asReadonly();
  readonly isSaving = this.savingSignal.asReadonly();
  readonly loadError = this.loadErrorSignal.asReadonly();
  readonly actionError = this.actionErrorSignal.asReadonly();

  readonly ticketCount = computed(() => this.ticketsSignal().length);

  readonly selectedTicket = computed(
    () => this.ticketsSignal().find((ticket) => ticket.id === this.selectedTicketIdSignal()) ?? null,
  );

  /** Tickets that match the filter (search term, statuses and creation date). */
  readonly filteredTickets = computed(() => {
    const filter = this.filterSignal();
    if (!filter.isActive) return this.ticketsSignal();
    const now = new Date();
    return this.ticketsSignal().filter((ticket) => filter.matches(ticket, now));
  });

  readonly filteredCount = computed(() => this.filteredTickets().length);

  /** Filtered tickets grouped by status, ready for the board columns. */
  readonly ticketsByStatus = computed(() => {
    const groups: Record<TicketStatus, Ticket[]> = {
      Pending: [], InProgress: [], Resolved: [], Closed: [], Cancelled: [],
    };
    for (const ticket of this.filteredTickets()) groups[ticket.status].push(ticket);
    return groups;
  });

  async loadTickets(): Promise<void> {
    this.loadingTicketsSignal.set(true);
    this.loadErrorSignal.set(null);
    try {
      const tickets = await firstValueFrom(this.api.getTickets());
      this.ticketsSignal.set(tickets);
      this.lastUpdatedSignal.set(new Date());
    } catch (error) {
      this.loadErrorSignal.set(toAppError(error).detail);
    } finally {
      this.loadingTicketsSignal.set(false);
    }
  }

  async selectTicket(ticketId: number): Promise<void> {
    this.selectedTicketIdSignal.set(ticketId);
    this.actionErrorSignal.set(null);
    await this.loadHistory(ticketId);
  }

  clearSelection(): void {
    this.selectedTicketIdSignal.set(null);
    this.historySignal.set([]);
    this.actionErrorSignal.set(null);
  }

  setSearchTerm(term: string): void {
    this.filterSignal.update((filter) => new TicketFilter(term, filter.statuses, filter.createdWithin));
  }

  setStatusFilter(statuses: readonly TicketStatus[]): void {
    this.filterSignal.update((filter) => new TicketFilter(filter.searchTerm, statuses, filter.createdWithin));
  }

  setCreatedWithin(createdWithin: CreatedWithin): void {
    this.filterSignal.update((filter) => new TicketFilter(filter.searchTerm, filter.statuses, createdWithin));
  }

  clearFilters(): void {
    this.filterSignal.set(TicketFilter.empty);
  }

  clearActionError(): void {
    this.actionErrorSignal.set(null);
  }

  /** Creates a ticket. Returns the created ticket, or null when the API rejected it. */
  async createTicket(command: CreateTicketCommand): Promise<Ticket | null> {
    this.savingSignal.set(true);
    this.actionErrorSignal.set(null);
    try {
      const ticket = await firstValueFrom(this.api.createTicket(command));
      this.ticketsSignal.update((tickets) => [ticket, ...tickets]);
      return ticket;
    } catch (error) {
      this.actionErrorSignal.set(toAppError(error).detail);
      return null;
    } finally {
      this.savingSignal.set(false);
    }
  }

  /**
   * Changes the status of a ticket. Returns true when the change was saved.
   * On a 409 (invalid transition or concurrent change) the board is reloaded so it shows the real status.
   */
  async changeTicketStatus(command: ChangeTicketStatusCommand): Promise<boolean> {
    this.savingSignal.set(true);
    this.actionErrorSignal.set(null);
    const previousStatus = this.ticketsSignal().find((ticket) => ticket.id === command.ticketId)?.status;
    try {
      const updated = await firstValueFrom(this.api.changeTicketStatus(command));
      this.ticketsSignal.update((tickets) => tickets.map((ticket) => (ticket.id === updated.id ? updated : ticket)));
      if (previousStatus) this.notifications.notifyStatusChanged(updated, previousStatus, command.changedBy);
      this.lastUpdatedSignal.set(new Date());
      await this.loadHistory(updated.id);
      return true;
    } catch (error) {
      const appError = toAppError(error);
      this.actionErrorSignal.set(appError.detail);
      if (appError.status === 409) {
        await this.loadTickets();
        await this.loadHistory(command.ticketId);
      }
      return false;
    } finally {
      this.savingSignal.set(false);
    }
  }

  private async loadHistory(ticketId: number): Promise<void> {
    this.loadingHistorySignal.set(true);
    try {
      this.historySignal.set(await firstValueFrom(this.api.getTicketHistory(ticketId)));
    } catch (error) {
      this.historySignal.set([]);
      this.actionErrorSignal.set(toAppError(error).detail);
    } finally {
      this.loadingHistorySignal.set(false);
    }
  }
}