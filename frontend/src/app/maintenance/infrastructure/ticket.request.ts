import { TicketStatus } from '../domain/model/ticket-status';

export interface CreateTicketRequest {
  title: string;
  assetCode: string;
  description: string;
  reportedBy: string;
}

export interface ChangeTicketStatusRequest {
  targetStatus: TicketStatus;
  comment: string | null;
  changedBy: string;
}