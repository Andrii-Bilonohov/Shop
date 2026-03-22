export const ENV = {
  API_BASE_URL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:8080',
    API_GATEWAY_KEY: import.meta.env.VITE_API_GATEWAY_KEY || 'GATEWAY_SECRET_KEY_123',
} as const;
