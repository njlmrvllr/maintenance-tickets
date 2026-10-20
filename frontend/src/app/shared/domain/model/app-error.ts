export type AppErrorKind = 'http' | 'network' | 'unknown';

/**
 * Error raised by the infrastructure layer in a shape the UI can show to the user.
 */
export class AppError extends Error {
  constructor(
    readonly kind: AppErrorKind,
    readonly status: number,
    readonly title: string,
    readonly detail: string,
    readonly operation?: string,
  ) {
    super(detail);
    this.name = 'AppError';
  }
}

export function isAppError(error: unknown): error is AppError {
  return error instanceof AppError;
}