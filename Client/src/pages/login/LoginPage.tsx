import { Navigate } from 'react-router';
import { LoginForm } from '@/features/auth/ui/LoginForm';
import { useAuthStore } from '@/features/auth/model/store';

export const LoginPage = () => {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);

  if (isAuthenticated) {
    return <Navigate to="/"  />;
  }

  return (
      <div className="min-h-screen bg-[#0f1115]">
          <LoginForm />
      </div>
  );
};
