import { ChangeTicketStatusCommand } from '../domain/model/change-ticket-status.command';
import { CreateTicketCommand } from '../domain/model/create-ticket.command';
import { Ticket } from '../domain/model/ticket.entity';
import { ChangeTicketStatusRequest, CreateTicketRequest } from './ticket.request';
import { TicketResponse } from './ticket.response';

/**
 * Converts between API payloads and the Ticket entity.
 */
export class TicketAssembler {
  toEntityFromResponse(response: TicketResponse): Ticket {
    return new Ticket({
      id: response.id,
      title: response.title,
      assetCode: response.assetCode,
      description: response.description,
      status: response.status,
      diagnosis: response.diagnosis,
      reportedBy: response.reportedBy,
      allowedTransitions: response.allowedTransitions,
      createdAt: response.createdAt ? new Date(response.createdAt) : null,
      updatedAt: response.updatedAt ? new Date(response.updatedAt) : null,
    });
  }

  toEntitiesFromResponse(responses: TicketResponse[]): Ticket[] {
    return responses.map((response) => this.toEntityFromResponse(response));
  }

  toCreateRequestFromCommand(command: CreateTicketCommand): CreateTicketRequest {
    return {
      title: command.title,
      assetCode: command.assetCode,
      description: command.description,
      reportedBy: command.reportedBy,
    };
  }

  toChangeStatusRequestFromCommand(command: ChangeTicketStatusCommand): ChangeTicketStatusRequest {
    return {
      targetStatus: command.targetStatus,
      comment: command.comment,
      changedBy: command.changedBy,
    };
  }
}