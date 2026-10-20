import { Component, input } from '@angular/core';
import { DatePipe } from '@angular/common';
import { TicketHistoryEntry } from '../../../domain/model/ticket-history-entry.entity';
import { TicketStatus } from '../../../domain/model/ticket-status';
import { TICKET_STATUS_LABELS } from '../../ticket-status-labels';

@Component({
  selector: 'app-history-timeline',
  imports: [DatePipe],
  templateUrl: './history-timeline.html',
  styleUrl: './history-timeline.scss',
})
export class HistoryTimeline {
  readonly entries = input.required<readonly TicketHistoryEntry[]>();

  protected label(status: TicketStatus | null): string {
    return status ? TICKET_STATUS_LABELS[status] : 'Created';
  }
}
