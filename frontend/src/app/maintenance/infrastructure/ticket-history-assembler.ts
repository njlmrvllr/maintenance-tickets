import { TicketHistoryEntry } from '../domain/model/ticket-history-entry.entity';
import { TicketHistoryEntryResponse } from './ticket.response';

/**
 * Converts history payloads into TicketHistoryEntry entities.
 */
export class TicketHistoryAssembler {
  toEntityFromResponse(response: TicketHistoryEntryResponse): TicketHistoryEntry {
    return new TicketHistoryEntry({
      id: response.id,
      fromStatus: response.fromStatus,
      toStatus: response.toStatus,
      comment: response.comment,
      changedBy: response.changedBy,
      changedAt: new Date(response.changedAt),
    });
  }

  toEntitiesFromResponse(responses: TicketHistoryEntryResponse[]): TicketHistoryEntry[] {
    return responses.map((response) => this.toEntityFromResponse(response));
  }
}