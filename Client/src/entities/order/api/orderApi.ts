import { apiClient } from '@/shared/api/client';
import type { Order, CreateOrderDto } from '../model/types';

export const orderApi = {
  getAll: async (): Promise<Order[]> => {
    const { data } = await apiClient.get<{ data: Order[] }>('/api/orders', {
    });
    return data.data;
  },

  getById: async (id: string): Promise<Order> => {
      const { data } = await apiClient.get<{ data: Order }>(`/api/orders/${id}`);
    return data.data;
  },

  create: async (orderData: CreateOrderDto): Promise<Order> => {
    const { data } = await apiClient.post<Order>('/api/orders', orderData);
    return data;
  },
};
