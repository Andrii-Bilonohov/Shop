import { useState } from 'react';
import { useParams, useNavigate } from 'react-router';
import { motion } from 'framer-motion';
import { ArrowLeft, AlertCircle, Clock, Package, Hash, CheckCircle2, Truck, XCircle, CircleDot, CreditCard, Copy, Check } from 'lucide-react';
import { Alert, AlertDescription, AlertTitle } from '@/app/components/ui/alert';
import { useOrder } from '@/features/order/api/useOrders';
import { useProductsByIds } from '@/features/product/api/useProductsByIds';

const STATUS_CONFIG: Record<string, { label: string; color: string; bg: string; icon: React.ReactNode }> = {
    created:   { label: 'Created',   color: 'text-blue-400',   bg: 'bg-blue-900/30 border-blue-800/40',    icon: <CircleDot className="h-4 w-4" /> },
    paid:      { label: 'Paid',      color: 'text-purple-400', bg: 'bg-purple-900/30 border-purple-800/40', icon: <CheckCircle2 className="h-4 w-4" /> },
    shipped:   { label: 'Shipped',   color: 'text-yellow-400', bg: 'bg-yellow-900/30 border-yellow-800/40', icon: <Truck className="h-4 w-4" /> },
    completed: { label: 'Completed', color: 'text-green-400',  bg: 'bg-green-900/30 border-green-800/40',   icon: <CheckCircle2 className="h-4 w-4" /> },
    cancelled: { label: 'Cancelled', color: 'text-red-400',    bg: 'bg-red-900/30 border-red-800/40',       icon: <XCircle className="h-4 w-4" /> },
};

const formatDate = (iso: string) =>
    new Intl.DateTimeFormat('en-US', {
        day: 'numeric', month: 'long', year: 'numeric',
        hour: '2-digit', minute: '2-digit',
    }).format(new Date(iso));

const fadeUp = {
    hidden: { opacity: 0, y: 16 },
    visible: (i: number) => ({ opacity: 1, y: 0, transition: { delay: i * 0.07, duration: 0.4 } }),
};

export const OrderDetailPage = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { data: order, isLoading, error } = useOrder(id!);
    const { products, isLoading: productsLoading } = useProductsByIds(order?.itemsId ?? []);
    const [copied, setCopied] = useState(false);

    const handleCopyId = () => {
        navigator.clipboard.writeText(order?.id ?? '');
        setCopied(true);
        setTimeout(() => setCopied(false), 2000);
    };

    if (isLoading) {
        return (
            <div className="min-h-screen bg-[#0f1115] flex items-center justify-center">
                <div className="flex flex-col items-center gap-4">
                    <div className="w-10 h-10 rounded-full border-2 border-purple-500 border-t-transparent animate-spin" />
                    <span className="text-gray-500 text-sm tracking-widest uppercase">Loading</span>
                </div>
            </div>
        );
    }

    if (error || !order) {
        return (
            <div className="min-h-screen bg-[#0f1115] flex items-center justify-center px-4">
                <Alert variant="destructive" className="max-w-md bg-red-950/40 border-red-800">
                    <AlertCircle className="h-4 w-4" />
                    <AlertTitle>Error</AlertTitle>
                    <AlertDescription>Failed to load order details. Please try again later.</AlertDescription>
                </Alert>
            </div>
        );
    }

    const statusKey = (order?.status ?? 'created').toLowerCase();
    const status = STATUS_CONFIG[statusKey] ?? STATUS_CONFIG['created'];

    return (
        <div className="min-h-screen bg-[#0f1115] text-gray-200">
            <div className="pointer-events-none fixed top-0 left-1/2 -translate-x-1/2 w-[600px] h-[300px] bg-purple-700/10 blur-[120px] rounded-full" />

            <div className="max-w-3xl mx-auto px-4 py-10 relative">

                <motion.button
                    initial={{ opacity: 0, x: -12 }}
                    animate={{ opacity: 1, x: 0 }}
                    onClick={() => navigate('/orders')}
                    className="flex items-center gap-2 text-gray-400 hover:text-white transition mb-10 group cursor-pointer"
                >
                    <ArrowLeft className="h-4 w-4 group-hover:-translate-x-1 transition-transform" />
                    <span className="text-sm">Back to Orders</span>
                </motion.button>

                <motion.div custom={0} variants={fadeUp} initial="hidden" animate="visible"
                            className="flex items-start justify-between gap-4 mb-8"
                >
                    <div>
                        <h1 className="text-4xl font-bold text-white mb-1">{order.title}</h1>
                        {order.description && (
                            <p className="text-gray-500 text-sm">{order.description}</p>
                        )}
                    </div>
                    <span className={`flex items-center gap-2 px-3 py-1.5 rounded-full border text-sm font-medium shrink-0 ${status.color} ${status.bg}`}>
                        {status.icon}
                        {status.label}
                    </span>
                </motion.div>

                <motion.div custom={1} variants={fadeUp} initial="hidden" animate="visible"
                            className="grid grid-cols-2 sm:grid-cols-4 gap-3 mb-6"
                >
                    <div
                        className="bg-[#1b1c22] rounded-2xl p-4 cursor-pointer hover:bg-[#22232d] transition-colors group"
                        onClick={handleCopyId}
                        title="Click to copy full ID"
                    >
                        <div className="flex items-center gap-2 text-gray-600 text-xs uppercase tracking-wider mb-1">
                            <Hash className="h-3 w-3" /> ID
                            {copied
                                ? <Check className="h-3 w-3 text-green-400 ml-auto" />
                                : <Copy className="h-3 w-3 ml-auto opacity-0 group-hover:opacity-100 transition-opacity" />
                            }
                        </div>
                        <div className={`font-mono text-sm transition-colors ${copied ? 'text-green-400' : 'text-white'}`}>
                            {copied ? 'Copied!' : `${order.id.slice(0, 8)}...`}
                        </div>
                    </div>
                    <div className="bg-[#1b1c22] rounded-2xl p-4">
                        <div className="flex items-center gap-2 text-gray-600 text-xs uppercase tracking-wider mb-1">
                            <Package className="h-3 w-3" /> Items
                        </div>
                        <div className="text-white font-semibold">{order.totalItems}</div>
                    </div>
                    <div className="bg-[#1b1c22] rounded-2xl p-4 sm:col-span-2">
                        <div className="flex items-center gap-2 text-gray-600 text-xs uppercase tracking-wider mb-1">
                            <Clock className="h-3 w-3" /> Placed
                        </div>
                        <div className="text-white text-sm">{formatDate(order.createdAt)}</div>
                    </div>
                </motion.div>

                <motion.div custom={2} variants={fadeUp} initial="hidden" animate="visible"
                            className="bg-[#1b1c22] rounded-2xl p-6 mb-4"
                >
                    <h2 className="text-xs uppercase tracking-widest text-gray-500 mb-4">Products</h2>

                    {productsLoading ? (
                        <div className="space-y-3">
                            {order.itemsId.map((itemId) => (
                                <div key={itemId} className="h-16 bg-[#13141a] rounded-xl animate-pulse" />
                            ))}
                        </div>
                    ) : (
                        <div className="space-y-2">
                            {products.map((product) => (
                                <div key={product!.id}
                                     className="flex items-center gap-4 p-3 rounded-xl bg-[#13141a] hover:bg-[#1a1b25] transition-colors"
                                >
                                    <div className="w-12 h-12 rounded-lg overflow-hidden bg-[#0f1115] shrink-0">
                                        {product!.imageUrl ? (
                                            <img
                                                src={product!.imageUrl}
                                                alt={product!.name}
                                                className="w-full h-full object-cover"
                                                onError={(e) => { e.currentTarget.style.display = 'none'; }}
                                            />
                                        ) : (
                                            <div className="w-full h-full flex items-center justify-center">
                                                <Package className="h-5 w-5 text-gray-700" />
                                            </div>
                                        )}
                                    </div>

                                    <div className="flex-1 min-w-0">
                                        <div className="text-white font-medium truncate">{product!.name}</div>
                                        <div className="text-gray-600 text-xs">{product!.category}</div>
                                    </div>

                                    <div className="text-green-400 font-bold shrink-0">
                                        ${product!.price.toLocaleString()}
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </motion.div>

                <motion.div custom={3} variants={fadeUp} initial="hidden" animate="visible"
                            className="bg-[#1b1c22] rounded-2xl p-6"
                >
                    <div className="flex justify-between text-gray-400 text-sm mb-3">
                        <span>Subtotal</span>
                        <span className="text-white">${order.totalPrice.toLocaleString()}</span>
                    </div>
                    <div className="flex justify-between text-gray-400 text-sm mb-4">
                        <span>Shipping</span>
                        <span className="text-green-400">Free</span>
                    </div>
                    <div className="h-px bg-gradient-to-r from-transparent via-gray-700 to-transparent mb-4" />
                    <div className="flex justify-between text-white font-bold text-2xl">
                        <span>Total</span>
                        <span className="bg-gradient-to-r from-green-400 to-emerald-300 bg-clip-text text-transparent">
                            ${order.totalPrice.toLocaleString()}
                        </span>
                    </div>

                    {statusKey === 'created' && (
                        <>
                            <div className="h-px bg-gradient-to-r from-transparent via-gray-700 to-transparent mt-4" />
                            <motion.button
                                whileHover={{ scale: 1.02 }}
                                whileTap={{ scale: 0.98 }}
                                className="w-full mt-4 h-14 rounded-2xl bg-gradient-to-r from-green-500 to-emerald-400 text-black font-bold text-base flex items-center justify-center gap-3 cursor-pointer shadow-lg shadow-green-900/30 hover:shadow-green-900/50 transition-shadow"
                            >
                                <CreditCard className="h-5 w-5" />
                                Pay ${order.totalPrice.toLocaleString()}
                            </motion.button>
                        </>
                    )}
                </motion.div>

            </div>
        </div>
    );
};