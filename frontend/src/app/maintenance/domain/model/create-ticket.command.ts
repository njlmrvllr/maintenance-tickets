/**
 * Command to report a new maintenance incident.
 */
export class CreateTicketCommand {
  constructor(
    readonly title: string,
    readonly assetCode: string,
    readonly description: string,
    readonly reportedBy: string,
  ) {}
}