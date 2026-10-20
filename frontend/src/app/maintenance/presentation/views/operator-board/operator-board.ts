import { Component, computed, inject, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CURRENT_OPERATOR_NAME } from '../../../application/current-operator';
import { TicketsStore } from '../../../application/tickets.store';
import { ChangeTicketStatusCommand } from '../../../domain/model/change-ticket-status.command';
import { TicketStatus } from '../../../domain/model/ticket-status';
import { BoardColumn } from '../../components/board-column/board-column';
import { StatusChangeRequest, TicketDetailPanel } from '../../components/ticket-detail-panel/ticket-detail-panel';
import { TICKET_STATUS_LABELS } from '../../ticket-status-labels';

interface BoardColumnDefinition {
  id: string;
  statuses: readonly TicketStatus[];
}

/** Board columns. Closed and Cancelled share one column because both are final states. */
const BOARD_COLUMNS: readonly BoardColumnDefinition[] = [
  { id: 'pending', statuses: ['Pending'] },
  { id: 'in-progress', statuses: ['InProgress'] },
  { id: 'resolved', statuses: ['Resolved'] },
  { id: 'done', statuses: ['Closed', 'Cancelled'] },
];

@Component({
  selector: 'app-operator-board',
  imports: [DatePipe, MatButton, MatProgressBar, BoardColumn, TicketDetailPanel],
  templateUrl: './operator-board.html',
  styleUrl: './operator-board.scss',
})
export class OperatorBoard implements OnInit {
  protected readonly store = inject(TicketsStore);
  private readonly snackBar = inject(MatSnackBar);

  protected readonly columns = computed(() => {
    const byStatus = this.store.ticketsByStatus();
    return BOARD_COLUMNS.map((column) => ({
      ...column,
      tickets: column.statuses.flatMap((status) => byStatus[status]),
    }));
  });

  ngOnInit(): void {
    this.store.clearActionError();
    void this.store.loadTickets();
  }

  protected onSearch(event: Event): void {
    this.store.setSearchTerm((event.target as HTMLInputElement).value);
  }

  protected onTicketSelected(ticketId: number): void {
    void this.store.selectTicket(ticketId);
  }

  protected async onStatusChangeRequested(request: StatusChangeRequest): Promise<void> {
    const ticket = this.store.selectedTicket();
    if (!ticket) return;

    const changed = await this.store.changeTicketStatus(
      new ChangeTicketStatusCommand(ticket.id, request.targetStatus, request.comment, CURRENT_OPERATOR_NAME),
    );

    if (changed) {
      this.snackBar.open(
        `Ticket #${ticket.id} moved to ${TICKET_STATUS_LABELS[request.targetStatus]}.`,
        'Close',
        { duration: 3000 },
      );
    }
  }
}
