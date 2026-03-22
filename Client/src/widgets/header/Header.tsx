import { Link, useNavigate } from 'react-router';
import { ShoppingCart, LogOut, Package, Store } from 'lucide-react';
import { useAuthStore } from '@/features/auth/model/store';
import { useCartStore } from '@/features/cart/model/store';

export const Header = () => {
    const navigate = useNavigate();
    const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
    const clearToken = useAuthStore((state) => state.clearToken);
    const totalItems = useCartStore((state) => state.getTotalItems());

    const handleLogout = () => {
        clearToken();
        navigate('/');
    };

    return (
        <header className="sticky top-0 z-50 w-full bg-[#0f1115]/80 backdrop-blur-md border-b border-white/5">
            <div className="max-w-7xl mx-auto flex h-16 items-center justify-between px-4">

                <Link to="/catalog" className="flex items-center gap-2 group">
                    <div className="w-8 h-8 rounded-lg bg-gradient-to-tr from-purple-600 to-indigo-500 flex items-center justify-center shadow-lg shadow-purple-900/40">
                        <Store className="h-4 w-4 text-white" />
                    </div>
                    <span className="text-white font-bold text-lg tracking-tight">
                        E-Commerce
                    </span>
                </Link>

                {isAuthenticated && (
                    <nav className="flex items-center gap-1">
                        <Link
                            to="/catalog"
                            className="flex items-center gap-2 px-3 py-2 rounded-lg text-gray-400 hover:text-white hover:bg-white/5 transition text-sm"
                        >
                            <Store className="h-4 w-4" />
                            Catalog
                        </Link>

                        <Link
                            to="/orders"
                            className="flex items-center gap-2 px-3 py-2 rounded-lg text-gray-400 hover:text-white hover:bg-white/5 transition text-sm"
                        >
                            <Package className="h-4 w-4" />
                            Orders
                        </Link>

                        <Link
                            to="/cart"
                            className="flex items-center gap-2 px-3 py-2 rounded-lg text-gray-400 hover:text-white hover:bg-white/5 transition text-sm relative"
                        >
                            <ShoppingCart className="h-4 w-4" />
                            Cart
                            {totalItems > 0 && (
                                <span className="absolute -top-0.5 -right-0.5 min-w-[18px] h-[18px] rounded-full bg-green-500 text-black text-[10px] font-bold flex items-center justify-center px-1">
                                    {totalItems}
                                </span>
                            )}
                        </Link>

                        <div className="w-px h-5 bg-gray-700 mx-1" />

                        <button
                            onClick={handleLogout}
                            className="flex items-center gap-2 px-3 py-2 rounded-lg text-gray-400 hover:text-red-400 hover:bg-red-900/10 transition text-sm cursor-pointer"
                        >
                            <LogOut className="h-4 w-4" />
                            Logout
                        </button>
                    </nav>
                )}
            </div>
        </header>
    );
};