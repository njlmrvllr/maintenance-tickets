import { HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { toAppError } from './app-error.mapper';

/**
 * Base class for endpoint classes. Converts HTTP errors into AppError.
 */
export abstract class ErrorHandlingEnabledBaseType {
  protected handleError(operation: string) {
    return (error: HttpErrorResponse): Observable<never> => throwError(() => toAppError(error, operation));
  }
}