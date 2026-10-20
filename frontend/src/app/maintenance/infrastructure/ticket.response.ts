import { TicketStatus } from '../domain/model/ticket-status';

export interface TicketResponse{
  id: number;
  title: string;
  assetCode: string;
  description: string;
  status: TicketStatus;
  diagnosis: string | null;
  reportedBy: string;
  allowedTransitions: TicketStatus[];
  createdAt: string | null;
  updatedAt: string | null;
}

export interface TicketHistoryEntryResponse{
  id: number;
  fromStatus: TicketStatus | null;
  toStatus: TicketStatus;
  comment: string | null;
  changedBy: string;
  changedAt: string;
}