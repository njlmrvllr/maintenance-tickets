import { TicketStatus } from './ticket-status';

/**
 * Command to move a ticket to a new status.
 */
export class ChangeTicketStatusCommand {
  constructor(
    readonly ticketId: number,
    readonly targetStatus: TicketStatus,
    readonly comment: string | null,
    readonly changedBy: string,
  ) {}
}