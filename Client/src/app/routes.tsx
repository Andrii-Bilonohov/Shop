import { createBrowserRouter, Navigate } from 'react-router';
import { ProtectedRoute } from './providers/ProtectedRoute';
import { LoginPage } from '@/pages/login/LoginPage';
import { RegisterPage } from '@/pages/register/RegisterPage';
import { CatalogPage } from '@/pages/catalog/CatalogPage';
import { ProductDetailPage } from '@/pages/product/ProductDetailPage';
import { CartPage } from '@/pages/cart/CartPage';
import { OrdersPage } from '@/pages/orders/OrdersPage';
import { OrderDetailPage } from '@/pages/order/OrderDetailPage';
import {HomeRedirect} from "@/app/providers/HomeRedirect.tsx";

export const router = createBrowserRouter([
  {
    path: '/',
    element: <HomeRedirect />,
  },
  {
    path: '/login',
    element: <LoginPage />,
  },
  {
    path: '/register',
    element: <RegisterPage />,
  },
  {
    element: <ProtectedRoute />,
    children: [
      {
        path: '/catalog',
        element: <CatalogPage />,
      },
      {
        path: '/product/:id',
        element: <ProductDetailPage />,
      },
      {
        path: '/cart',
        element: <CartPage />,
      },
      {
        path: '/orders',
        element: <OrdersPage />,
      },
      {
        path: '/order/:id',
        element: <OrderDetailPage />,
      },
    ],
  },
  {
    path: '*',
    element: <Navigate to="/" />,
  },
]);
