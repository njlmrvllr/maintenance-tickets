import { computed, Injectable, signal } from '@angular/core';
import { TicketNotification } from '../domain/model/ticket-notification';
import { Ticket } from '../domain/model/ticket.entity';
import { TicketStatus } from '../domain/model/ticket-status';

/** Number of notifications kept in memory. */
const MAX_NOTIFICATIONS = 20;

/**
 * Application-layer store for the simulated notifications sent to reporters.
 * Notifications live in memory only: they are cleared when the page reloads.
 */
@Injectable({ providedIn: 'root' })
export class NotificationsStore {
  private readonly notificationsSignal = signal<TicketNotification[]>([]);
  private readonly unreadCountSignal = signal(0);
  private nextId = 1;

  /** Newest first. */
  readonly notifications = this.notificationsSignal.asReadonly();
  readonly unreadCount = this.unreadCountSignal.asReadonly();
  readonly hasNotifications = computed(() => this.notificationsSignal().length > 0);

  /** Records the notification sent to the reporter after a successful status change. */
  notifyStatusChanged(ticket: Ticket, previousStatus: TicketStatus, changedBy: string): void {
    const notification = new TicketNotification(
      this.nextId++, ticket.id, ticket.title, ticket.reportedBy, previousStatus, ticket.status, changedBy, new Date(),
    );
    this.notificationsSignal.update((notifications) => [notification, ...notifications].slice(0, MAX_NOTIFICATIONS));
    this.unreadCountSignal.update((count) => Math.min(count + 1, MAX_NOTIFICATIONS));
  }

  /** True when the notification arrived after the list was last read. */
  isUnread(notification: TicketNotification): boolean {
    return this.notificationsSignal().indexOf(notification) < this.unreadCountSignal();
  }

  markAllAsRead(): void {
    this.unreadCountSignal.set(0);
  }
}
