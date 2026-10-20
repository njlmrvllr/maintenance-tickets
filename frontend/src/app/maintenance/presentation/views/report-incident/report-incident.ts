import { Component, inject, signal, viewChild } from '@angular/core';
import { TicketsStore } from '../../../application/tickets.store';
import { CreateTicketCommand } from '../../../domain/model/create-ticket.command';
import { Ticket } from '../../../domain/model/ticket.entity';
import { TicketStatus } from '../../../domain/model/ticket-status';
import { ReportForm } from '../../components/report-form/report-form';
import { StatusBadge } from '../../components/status-badge/status-badge';

@Component({
  selector: 'app-report-incident',
  imports: [ReportForm, StatusBadge],
  templateUrl: './report-incident.html',
  styleUrl: './report-incident.scss',
})
export class ReportIncident {
  protected readonly store = inject(TicketsStore);
  private readonly reportForm = viewChild.required(ReportForm);

  protected readonly createdTicket = signal<Ticket | null>(null);

  protected readonly nextSteps: { status: TicketStatus; text: string }[] = [
    { status: 'Pending', text: 'Your report is registered and waits for an operator.' },
    { status: 'InProgress', text: 'An operator is diagnosing or repairing the equipment.' },
    { status: 'Resolved', text: 'The issue is fixed. The operator records the diagnosis.' },
    { status: 'Closed', text: 'The ticket is confirmed and archived.' },
  ];

  constructor() {
    this.store.clearActionError();
  }

  protected async onSubmitted(command: CreateTicketCommand): Promise<void> {
    this.createdTicket.set(null);
    const ticket = await this.store.createTicket(command);
    if (ticket) {
      this.createdTicket.set(ticket);
      this.reportForm().reset();
    }
  }
}
