import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ChangeTicketStatusCommand } from '../domain/model/change-ticket-status.command';
import { CreateTicketCommand } from '../domain/model/create-ticket.command';
import { TicketHistoryEntry } from '../domain/model/ticket-history-entry.entity';
import { Ticket } from '../domain/model/ticket.entity';
import { TicketStatus } from '../domain/model/ticket-status';
import { ChangeTicketStatusApiEndpoint } from './change-ticket-status-endpoint';
import { CreateTicketApiEndpoint } from './create-ticket-endpoint';
import { TicketAssembler } from './ticket-assembler';
import { TicketHistoryApiEndpoint } from './ticket-history-endpoint';
import { TicketHistoryAssembler } from './ticket-history-assembler';
import { TicketsApiEndpoint } from './tickets-endpoint';

/**
 * API facade for the Maintenance bounded context. It composes one endpoint class per operation.
 */
@Injectable({ providedIn: 'root' })
export class TicketsApi{
  private readonly ticketsEndpoint: TicketsApiEndpoint;
  private readonly createTicketEndpoint: CreateTicketApiEndpoint;
  private readonly changeTicketStatusEndpoint: ChangeTicketStatusApiEndpoint;
  private readonly ticketHistoryEndpoint: TicketHistoryApiEndpoint;

  constructor(http: HttpClient) {
    const ticketAssembler = new TicketAssembler();
    const ticketHistoryAssembler = new TicketHistoryAssembler();
    this.ticketsEndpoint = new TicketsApiEndpoint(http, ticketAssembler);
    this.createTicketEndpoint = new CreateTicketApiEndpoint(http, ticketAssembler);
    this.changeTicketStatusEndpoint = new ChangeTicketStatusApiEndpoint(http, ticketAssembler);
    this.ticketHistoryEndpoint = new TicketHistoryApiEndpoint(http, ticketHistoryAssembler);
  }

  getTickets(status?: TicketStatus): Observable<Ticket[]> {
    return this.ticketsEndpoint.getTickets(status);
  }

  getTicketById(id: number): Observable<Ticket> {
    return this.ticketsEndpoint.getTicketById(id);
  }

  createTicket(command: CreateTicketCommand): Observable<Ticket> {
    return this.createTicketEndpoint.createTicket(command);
  }

  changeTicketStatus(command: ChangeTicketStatusCommand): Observable<Ticket> {
    return this.changeTicketStatusEndpoint.changeTicketStatus(command);
  }

  getTicketHistory(ticketId: number): Observable<TicketHistoryEntry[]> {
    return this.ticketHistoryEndpoint.getTicketHistory(ticketId);
  }
}