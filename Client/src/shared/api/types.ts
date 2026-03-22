export interface ApiError {
  error: string | null;
  message?: string;
}

export interface PaginationParams {
  limit?: number;
  offset?: number;
}
