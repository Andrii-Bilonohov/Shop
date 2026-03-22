import { Navigate } from 'react-router';
import { useAuthStore } from '@/features/auth/model/store';
import { GuestHomePage } from '@/pages/guest-home-page/GuestHomePage';

export const HomeRedirect = () => {
    const isAuthenticated = useAuthStore((state) => state.isAuthenticated);

    return isAuthenticated
        ? <Navigate to="/catalog" replace />
        : <GuestHomePage />;
};