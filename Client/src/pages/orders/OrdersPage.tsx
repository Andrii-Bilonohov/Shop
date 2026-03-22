import { AlertCircle } from 'lucide-react';
import { motion } from 'framer-motion';
import { Alert, AlertDescription, AlertTitle } from '@/app/components/ui/alert';
import { OrderList } from '@/widgets/order-list/OrderList';
import { useOrders } from '@/features/order/api/useOrders';

const OrderSkeleton = () => (
    <div className="bg-[#111217] rounded-2xl p-5 animate-pulse border border-white/5 space-y-4">
        <div className="flex justify-between items-start">
            <div className="space-y-2">
                <div className="h-4 w-36 bg-white/5 rounded-lg" />
                <div className="h-3 w-52 bg-white/5 rounded-lg" />
            </div>
            <div className="h-6 w-20 bg-white/5 rounded-full" />
        </div>
        <div className="flex justify-between items-center pt-1">
            <div className="flex gap-3">
                <div className="h-3 w-28 bg-white/5 rounded-lg" />
                <div className="h-3 w-16 bg-white/5 rounded-lg" />
            </div>
            <div className="h-5 w-16 bg-white/5 rounded-lg" />
        </div>
    </div>
);

export const OrdersPage = () => {
    const { data: orders, isLoading, error } = useOrders();

    return (
        <div className="min-h-screen bg-[#0a0b0f] text-gray-200">
            <div className="max-w-4xl mx-auto px-4 py-10">

                <motion.div
                    initial={{ opacity: 0, y: -12 }}
                    animate={{ opacity: 1, y: 0 }}
                    className="mb-10"
                >
                    <p className="text-xs uppercase tracking-widest text-gray-600 mb-1">History</p>
                    <div className="flex items-baseline gap-3">
                        <h1 className="text-5xl font-black text-white">Orders</h1>
                        {!isLoading && !error && orders && (
                            <span className="text-gray-600 text-xl">{orders.length} total</span>
                        )}
                    </div>
                </motion.div>

                {isLoading && (
                    <div className="space-y-3">
                        {Array.from({ length: 5 }).map((_, i) => (
                            <motion.div
                                key={i}
                                initial={{ opacity: 0, y: 8 }}
                                animate={{ opacity: 1, y: 0, transition: { delay: i * 0.05 } }}
                            >
                                <OrderSkeleton />
                            </motion.div>
                        ))}
                    </div>
                )}

                {error && (
                    <Alert variant="destructive" className="bg-red-950/40 border-red-800">
                        <AlertCircle className="h-4 w-4" />
                        <AlertTitle>Error</AlertTitle>
                        <AlertDescription>Failed to load orders. Please try again later.</AlertDescription>
                    </Alert>
                )}

                {!isLoading && !error && (
                    <OrderList orders={orders || []} />
                )}

            </div>
        </div>
    );
};