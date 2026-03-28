import type { InternalAxiosRequestConfig } from "axios";
import axios, { AxiosError } from 'axios';
import { ENV } from '../config/env';

export const apiClient = axios.create({
  baseURL: ENV.API_BASE_URL,
  headers: {
      'Content-Type': 'application/json',
      'X-Api-Gateway': ENV.API_GATEWAY_KEY,
  },
});

apiClient.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        let token = localStorage.getItem('accessToken');

        if (!token) {
            try {
                const authStorage = localStorage.getItem('auth-storage');
                if (authStorage) {
                    token = JSON.parse(authStorage)?.state?.accessToken;
                }
            } catch {
                console.log('error');
            }
        }

        if (token && config.headers) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        if (config.headers) {
            config.headers['X-Api-Gateway'] = ENV.API_GATEWAY_KEY;
        }
        return config;
    }
);

apiClient.interceptors.response.use(
    (response) => response,
    (error: AxiosError) => {
        if (error.response?.status === 401) {
            localStorage.removeItem('accessToken');
        }
        return Promise.reject(error);
    }
);
