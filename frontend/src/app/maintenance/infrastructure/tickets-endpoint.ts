import { HttpClient, HttpParams } from '@angular/common/http';
import { catchError, map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildApiUrl } from '../../shared/infrastructure/api-url';
import { ErrorHandlingEnabledBaseType } from '../../shared/infrastructure/error-handling-enabled-base-type';
import { Ticket } from '../domain/model/ticket.entity';
import { TicketStatus } from '../domain/model/ticket-status';
import { TicketAssembler } from './ticket-assembler';
import { TicketResponse } from './ticket.response';

const ticketsApiEndpointUrl = buildApiUrl(environment.apiBaseUrl, environment.ticketsEndpointPath);

export class TicketsApiEndpoint extends ErrorHandlingEnabledBaseType {
  constructor(
    private http: HttpClient,
    private assembler: TicketAssembler,
  ) {
    super();
  }

  getTickets(status?: TicketStatus): Observable<Ticket[]> {
    const params = status ? new HttpParams().set('status', status) : undefined;
    return this.http.get<TicketResponse[]>(ticketsApiEndpointUrl, { params }).pipe(
      map((responses) => this.assembler.toEntitiesFromResponse(responses)),
      catchError(this.handleError('Failed to load tickets')),
    );
  }

  getTicketById(id: number): Observable<Ticket> {
    return this.http.get<TicketResponse>(`${ticketsApiEndpointUrl}/${id}`).pipe(
      map((response) => this.assembler.toEntityFromResponse(response)),
      catchError(this.handleError('Failed to load the ticket')),
    );
  }
}