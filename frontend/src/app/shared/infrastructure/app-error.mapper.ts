import { HttpErrorResponse } from '@angular/common/http';
import { AppError, isAppError } from '../domain/model/app-error';

/**
 * RFC 7807 ProblemDetails payload returned by the .NET API.
 */
interface ProblemDetailsPayload {
  title?: string;
  detail?: string;
  status?: number;
  errors?: Record<string, string[]>;
}

/**
 * Converts any error into an AppError with a message the user can understand.
 * It reads the ProblemDetails returned by the backend (title, detail and validation errors).
 */
export function toAppError(error: unknown, operation?: string): AppError {
  if (isAppError(error)) return error;

  if (error instanceof HttpErrorResponse) {
    if (error.status === 0) {
      return new AppError('network', 0, 'Connection problem',
        'The server could not be reached. Check your connection and try again.', operation);
    }

    const payload = (typeof error.error === 'object' && error.error !== null
      ? error.error
      : {}) as ProblemDetailsPayload;
    const firstValidationMessage = payload.errors ? Object.values(payload.errors).flat()[0] : undefined;

    return new AppError(
      'http',
      error.status,
      payload.title ?? 'Request failed',
      payload.detail ?? firstValidationMessage ?? payload.title ?? 'The request could not be completed.',
      operation,
    );
  }

  return new AppError('unknown', 0, 'Unexpected error', 'Something went wrong. Please try again.', operation);
}