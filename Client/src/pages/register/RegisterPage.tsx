import { Navigate } from 'react-router';
import { RegisterForm } from '@/features/auth/ui/RegisterForm';
import { useAuthStore } from '@/features/auth/model/store';

export const RegisterPage = () => {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);

  if (isAuthenticated) {
    return <Navigate to="/catalog" replace />;
  }

  return (
      <div className="min-h-screen bg-[#0f1115]">
          <RegisterForm />
      </div>
  );
};
