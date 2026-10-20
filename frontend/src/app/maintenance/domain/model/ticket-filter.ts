import { Ticket } from './ticket.entity';
import { TicketStatus } from './ticket-status';

/** Creation date ranges the board can filter by. */
export type CreatedWithin = 'any' | 'today' | 'last7Days' | 'last30Days';

/** Days covered by each range, counting today. */
const DAYS_IN_RANGE: Record<Exclude<CreatedWithin, 'any'>, number> = {
  today: 1,
  last7Days: 7,
  last30Days: 30,
};

/**
 * Criteria to filter the tickets on the board: search term, statuses and creation date.
 * An empty status list means "all statuses".
 */
export class TicketFilter {
  static readonly empty = new TicketFilter('', [], 'any');

  constructor(
    readonly searchTerm: string,
    readonly statuses: readonly TicketStatus[],
    readonly createdWithin: CreatedWithin,
  ) {}

  /** True when at least one criterion is set. */
  get isActive(): boolean {
    return this.searchTerm.trim() !== '' || this.statuses.length > 0 || this.createdWithin !== 'any';
  }

  /** True when the status is shown with the current criteria. */
  includesStatus(status: TicketStatus): boolean {
    return this.statuses.length === 0 || this.statuses.includes(status);
  }

  matches(ticket: Ticket, now: Date = new Date()): boolean {
    return this.matchesSearchTerm(ticket) && this.includesStatus(ticket.status) && this.matchesCreatedWithin(ticket, now);
  }

  private matchesSearchTerm(ticket: Ticket): boolean {
    const term = this.searchTerm.trim().toLowerCase();
    if (!term) return true;
    return ticket.title.toLowerCase().includes(term) || ticket.assetCode.toLowerCase().includes(term);
  }

  /** Compares calendar days in local time: "today" starts at 00:00, not 24 hours ago. */
  private matchesCreatedWithin(ticket: Ticket, now: Date): boolean {
    if (this.createdWithin === 'any') return true;
    if (!ticket.createdAt) return false;

    const from = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    from.setDate(from.getDate() - (DAYS_IN_RANGE[this.createdWithin] - 1));
    return ticket.createdAt >= from;
  }
}
