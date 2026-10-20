import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildApiUrl } from '../../shared/infrastructure/api-url';
import { ErrorHandlingEnabledBaseType } from '../../shared/infrastructure/error-handling-enabled-base-type';
import { ChangeTicketStatusCommand } from '../domain/model/change-ticket-status.command';
import { Ticket } from '../domain/model/ticket.entity';
import { TicketAssembler } from './ticket-assembler';
import { TicketResponse } from './ticket.response';

const ticketsApiEndpointUrl = buildApiUrl(environment.apiBaseUrl, environment.ticketsEndpointPath);

export class ChangeTicketStatusApiEndpoint extends ErrorHandlingEnabledBaseType {
  constructor(
    private http: HttpClient,
    private assembler: TicketAssembler,
  ) {
    super();
  }

  changeTicketStatus(command: ChangeTicketStatusCommand): Observable<Ticket> {
    const request = this.assembler.toChangeStatusRequestFromCommand(command);
    return this.http.patch<TicketResponse>(`${ticketsApiEndpointUrl}/${command.ticketId}/status`, request).pipe(
      map((response) => this.assembler.toEntityFromResponse(response)),
      catchError(this.handleError('Failed to change the ticket status')),
    );
  }
}