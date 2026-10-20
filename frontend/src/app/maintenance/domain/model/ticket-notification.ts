import { TicketStatus } from './ticket-status';

/**
 * Notification sent to the reporter when a ticket changes status.
 * Delivery is simulated: the API writes it to its log and the UI lists it in the notification center.
 */
export class TicketNotification {
  constructor(
    readonly id: number,
    readonly ticketId: number,
    readonly ticketTitle: string,
    readonly recipient: string,
    readonly previousStatus: TicketStatus,
    readonly newStatus: TicketStatus,
    readonly changedBy: string,
    readonly sentAt: Date,
  ) {}
}
