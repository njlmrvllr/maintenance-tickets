import { Component, computed, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { MatIconButton } from '@angular/material/button';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { NotificationsStore } from '../../../application/notifications.store';
import { TICKET_STATUS_LABELS } from '../../ticket-status-labels';

/**
 * Bell with the simulated notifications sent to reporters.
 * The list is marked as read when the menu closes, so the unread items stay highlighted while it is open.
 */
@Component({
  selector: 'app-notification-center',
  imports: [DatePipe, MatIconButton, MatMenu, MatMenuTrigger],
  templateUrl: './notification-center.html',
  styleUrl: './notification-center.scss',
})
export class NotificationCenter {
  protected readonly store = inject(NotificationsStore);
  protected readonly statusLabels = TICKET_STATUS_LABELS;

  protected readonly ariaLabel = computed(() => {
    const unread = this.store.unreadCount();
    return unread > 0 ? `Notifications, ${unread} unread` : 'Notifications';
  });
}
