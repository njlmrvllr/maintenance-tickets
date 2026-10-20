import { Component, input, output } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Ticket } from '../../../domain/model/ticket.entity';

@Component({
  selector: 'app-ticket-card',
  imports: [DatePipe],
  templateUrl: './ticket-card.html',
  styleUrl: './ticket-card.scss',
})
export class TicketCard {
  readonly ticket = input.required<Ticket>();
  readonly selected = input(false);
  readonly ticketSelected = output<number>();
}
