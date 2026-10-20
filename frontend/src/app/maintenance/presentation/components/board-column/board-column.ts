import { Component, computed, input, output } from '@angular/core';
import { Ticket } from '../../../domain/model/ticket.entity';
import { TicketStatus } from '../../../domain/model/ticket-status';
import { TICKET_STATUS_LABELS } from '../../ticket-status-labels';
import { StatusBadge } from '../status-badge/status-badge';
import { TicketCard } from '../ticket-card/ticket-card';

@Component({
  selector: 'app-board-column',
  imports: [StatusBadge, TicketCard],
  templateUrl: './board-column.html',
  styleUrl: './board-column.scss',
})
export class BoardColumn {
  readonly statuses = input.required<readonly TicketStatus[]>();
  readonly tickets = input.required<readonly Ticket[]>();
  readonly selectedTicketId = input<number | null>(null);
  readonly ticketSelected = output<number>();

  /** Columns that group several statuses show the status on each card. */
  protected readonly showStatusOnCards = computed(() => this.statuses().length > 1);
  protected readonly ariaLabel = computed(
    () => `${this.statuses().map((status) => TICKET_STATUS_LABELS[status]).join(' and ')} tickets`,
  );
}
