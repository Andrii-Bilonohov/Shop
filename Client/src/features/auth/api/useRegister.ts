import { useMutation } from '@tanstack/react-query';
import { useNavigate } from 'react-router';
import { toast } from 'sonner';
import { authApi } from '@/entities/user/api/authApi';
import type { RegisterCredentials } from '@/entities/user/model/types';
import { useAuthStore } from '../model/store';

export const useRegister = () => {
  const navigate = useNavigate();
  const setToken = useAuthStore((state) => state.setToken);

  return useMutation({
    mutationFn: (credentials: RegisterCredentials) => authApi.register(credentials),
    onSuccess: (data) => {
      if (data.error) {
        toast.error(data.error);
        return;
      }

      if (data.accessToken) {
        setToken(data.accessToken);
        toast.success('Registration successful!');
        navigate('/catalog');
      }
    },
    onError: (error: Error) => {
      toast.error(error.message || 'Registration failed. Please try again.');
    },
  });
};
