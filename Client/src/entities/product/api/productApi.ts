import { apiClient } from '@/shared/api/client';
import type { Product } from '../model/types';

export const productApi = {
    getAll: async (): Promise<Product[]> => {
        const response = await apiClient.get<{ data: Product[] }>('/api/items');
        return response.data.data;
    },

    getById: async (id: string): Promise<Product> => {
        const response = await apiClient.get<{ data: Product }>('/api/items/' + id);
        return response.data.data;
    },
};
