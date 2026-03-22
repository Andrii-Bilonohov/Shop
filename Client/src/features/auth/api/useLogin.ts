import { useMutation } from '@tanstack/react-query';
import { useNavigate } from 'react-router';
import { toast } from 'sonner';
import { authApi } from '@/entities/user/api/authApi';
import type { LoginCredentials } from '@/entities/user/model/types';
import { useAuthStore } from '../model/store';

export const useLogin = () => {
  const navigate = useNavigate();
  const setToken = useAuthStore((state) => state.setToken);

  return useMutation({
    mutationFn: (credentials: LoginCredentials) => authApi.login(credentials),
    onSuccess: (data) => {
      if (data.error) {
        toast.error(data.error);
        return;
      }

      if (data.accessToken) {
        setToken(data.accessToken);
        toast.success('Login successful!');
        navigate('/catalog');
      }
    },
    onError: (error: Error) => {
      toast.error(error.message || 'Login failed. Please try again.');
    },
  });
};
