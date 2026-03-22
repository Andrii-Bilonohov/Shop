import { motion, AnimatePresence } from 'framer-motion';
import { ShoppingBag, Clock, CheckCircle2, Truck, XCircle, CircleDot } from 'lucide-react';
import { useNavigate } from 'react-router';
import type { Order } from '@/entities/order/model/types';

type Props = {
    orders: Order[];
};

const STATUS_CONFIG: Record<Order['status'], { label: string; color: string; bg: string; icon: React.ReactNode }> = {
    created:   { label: 'Created',   color: 'text-blue-400',   bg: 'bg-blue-900/30 border-blue-800/40',   icon: <CircleDot className="h-3.5 w-3.5" /> },
    paid:      { label: 'Paid',      color: 'text-purple-400', bg: 'bg-purple-900/30 border-purple-800/40', icon: <CheckCircle2 className="h-3.5 w-3.5" /> },
    shipped:   { label: 'Shipped',   color: 'text-yellow-400', bg: 'bg-yellow-900/30 border-yellow-800/40', icon: <Truck className="h-3.5 w-3.5" /> },
    completed: { label: 'Completed', color: 'text-green-400',  bg: 'bg-green-900/30 border-green-800/40',  icon: <CheckCircle2 className="h-3.5 w-3.5" /> },
    cancelled: { label: 'Cancelled', color: 'text-red-400',    bg: 'bg-red-900/30 border-red-800/40',      icon: <XCircle className="h-3.5 w-3.5" /> },
};

const formatDate = (iso: string) =>
    new Intl.DateTimeFormat('en-US', { day: 'numeric', month: 'short', year: 'numeric', hour: '2-digit', minute: '2-digit' }).format(new Date(iso));

export const OrderList = ({ orders }: Props) => {
    const navigate = useNavigate();

    if (orders.length === 0) {
        return (
            <motion.div
                initial={{ opacity: 0, y: 16 }}
                animate={{ opacity: 1, y: 0 }}
                className="flex flex-col items-center justify-center py-24 text-center"
            >
                <div className="w-20 h-20 rounded-2xl bg-[#111217] flex items-center justify-center mb-5">
                    <ShoppingBag className="h-9 w-9 text-gray-700" />
                </div>
                <h2 className="text-white font-semibold text-lg mb-2">No orders yet</h2>
                <p className="text-gray-600 text-sm">Your completed orders will appear here</p>
            </motion.div>
        );
    }

    return (
        <div className="space-y-3">
            <AnimatePresence>
                {orders.map((order, i) => {
                    const status = STATUS_CONFIG[order.status.toLowerCase() as Order['status']] ?? STATUS_CONFIG['created'];
                    return (
                        <motion.div
                            key={order.id}
                            initial={{ opacity: 0, y: 16 }}
                            animate={{ opacity: 1, y: 0, transition: { delay: i * 0.06 } }}
                            exit={{ opacity: 0, x: -20 }}
                            onClick={() => navigate(`/order/${order.id}`)}
                            className="bg-[#111217] rounded-2xl p-5 cursor-pointer hover:bg-[#16171e] transition-colors group"
                        >
                            <div className="flex items-start justify-between gap-4">
                                <div className="flex-1 min-w-0">
                                    <div className="flex items-center gap-2 mb-1">
                                        <h3 className="text-white font-semibold truncate group-hover:text-purple-300 transition-colors">
                                            {order.title}
                                        </h3>
                                    </div>

                                    {order.description && (
                                        <p className="text-gray-500 text-sm truncate mb-3">
                                            {order.description}
                                        </p>
                                    )}

                                    <div className="flex items-center gap-4 text-xs text-gray-600">
                                        <span className="flex items-center gap-1">
                                            <Clock className="h-3 w-3" />
                                            {formatDate(order.createdAt)}
                                        </span>
                                        <span>{order.totalItems} item{order.totalItems !== 1 ? 's' : ''}</span>
                                        <span className="font-mono text-gray-700">#{order.id.slice(0, 8)}</span>
                                    </div>
                                </div>

                                <div className="flex flex-col items-end gap-3 shrink-0">
                                    <span className={`flex items-center gap-1.5 text-xs font-medium px-2.5 py-1 rounded-full border ${status.color} ${status.bg}`}>
                                        {status.icon}
                                        {status.label}
                                    </span>
                                    <span className="text-green-400 font-bold text-lg">
                                        ${order.totalPrice.toLocaleString()}
                                    </span>
                                </div>
                            </div>
                        </motion.div>
                    );
                })}
            </AnimatePresence>
        </div>
    );
};