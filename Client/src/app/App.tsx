import { RouterProvider } from 'react-router';
import { Toaster } from './components/ui/sonner';
import { QueryProvider } from './providers/QueryProvider';
import { router } from './routes';

export default function App() {
  return (
    <QueryProvider>
      <RouterProvider router={router} />
      <Toaster />
    </QueryProvider>
  );
}
