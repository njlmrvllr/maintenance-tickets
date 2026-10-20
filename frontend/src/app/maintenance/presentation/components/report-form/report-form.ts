import { Component, inject, input, output, viewChild } from '@angular/core';
import { FormGroupDirective, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatError, MatFormField, MatHint, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { CreateTicketCommand } from '../../../domain/model/create-ticket.command';

/**
 * Form to report an incident. Limits match the API and database columns.
 */
@Component({
  selector: 'app-report-form',
  imports: [ReactiveFormsModule, MatFormField, MatLabel, MatHint, MatError, MatInput, MatButton],
  templateUrl: './report-form.html',
  styleUrl: './report-form.scss',
})
export class ReportForm {
  readonly isSaving = input(false);
  readonly submitted = output<CreateTicketCommand>();

  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly formDirective = viewChild.required(FormGroupDirective);

  protected readonly limits = { title: 120, assetCode: 30, description: 1000, reportedBy: 80 };

  protected readonly form = this.formBuilder.group({
    title: ['', [Validators.required, Validators.maxLength(this.limits.title)]],
    assetCode: ['', [Validators.required, Validators.maxLength(this.limits.assetCode)]],
    description: ['', [Validators.required, Validators.maxLength(this.limits.description)]],
    reportedBy: ['', [Validators.required, Validators.maxLength(this.limits.reportedBy)]],
  });

  protected submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const value = this.form.getRawValue();
    this.submitted.emit(new CreateTicketCommand(
      value.title.trim(), value.assetCode.trim(), value.description.trim(), value.reportedBy.trim(),
    ));
  }

  /**
   * Clears the values and the "submitted" state, so Angular Material does not show errors on the empty form.
   */
  reset(): void {
    this.formDirective().resetForm();
  }
}
