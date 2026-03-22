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
    const token = localStorage.getItem('accessToken');
    
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }

      if (config.headers) {
          config.headers['X-Api-Gateway'] = ENV.API_GATEWAY_KEY;
      }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

apiClient.interceptors.response.use(
    (response) => response,
    (error: AxiosError) => {
        if (error.response?.status === 401) {
            localStorage.removeItem('accessToken');
            window.location.href = '/login';
        }
        return Promise.reject(error);
    })
