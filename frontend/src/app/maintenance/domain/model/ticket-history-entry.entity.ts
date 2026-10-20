import { TicketStatus } from './ticket-status';

export interface TicketHistoryEntryProps {
  id: number;
  fromStatus: TicketStatus | null;
  toStatus: TicketStatus;
  comment: string | null;
  changedBy: string;
  changedAt: Date;
}

/**
 * One event in the history of a ticket. fromStatus is null for the creation event.
 */
export class TicketHistoryEntry{
  private readonly _id: number;
  private readonly _fromStatus: TicketStatus | null;
  private readonly _toStatus: TicketStatus;
  private readonly _comment: string | null;
  private readonly _changedBy: string;
  private readonly _changedAt: Date;

  constructor(props: TicketHistoryEntryProps) {
    this._id = props.id;
    this._fromStatus = props.fromStatus;
    this._toStatus = props.toStatus;
    this._comment = props.comment;
    this._changedBy = props.changedBy;
    this._changedAt = props.changedAt;
  }

  get id(): number { return this._id; }
  get fromStatus(): TicketStatus | null { return this._fromStatus; }
  get toStatus(): TicketStatus { return this._toStatus; }
  get comment(): string | null { return this._comment; }
  get changedBy(): string { return this._changedBy; }
  get changedAt(): Date { return this._changedAt; }
}