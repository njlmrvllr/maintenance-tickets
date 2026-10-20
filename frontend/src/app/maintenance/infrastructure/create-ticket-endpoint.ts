import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildApiUrl } from '../../shared/infrastructure/api-url';
import { ErrorHandlingEnabledBaseType } from '../../shared/infrastructure/error-handling-enabled-base-type';
import { CreateTicketCommand } from '../domain/model/create-ticket.command';
import { Ticket } from '../domain/model/ticket.entity';
import { TicketAssembler } from './ticket-assembler';
import { TicketResponse } from './ticket.response';

const createTicketApiEndpointUrl = buildApiUrl(environment.apiBaseUrl, environment.ticketsEndpointPath);

export class CreateTicketApiEndpoint extends ErrorHandlingEnabledBaseType {
  constructor(
    private http: HttpClient,
    private assembler: TicketAssembler,
  ) {
    super();
  }

  createTicket(command: CreateTicketCommand): Observable<Ticket> {
    const request = this.assembler.toCreateRequestFromCommand(command);
    return this.http.post<TicketResponse>(createTicketApiEndpointUrl, request).pipe(
      map((response) => this.assembler.toEntityFromResponse(response)),
      catchError(this.handleError('Failed to create the ticket')),
    );
  }
}