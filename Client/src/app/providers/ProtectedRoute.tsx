import { useState, useEffect } from 'react';
import { Navigate, Outlet } from 'react-router';
import { useAuthStore } from '@/features/auth/model/store';
import { Header } from '@/widgets/header/Header';
import {Footer} from "@/widgets/footer/Footer.tsx";

export const ProtectedRoute = () => {
    const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
    const [hydrated, setHydrated] = useState(() => {
        try {
            const stored = localStorage.getItem('auth-storage');
            if (stored) {
                const parsed = JSON.parse(stored);
                return !!parsed?.state?.accessToken;
            }
        } catch {}
        return false;
    });

    useEffect(() => {
        setHydrated(true);
    }, []);

    if (!hydrated && !isAuthenticated) {
        return (
            <div className="min-h-screen bg-[#0f1115] flex items-center justify-center">
                <div className="w-8 h-8 rounded-full border-2 border-purple-500 border-t-transparent animate-spin" />
            </div>
        );
    }

    if (!isAuthenticated) {
        return <Navigate to="/login" replace />;
    }

    return (
        <div className="min-h-screen bg-[#0f1115]">
            <Header />
            <Outlet />
            <Footer />
        </div>
    );
};