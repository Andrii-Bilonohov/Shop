export interface Product {
  id: string;
  name: string;
  description: string;
  category: string;
  rating?: number;
  price: number;
  stock: number;
  weight: number;
  imageUrl: string;
}

export type ProductsPage = {
    limit: number;
    offset: number;
    items: number;
    pages: number;
    data: Product[];
};

export type SortOption = 'price-asc' | 'price-desc' | 'rating' | 'name';