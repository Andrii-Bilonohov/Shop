import { useState } from 'react';
import { useNavigate } from 'react-router';
import { motion, AnimatePresence } from 'framer-motion';
import { Trash2, ShoppingBag, Loader2, ArrowLeft, Package, Minus, Plus } from 'lucide-react';
import { useCartStore } from '@/features/cart/model/store';
import { useCreateOrder } from '@/features/order/api/useOrders';
import { useAuthStore } from "@/features/auth";

const getUserId = (token: string | null): string => {
    if (!token) return '';
    try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        return payload.sub ?? payload.userId ?? payload.id ?? '';
    } catch { return ''; }
};

export const CartPage = () => {
    const navigate = useNavigate();
    const accessToken = useAuthStore((state) => state.accessToken);
    const items = useCartStore((state) => state.items);
    const updateQuantity = useCartStore((state) => state.updateQuantity);
    const removeItem = useCartStore((state) => state.removeItem);
    const clearCart = useCartStore((state) => state.clearCart);
    const getTotalAmount = useCartStore((state) => state.getTotalAmount());
    const { mutate: createOrder, isPending } = useCreateOrder();
    const [imgErrors, setImgErrors] = useState<Record<string, boolean>>({});

    const handleCheckout = () => {
        const orderData = {
            userId: getUserId(accessToken),
            title: `Order #${Date.now()}`,
            description: `${items.length} item(s)`,
            totalPrice: getTotalAmount,
            totalItems: items.reduce((sum, i) => sum + i.quantity, 0),
            itemsId: items.map((i) => i.product.id),
        };
        createOrder(orderData, {
            onSuccess: () => { clearCart(); navigate('/orders'); },
        });
    };

    if (items.length === 0) {
        return (
            <div className="min-h-screen bg-[#0a0b0f] flex items-center justify-center px-4">
                <motion.div initial={{ opacity: 0, y: 24 }} animate={{ opacity: 1, y: 0 }} className="text-center">
                    <div className="w-28 h-28 mx-auto mb-8 rounded-3xl bg-[#111217] border border-white/5 flex items-center justify-center">
                        <ShoppingBag className="h-12 w-12 text-gray-700" />
                    </div>
                    <h1 className="text-3xl font-black text-white mb-3">Cart is empty</h1>
                    <p className="text-gray-600 mb-8">Browse our catalog and add something you like</p>
                    <motion.button
                        whileHover={{ scale: 1.03 }} whileTap={{ scale: 0.97 }}
                        onClick={() => navigate('/catalog')}
                        className="flex items-center gap-2 px-8 py-3.5 bg-white text-black font-bold rounded-xl mx-auto cursor-pointer hover:bg-gray-100 transition-colors"
                    >
                        <ArrowLeft className="h-4 w-4" /> Browse products
                    </motion.button>
                </motion.div>
            </div>
        );
    }

    const totalItems = items.reduce((sum, i) => sum + i.quantity, 0);

    return (
        <div className="min-h-screen bg-[#0a0b0f] text-gray-200">
            <div className="max-w-6xl mx-auto px-4 py-10">

                <motion.div initial={{ opacity: 0, y: -12 }} animate={{ opacity: 1, y: 0 }} className="mb-10">
                    <button
                        onClick={() => navigate('/catalog')}
                        className="flex items-center gap-2 text-gray-600 hover:text-white transition-colors group mb-6 cursor-pointer text-sm"
                    >
                        <ArrowLeft className="h-4 w-4 group-hover:-translate-x-1 transition-transform" />
                        Back to catalog
                    </button>
                    <div className="flex items-baseline gap-3">
                        <h1 className="text-5xl font-black text-white">Cart</h1>
                        <span className="text-gray-600 text-xl">{totalItems} {totalItems === 1 ? 'item' : 'items'}</span>
                    </div>
                </motion.div>

                <div className="grid lg:grid-cols-5 gap-8">

                    <div className="lg:col-span-3 space-y-3">
                        <AnimatePresence>
                            {items.map((item, i) => (
                                <motion.div
                                    key={item.product.id}
                                    layout
                                    initial={{ opacity: 0, y: 16 }}
                                    animate={{ opacity: 1, y: 0, transition: { delay: i * 0.05 } }}
                                    exit={{ opacity: 0, x: -30, transition: { duration: 0.2 } }}
                                    className="group bg-[#111217] rounded-2xl p-4 border border-white/5 hover:border-white/10 transition-colors"
                                >
                                    <div className="flex gap-4 items-center">
                                        <div className="w-18 h-18 w-[72px] h-[72px] rounded-xl overflow-hidden shrink-0 bg-[#0a0b0f]">
                                            {item.product.imageUrl && !imgErrors[item.product.id] ? (
                                                <img
                                                    src={item.product.imageUrl}
                                                    alt={item.product.name}
                                                    className="w-full h-full object-cover"
                                                    onError={() => setImgErrors(p => ({ ...p, [item.product.id]: true }))}
                                                />
                                            ) : (
                                                <div className="w-full h-full flex items-center justify-center">
                                                    <Package className="h-6 w-6 text-gray-700" />
                                                </div>
                                            )}
                                        </div>

                                        <div className="flex-1 min-w-0">
                                            <h3 className="text-white font-semibold truncate">{item.product.name}</h3>
                                            {item.product.category && (
                                                <span className="text-xs text-gray-600">{item.product.category}</span>
                                            )}
                                            <div className="text-green-400 font-bold mt-0.5">
                                                ${item.product.price.toLocaleString()}
                                            </div>
                                        </div>

                                        <div className="flex flex-col items-end gap-3 shrink-0">
                                            <span className="text-white font-bold text-lg">
                                                ${(item.product.price * item.quantity).toLocaleString()}
                                            </span>

                                            <div className="flex items-center gap-1">
                                                <button
                                                    onClick={() => updateQuantity(item.product.id, item.quantity - 1)}
                                                    className="w-7 h-7 rounded-lg bg-white/5 hover:bg-white/10 flex items-center justify-center transition-colors cursor-pointer"
                                                >
                                                    <Minus className="h-3 w-3 text-gray-400" />
                                                </button>
                                                <span className="w-8 text-center text-white text-sm font-medium">{item.quantity}</span>
                                                <button
                                                    onClick={() => updateQuantity(item.product.id, item.quantity + 1)}
                                                    className="w-7 h-7 rounded-lg bg-white/5 hover:bg-white/10 flex items-center justify-center transition-colors cursor-pointer"
                                                >
                                                    <Plus className="h-3 w-3 text-gray-400" />
                                                </button>
                                                <div className="w-px h-4 bg-white/10 mx-1" />
                                                <button
                                                    onClick={() => removeItem(item.product.id)}
                                                    className="w-7 h-7 rounded-lg hover:bg-red-900/30 flex items-center justify-center transition-colors cursor-pointer"
                                                >
                                                    <Trash2 className="h-3.5 w-3.5 text-gray-600 hover:text-red-400 transition-colors" />
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </motion.div>
                            ))}
                        </AnimatePresence>

                        <motion.div initial={{ opacity: 0 }} animate={{ opacity: 1, transition: { delay: 0.3 } }}>
                            <button
                                onClick={clearCart}
                                className="text-xs text-gray-700 hover:text-red-400 transition-colors cursor-pointer mt-2"
                            >
                                Clear all items
                            </button>
                        </motion.div>
                    </div>

                    <motion.div
                        initial={{ opacity: 0, y: 20 }}
                        animate={{ opacity: 1, y: 0, transition: { delay: 0.15 } }}
                        className="lg:col-span-2"
                    >
                        <div className="sticky top-6 bg-[#111217] rounded-2xl border border-white/5 overflow-hidden">
                            <div className="p-6 border-b border-white/5">
                                <h2 className="text-white font-bold text-lg">Order summary</h2>
                            </div>

                            <div className="p-6 space-y-4">
                                <div className="space-y-3 text-sm">
                                    {items.map(item => (
                                        <div key={item.product.id} className="flex justify-between text-gray-500">
                                            <span className="truncate mr-4">{item.product.name} × {item.quantity}</span>
                                            <span className="shrink-0">${(item.product.price * item.quantity).toLocaleString()}</span>
                                        </div>
                                    ))}
                                </div>

                                <div className="h-px bg-white/5" />

                                <div className="flex justify-between text-sm text-gray-500">
                                    <span>Shipping</span>
                                    <span className="text-green-400 font-medium">Free</span>
                                </div>

                                <div className="h-px bg-white/5" />

                                <div className="flex justify-between text-white font-black text-2xl">
                                    <span>Total</span>
                                    <span>${getTotalAmount.toLocaleString()}</span>
                                </div>
                            </div>

                            <div className="px-6 pb-6">
                                <motion.button
                                    whileHover={{ scale: 1.02 }}
                                    whileTap={{ scale: 0.98 }}
                                    onClick={handleCheckout}
                                    disabled={isPending}
                                    className="w-full h-13 py-4 rounded-xl bg-white text-black font-bold text-sm flex items-center justify-center gap-2 cursor-pointer hover:bg-gray-100 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                                >
                                    {isPending
                                        ? <><Loader2 className="h-4 w-4 animate-spin" /> Processing...</>
                                        : 'Checkout'
                                    }
                                </motion.button>
                            </div>
                        </div>
                    </motion.div>

                </div>
            </div>
        </div>
    );
};