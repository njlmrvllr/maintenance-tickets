/**
 * Ticket statuses, with the same names the API sends.
 */
export type TicketStatus = 'Pending' | 'InProgress' | 'Resolved' | 'Closed' | 'Cancelled';

/**
 * Statuses that need a comment: a diagnosis to resolve, a reason to cancel.
 * The backend enforces this rule; the UI uses it to warn the user before sending the request.
 */
export const STATUSES_REQUIRING_COMMENT: readonly TicketStatus[] = ['Resolved', 'Cancelled'];