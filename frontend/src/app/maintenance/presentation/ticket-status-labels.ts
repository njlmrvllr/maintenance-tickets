import { TicketStatus } from '../domain/model/ticket-status';

export const TICKET_STATUS_LABELS: Record<TicketStatus, string> = {
  Pending: 'Pending',
  InProgress: 'In Progress',
  Resolved: 'Resolved',
  Closed: 'Closed',
  Cancelled: 'Cancelled',
};

/** Button label for each target status. */
export const TICKET_ACTION_LABELS: Record<TicketStatus, string> = {
  Pending: 'Move to pending',
  InProgress: 'Start work',
  Resolved: 'Mark as resolved',
  Closed: 'Close ticket',
  Cancelled: 'Cancel ticket',
};
