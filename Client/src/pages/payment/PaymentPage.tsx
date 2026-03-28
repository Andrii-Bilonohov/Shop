import { useState } from 'react';
import { useParams, useNavigate } from 'react-router';
import { motion, AnimatePresence } from 'framer-motion';
import { ArrowLeft, CreditCard, Bitcoin, Loader2, CheckCircle2, XCircle, Lock } from 'lucide-react';
import { useOrder } from '@/features/order/api/useOrders';
import { useAuthStore } from '@/features/auth/model/store';
import { apiClient } from '@/shared/api/client';

type PaymentMethod = 'card' | 'crypto';
type PaymentStatus = 'idle' | 'loading' | 'success' | 'error';

const getUserId = (token: string | null): string => {
    if (!token) return '';
    try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        return payload.sub ?? payload.userId ?? payload.id ?? '';

    } catch { return ''; }
};

export const PaymentPage = () => {
    const { orderId } = useParams<{ orderId: string }>();
    const navigate = useNavigate();
    const accessToken = useAuthStore((state) => state.accessToken);
    const { data: order, isLoading } = useOrder(orderId!);

    const [method, setMethod] = useState<PaymentMethod>('card');
    const [status, setStatus] = useState<PaymentStatus>('idle');
    const [error, setError] = useState<string | null>(null);
    const [liqpayData, setLiqpayData] = useState<{ data: string; signature: string } | null>(null);

    // Card form state
    const [cardNumber, setCardNumber] = useState('');
    const [expiry, setExpiry] = useState('');
    const [cvv, setCvv] = useState('');

    const formatCardNumber = (v: string) =>
        v.replace(/\D/g, '').slice(0, 16).replace(/(.{4})/g, '$1 ').trim();

    const formatExpiry = (v: string) => {
        const digits = v.replace(/\D/g, '').slice(0, 4);
        return digits.length >= 2 ? `${digits.slice(0, 2)}/${digits.slice(2)}` : digits;
    };

    const handlePay = async () => {
        if (!order) return;
        setStatus('loading');
        setError(null);

        try {
            const { data } = await apiClient.post(`/api/payments/${orderId}/pay`, {
                userId: getUserId(accessToken),
                method: method === 'card',
                amount: order.totalPrice,
            });

            console.log(data);

            setLiqpayData({ data: data.data, signature: data.signature });
            setStatus('success');
        } catch {
            // setError(e?.response?.data?.error || 'Payment failed. Please try again.');
            setStatus('error');
        }
    };

    if (isLoading) {
        return (
            <div className="min-h-screen bg-[#0a0b0f] flex items-center justify-center">
                <div className="w-8 h-8 rounded-full border-2 border-purple-500 border-t-transparent animate-spin" />
            </div>
        );
    }

    // Success screen
    if (status === 'success' && liqpayData) {
        return (
            <div className="min-h-screen bg-[#0a0b0f] flex items-center justify-center px-4">
                <motion.div
                    initial={{ opacity: 0, scale: 0.9 }}
                    animate={{ opacity: 1, scale: 1 }}
                    className="w-full max-w-md text-center"
                >
                    <div className="w-20 h-20 rounded-full bg-green-900/30 border border-green-700/40 flex items-center justify-center mx-auto mb-6">
                        <CheckCircle2 className="h-10 w-10 text-green-400" />
                    </div>
                    <h1 className="text-3xl font-black text-white mb-2">Payment initiated!</h1>
                    <p className="text-gray-500 mb-8">Your LiqPay payment form is ready</p>

                    {/* LiqPay form */}
                    <div className="bg-[#111217] rounded-2xl p-6 border border-white/5 mb-6 text-left space-y-3">
                        <div>
                            <p className="text-xs text-gray-600 uppercase tracking-widest mb-1">Data</p>
                            <p className="text-gray-400 text-xs font-mono break-all">{liqpayData.data}</p>
                        </div>
                        <div className="h-px bg-white/5" />
                        <div>
                            <p className="text-xs text-gray-600 uppercase tracking-widest mb-1">Signature</p>
                            <p className="text-gray-400 text-xs font-mono break-all">{liqpayData.signature}</p>
                        </div>
                    </div>

                    {/* LiqPay embedded form */}
                    <form method="POST" action="https://www.liqpay.ua/api/3/checkout" acceptCharset="utf-8" target="_blank">
                        <input type="hidden" name="data" value={liqpayData.data} />
                        <input type="hidden" name="signature" value={liqpayData.signature} />
                        <motion.button
                            type="submit"
                            whileHover={{ scale: 1.02 }}
                            whileTap={{ scale: 0.98 }}
                            className="w-full h-12 rounded-xl bg-green-600 hover:bg-green-500 text-black font-bold text-sm cursor-pointer transition-colors mb-3"
                        >
                            Open LiqPay Checkout
                        </motion.button>
                    </form>

                    <button
                        onClick={() => navigate('/orders')}
                        className="text-sm text-gray-600 hover:text-white transition-colors cursor-pointer"
                    >
                        Back to orders
                    </button>
                </motion.div>
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-[#0a0b0f] text-gray-200">
            <div className="max-w-lg mx-auto px-4 py-10">

                {/* Back */}
                <motion.button
                    initial={{ opacity: 0, x: -12 }}
                    animate={{ opacity: 1, x: 0 }}
                    onClick={() => navigate(-1)}
                    className="flex items-center gap-2 text-gray-600 hover:text-white transition-colors group mb-10 cursor-pointer text-sm"
                >
                    <ArrowLeft className="h-4 w-4 group-hover:-translate-x-1 transition-transform" />
                    Back
                </motion.button>

                <motion.div
                    initial={{ opacity: 0, y: 24 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ duration: 0.4 }}
                >
                    {/* Header */}
                    <div className="mb-8">
                        <p className="text-xs uppercase tracking-widest text-gray-600 mb-1">Checkout</p>
                        <h1 className="text-4xl font-black text-white">Payment</h1>
                    </div>

                    {/* Order summary */}
                    {order && (
                        <div className="bg-[#111217] rounded-2xl p-5 border border-white/5 mb-6">
                            <p className="text-xs uppercase tracking-widest text-gray-600 mb-3">Order</p>
                            <div className="flex justify-between items-center">
                                <div>
                                    <p className="text-white font-semibold">{order.title}</p>
                                    <p className="text-gray-600 text-sm">{order.totalItems} item{order.totalItems !== 1 ? 's' : ''}</p>
                                </div>
                                <span className="text-2xl font-black bg-gradient-to-r from-green-400 to-emerald-300 bg-clip-text text-transparent">
                                    ${order.totalPrice.toLocaleString()}
                                </span>
                            </div>
                        </div>
                    )}

                    {/* Payment method */}
                    <div className="mb-6">
                        <p className="text-xs uppercase tracking-widest text-gray-600 mb-3">Payment method</p>
                        <div className="grid grid-cols-2 gap-3">
                            {([
                                { id: 'card', label: 'Card', icon: <CreditCard className="h-5 w-5" /> },
                                { id: 'crypto', label: 'Crypto', icon: <Bitcoin className="h-5 w-5" /> },
                            ] as const).map(({ id, label, icon }) => (
                                <button
                                    key={id}
                                    onClick={() => setMethod(id)}
                                    className={`flex items-center justify-center gap-2 h-12 rounded-xl border text-sm font-medium transition-all cursor-pointer ${
                                        method === id
                                            ? 'bg-purple-900/40 border-purple-600 text-purple-300'
                                            : 'bg-[#111217] border-white/5 text-gray-500 hover:border-white/10 hover:text-white'
                                    }`}
                                >
                                    {icon}
                                    {label}
                                </button>
                            ))}
                        </div>
                    </div>

                    {/* Card form */}
                    <AnimatePresence mode="wait">
                        {method === 'card' && (
                            <motion.div
                                key="card"
                                initial={{ opacity: 0, y: 10 }}
                                animate={{ opacity: 1, y: 0 }}
                                exit={{ opacity: 0, y: -10 }}
                                className="space-y-3 mb-6"
                            >
                                <div>
                                    <label className="text-xs uppercase tracking-widest text-gray-600 mb-1.5 block">Card number</label>
                                    <input
                                        type="text"
                                        value={cardNumber}
                                        onChange={e => setCardNumber(formatCardNumber(e.target.value))}
                                        placeholder="0000 0000 0000 0000"
                                        className="w-full h-11 px-4 bg-[#111217] border border-white/5 rounded-xl text-white placeholder-gray-700 focus:border-purple-600 focus:outline-none transition-colors font-mono"
                                    />
                                </div>
                                <div className="grid grid-cols-2 gap-3">
                                    <div>
                                        <label className="text-xs uppercase tracking-widest text-gray-600 mb-1.5 block">Expiry</label>
                                        <input
                                            type="text"
                                            value={expiry}
                                            onChange={e => setExpiry(formatExpiry(e.target.value))}
                                            placeholder="MM/YY"
                                            className="w-full h-11 px-4 bg-[#111217] border border-white/5 rounded-xl text-white placeholder-gray-700 focus:border-purple-600 focus:outline-none transition-colors font-mono"
                                        />
                                    </div>
                                    <div>
                                        <label className="text-xs uppercase tracking-widest text-gray-600 mb-1.5 block">CVV</label>
                                        <input
                                            type="password"
                                            value={cvv}
                                            onChange={e => setCvv(e.target.value.replace(/\D/g, '').slice(0, 3))}
                                            placeholder="•••"
                                            className="w-full h-11 px-4 bg-[#111217] border border-white/5 rounded-xl text-white placeholder-gray-700 focus:border-purple-600 focus:outline-none transition-colors font-mono"
                                        />
                                    </div>
                                </div>
                            </motion.div>
                        )}

                        {method === 'crypto' && (
                            <motion.div
                                key="crypto"
                                initial={{ opacity: 0, y: 10 }}
                                animate={{ opacity: 1, y: 0 }}
                                exit={{ opacity: 0, y: -10 }}
                                className="bg-[#111217] rounded-2xl p-5 border border-white/5 mb-6"
                            >
                                <div className="flex items-center gap-3">
                                    <Bitcoin className="h-8 w-8 text-yellow-500 shrink-0" />
                                    <div>
                                        <p className="text-white font-medium">Cryptocurrency payment</p>
                                        <p className="text-gray-600 text-sm">You will be redirected to LiqPay crypto checkout</p>
                                    </div>
                                </div>
                            </motion.div>
                        )}
                    </AnimatePresence>

                    {/* Error */}
                    <AnimatePresence>
                        {status === 'error' && error && (
                            <motion.div
                                initial={{ opacity: 0, y: -8 }}
                                animate={{ opacity: 1, y: 0 }}
                                exit={{ opacity: 0 }}
                                className="flex items-center gap-3 bg-red-950/40 border border-red-800/40 rounded-xl p-4 mb-5"
                            >
                                <XCircle className="h-5 w-5 text-red-400 shrink-0" />
                                <p className="text-red-300 text-sm">{error}</p>
                            </motion.div>
                        )}
                    </AnimatePresence>

                    {/* Pay button */}
                    <motion.button
                        whileHover={{ scale: 1.02 }}
                        whileTap={{ scale: 0.98 }}
                        onClick={handlePay}
                        disabled={status === 'loading'}
                        className="w-full h-14 rounded-xl bg-white text-black font-bold text-base flex items-center justify-center gap-3 cursor-pointer hover:bg-gray-100 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                    >
                        {status === 'loading' ? (
                            <><Loader2 className="h-5 w-5 animate-spin" /> Processing...</>
                        ) : (
                            <><Lock className="h-4 w-4" /> Pay ${order?.totalPrice.toLocaleString()}</>
                        )}
                    </motion.button>

                    <p className="text-center text-xs text-gray-700 mt-4">
                        Secured by LiqPay · SSL encrypted
                    </p>
                </motion.div>
            </div>
        </div>
    );
};