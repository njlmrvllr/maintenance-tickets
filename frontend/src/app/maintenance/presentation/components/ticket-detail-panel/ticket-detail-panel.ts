import { Component, computed, effect, input, output, signal, untracked } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatError, MatFormField, MatHint, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatProgressBar } from '@angular/material/progress-bar';
import { TicketHistoryEntry } from '../../../domain/model/ticket-history-entry.entity';
import { Ticket } from '../../../domain/model/ticket.entity';
import { STATUSES_REQUIRING_COMMENT, TicketStatus } from '../../../domain/model/ticket-status';
import { TICKET_ACTION_LABELS, TICKET_STATUS_LABELS } from '../../ticket-status-labels';
import { HistoryTimeline } from '../history-timeline/history-timeline';
import { StatusBadge } from '../status-badge/status-badge';

export interface StatusChangeRequest {
  targetStatus: TicketStatus;
  comment: string | null;
}

/**
 * Detail of the selected ticket: data, allowed actions and history.
 * Only the transitions returned by the API are shown as buttons.
 */
@Component({
  selector: 'app-ticket-detail-panel',
  imports: [
    DatePipe, ReactiveFormsModule, MatFormField, MatLabel, MatHint, MatError, MatInput,
    MatButton, MatProgressBar, StatusBadge, HistoryTimeline,
  ],
  templateUrl: './ticket-detail-panel.html',
  styleUrl: './ticket-detail-panel.scss',
})
export class TicketDetailPanel {
  readonly ticket = input.required<Ticket>();
  readonly history = input<readonly TicketHistoryEntry[]>([]);
  readonly isLoadingHistory = input(false);
  readonly isSaving = input(false);
  readonly errorMessage = input<string | null>(null);

  readonly statusChangeRequested = output<StatusChangeRequest>();
  readonly closed = output<void>();

  protected readonly maxCommentLength = 500;
  protected readonly statusLabels = TICKET_STATUS_LABELS;
  protected readonly actionLabels = TICKET_ACTION_LABELS;
  protected readonly commentControl = new FormControl('', {
    nonNullable: true,
    validators: [Validators.maxLength(this.maxCommentLength)],
  });
  protected readonly requiredMessage = signal('');

  /** Main action of the current status (shown as the filled button). */
  protected readonly primaryTransition = computed(
    () => this.ticket().allowedTransitions.find((status) => status !== 'Cancelled') ?? null,
  );

  constructor() {
    // Clear the comment when another ticket is selected or the ticket is updated.
    effect(() => {
      this.ticket();
      untracked(() => {
        this.commentControl.reset();
        this.requiredMessage.set('');
      });
    });
  }

  protected appearanceFor(target: TicketStatus): 'filled' | 'outlined' {
    return target === this.primaryTransition() ? 'filled' : 'outlined';
  }

  protected requestTransition(target: TicketStatus): void {
    const comment = this.commentControl.value.trim();

    // Error prevention: warn before calling the API when a comment is required.
    if (STATUSES_REQUIRING_COMMENT.includes(target) && !comment) {
      this.requiredMessage.set(target === 'Resolved'
        ? 'A diagnosis is required to resolve this ticket.'
        : 'A reason is required to cancel this ticket.');
      this.commentControl.setErrors({ commentRequired: true });
      this.commentControl.markAsTouched();
      return;
    }

    if (this.commentControl.invalid) {
      this.commentControl.markAsTouched();
      return;
    }

    this.statusChangeRequested.emit({ targetStatus: target, comment: comment || null });
  }
}
