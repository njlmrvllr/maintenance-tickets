import { Component, computed, input } from '@angular/core';
import { TicketStatus } from '../../../domain/model/ticket-status';
import { TICKET_STATUS_LABELS } from '../../ticket-status-labels';

/**
 * Status pill. Color always comes with the status name, so it never relies on color alone.
 */
@Component({
  selector: 'app-status-badge',
  templateUrl: './status-badge.html',
  styleUrl: './status-badge.scss',
})
export class StatusBadge {
  readonly status = input.required<TicketStatus>();
  protected readonly label = computed(() => TICKET_STATUS_LABELS[this.status()]);
}
