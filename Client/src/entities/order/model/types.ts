export interface Order {
  id: string;
  userId: string;
  title: string;
  description: string;
  totalPrice: number;
  totalItems: number;
  status: 'created' | 'paid' | 'shipped' | 'completed' | 'cancelled';
  itemsId: string[];
  createdAt: string;
  updatedAt?: string;
}

export interface CreateOrderDto {
    userId: string;
    title: string;
    description: string;
    totalPrice: number;
    totalItems: number;
    itemsId: string[];
}
