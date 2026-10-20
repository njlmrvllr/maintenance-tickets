import { TicketStatus } from './ticket-status';

export interface TicketProps {
  id: number;
  title: string;
  assetCode: string;
  description: string;
  status: TicketStatus;
  diagnosis: string | null;
  reportedBy: string;
  allowedTransitions: TicketStatus[];
  createdAt: Date | null;
  updatedAt: Date | null;
}

/**
 * A maintenance ticket as shown in the UI.
 * The allowed transitions come from the API: the frontend never decides the business rules.
 */
export class Ticket{
  private readonly _id: number;
  private readonly _title: string;
  private readonly _assetCode: string;
  private readonly _description: string;
  private readonly _status: TicketStatus;
  private readonly _diagnosis: string | null;
  private readonly _reportedBy: string;
  private readonly _allowedTransitions: readonly TicketStatus[];
  private readonly _createdAt: Date | null;
  private readonly _updatedAt: Date | null;

  constructor(props: TicketProps) {
    this._id = props.id;
    this._title = props.title;
    this._assetCode = props.assetCode;
    this._description = props.description;
    this._status = props.status;
    this._diagnosis = props.diagnosis;
    this._reportedBy = props.reportedBy;
    this._allowedTransitions = [...props.allowedTransitions];
    this._createdAt = props.createdAt;
    this._updatedAt = props.updatedAt;
  }

  get id(): number { return this._id; }
  get title(): string { return this._title; }
  get assetCode(): string { return this._assetCode; }
  get description(): string { return this._description; }
  get status(): TicketStatus { return this._status; }
  get diagnosis(): string | null { return this._diagnosis; }
  get reportedBy(): string { return this._reportedBy; }
  get allowedTransitions(): readonly TicketStatus[] { return this._allowedTransitions; }
  get createdAt(): Date | null { return this._createdAt; }
  get updatedAt(): Date | null { return this._updatedAt; }

  /** True when no further status changes are possible (Closed or Cancelled). */
  get isFinal(): boolean { return this._allowedTransitions.length === 0; }

  canTransitionTo(status: TicketStatus): boolean {
    return this._allowedTransitions.includes(status);
  }
}