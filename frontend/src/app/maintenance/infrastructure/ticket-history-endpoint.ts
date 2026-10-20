import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildApiUrl } from '../../shared/infrastructure/api-url';
import { ErrorHandlingEnabledBaseType } from '../../shared/infrastructure/error-handling-enabled-base-type';
import { TicketHistoryEntry } from '../domain/model/ticket-history-entry.entity';
import { TicketHistoryAssembler } from './ticket-history-assembler';
import { TicketHistoryEntryResponse } from './ticket.response';

const ticketsApiEndpointUrl = buildApiUrl(environment.apiBaseUrl, environment.ticketsEndpointPath);

export class TicketHistoryApiEndpoint extends ErrorHandlingEnabledBaseType {
  constructor(
    private http: HttpClient,
    private assembler: TicketHistoryAssembler,
  ) {
    super();
  }

  getTicketHistory(ticketId: number): Observable<TicketHistoryEntry[]> {
    return this.http.get<TicketHistoryEntryResponse[]>(`${ticketsApiEndpointUrl}/${ticketId}/history`).pipe(
      map((responses) => this.assembler.toEntitiesFromResponse(responses)),
      catchError(this.handleError('Failed to load the ticket history')),
    );
  }
}