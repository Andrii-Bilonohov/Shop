import { apiClient } from '@/shared/api/client';
import type { AuthResponse, LoginCredentials, RegisterCredentials } from '../model/types';

export const authApi = {
  login: async (credentials: LoginCredentials): Promise<AuthResponse> => {
    const { data } = await apiClient.post<AuthResponse>('/api/login', credentials);
    return data;
  },

  register: async (credentials: RegisterCredentials): Promise<AuthResponse> => {
    const { data } = await apiClient.post<AuthResponse>('/api/register', credentials);
    return data;
  },
};
