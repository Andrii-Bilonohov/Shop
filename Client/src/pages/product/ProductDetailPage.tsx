import { useState } from 'react';
import { useParams, useNavigate } from 'react-router';
import { motion } from 'framer-motion';
import { ArrowLeft, ShoppingCart, AlertCircle, Star, Package, Weight, Tag } from 'lucide-react';
import { Button } from '@/app/components/ui/button';
import { Badge } from '@/app/components/ui/badge';
import { Alert, AlertDescription, AlertTitle } from '@/app/components/ui/alert';
import { useProduct } from '@/features/product/api/useProducts';
import { useCartStore } from '@/features/cart/model/store';
import { toast } from 'sonner';

const fadeUp = {
    hidden: { opacity: 0, y: 16 },
    visible: (i: number) => ({
        opacity: 1, y: 0,
        transition: { delay: i * 0.07, duration: 0.4 },
    }),
};

export const ProductDetailPage = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { data: product, isLoading, error } = useProduct(id!);
    const addItem = useCartStore((state) => state.addItem);
    const [imgError, setImgError] = useState(false);

    const handleAddToCart = () => {
        if (product) {
            addItem(product);
            toast.success(`${product.name} added to cart`);
        }
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

    if (error || !product) {
        return (
            <div className="min-h-screen bg-[#0f1115] flex items-center justify-center px-4">
                <Alert variant="destructive" className="max-w-md bg-red-950/40 border-red-800">
                    <AlertCircle className="h-4 w-4" />
                    <AlertTitle>Error</AlertTitle>
                    <AlertDescription>Failed to load product. Please try again later.</AlertDescription>
                </Alert>
            </div>
        );
    }

    const rating = product.rating || 0;

    return (
        <div className="min-h-screen bg-[#0f1115] text-gray-200">

            <div className="pointer-events-none fixed top-0 left-1/2 -translate-x-1/2 w-[600px] h-[300px] bg-purple-700/10 blur-[120px] rounded-full" />

            <div className="max-w-6xl mx-auto px-4 py-10 relative">

                <motion.button
                    initial={{ opacity: 0, x: -12 }}
                    animate={{ opacity: 1, x: 0 }}
                    onClick={() => navigate('/catalog')}
                    className="flex items-center gap-2 text-gray-400 hover:text-white transition mb-10 group"
                >
                    <ArrowLeft className="h-4 w-4 group-hover:-translate-x-1 transition-transform" />
                    <span className="text-sm tracking-wide">Back to Catalog</span>
                </motion.button>

                <div className="grid md:grid-cols-2 gap-12 items-start">

                    <motion.div
                        initial={{ opacity: 0, scale: 0.97 }}
                        animate={{ opacity: 1, scale: 1 }}
                        transition={{ duration: 0.45, ease: 'easeOut' }}
                        className="sticky top-10"
                    >
                        <div className="relative rounded-2xl overflow-hidden bg-[#1b1c22] shadow-2xl shadow-black/60 aspect-square">
                            {product.imageUrl && !imgError ? (
                                <img
                                    src={product.imageUrl}
                                    alt={product.name}
                                    className="w-full h-full object-cover"
                                    onError={() => setImgError(true)}
                                />
                            ) : (
                                <div className="w-full h-full bg-[#13141a] flex items-center justify-center relative overflow-hidden">
                                    <svg viewBox="0 0 400 400" xmlns="http://www.w3.org/2000/svg" className="w-full h-full">
                                        <line x1="0" y1="0" x2="400" y2="400" stroke="#1e1f28" strokeWidth="1.5"/>
                                        <line x1="400" y1="0" x2="0" y2="400" stroke="#1e1f28" strokeWidth="1.5"/>
                                        <line x1="0" y1="133" x2="400" y2="133" stroke="#1a1b23" strokeWidth="1"/>
                                        <line x1="0" y1="266" x2="400" y2="266" stroke="#1a1b23" strokeWidth="1"/>
                                        <line x1="133" y1="0" x2="133" y2="400" stroke="#1a1b23" strokeWidth="1"/>
                                        <line x1="266" y1="0" x2="266" y2="400" stroke="#1a1b23" strokeWidth="1"/>
                                        <rect x="155" y="155" width="90" height="90" rx="10" fill="none" stroke="#2e2f3e" strokeWidth="2"/>
                                        <polyline points="160,238 180,210 198,222 220,196 242,238" fill="none" stroke="#2e2f3e" strokeWidth="2" strokeLinejoin="round"/>
                                        <circle cx="174" cy="175" r="8" fill="none" stroke="#2e2f3e" strokeWidth="2"/>
                                        <line x1="148" y1="252" x2="252" y2="148" stroke="#ef4444" strokeWidth="2.5" strokeLinecap="round" opacity="0.8"/>
                                        <text x="200" y="270" textAnchor="middle" fontFamily="monospace" fontSize="11" fill="#3a3b4e" letterSpacing="4">NO IMAGE</text>
                                    </svg>
                                </div>
                            )}

                            {!product.stock && (
                                <div className="absolute inset-0 bg-black/60 flex items-center justify-center">
                                    <span className="text-white text-lg font-semibold tracking-widest uppercase border border-white/20 px-4 py-2 rounded-lg backdrop-blur-sm">
                                        Out of Stock
                                    </span>
                                </div>
                            )}
                        </div>
                    </motion.div>

                    <div className="space-y-6">

                        <motion.div custom={0} variants={fadeUp} initial="hidden" animate="visible">
                            {product.category && (
                                <Badge className="mb-3 bg-purple-900/50 text-purple-300 border-purple-700/50 hover:bg-purple-900/50">
                                    <Tag className="h-3 w-3 mr-1" />
                                    {product.category}
                                </Badge>
                            )}
                            <h1 className="text-4xl font-bold text-white leading-tight">
                                {product.name}
                            </h1>
                        </motion.div>

                        <motion.div custom={1} variants={fadeUp} initial="hidden" animate="visible"
                                    className="flex items-center gap-3"
                        >
                            <div className="flex gap-1">
                                {Array.from({ length: 5 }).map((_, i) => (
                                    <Star key={i} className={`h-5 w-5 ${
                                        i < Math.floor(rating)
                                            ? 'fill-yellow-400 text-yellow-400'
                                            : 'text-gray-600'
                                    }`} />
                                ))}
                            </div>
                            <span className="text-gray-400 text-sm">{rating > 0 ? `${rating} / 5` : 'No reviews yet'}</span>
                        </motion.div>

                        <motion.div custom={2} variants={fadeUp} initial="hidden" animate="visible">
                            <div className="text-5xl font-bold bg-gradient-to-r from-green-400 to-emerald-300 bg-clip-text text-transparent">
                                ${product.price.toLocaleString()}
                            </div>
                        </motion.div>

                        <motion.div custom={3} variants={fadeUp} initial="hidden" animate="visible">
                            <div className="h-px bg-gradient-to-r from-transparent via-gray-700 to-transparent" />
                        </motion.div>

                        <motion.div custom={4} variants={fadeUp} initial="hidden" animate="visible">
                            <h2 className="text-xs uppercase tracking-widest text-gray-500 mb-2">Description</h2>
                            <p className="text-gray-300 leading-relaxed">{product.description}</p>
                        </motion.div>

                        <motion.div custom={5} variants={fadeUp} initial="hidden" animate="visible"
                                    className="grid grid-cols-2 gap-3"
                        >
                            <div className="bg-[#1b1c22] rounded-xl p-4 flex items-center gap-3">
                                <Package className="h-5 w-5 text-purple-400 shrink-0" />
                                <div>
                                    <div className="text-xs text-gray-500 uppercase tracking-wider">Stock</div>
                                    <div className="text-white font-semibold">{product.stock} units</div>
                                </div>
                            </div>
                            <div className="bg-[#1b1c22] rounded-xl p-4 flex items-center gap-3">
                                <Weight className="h-5 w-5 text-indigo-400 shrink-0" />
                                <div>
                                    <div className="text-xs text-gray-500 uppercase tracking-wider">Weight</div>
                                    <div className="text-white font-semibold">{product.weight} kg</div>
                                </div>
                            </div>
                        </motion.div>

                        <motion.div custom={6} variants={fadeUp} initial="hidden" animate="visible">
                            <Button
                                size="lg"
                                disabled={!product.stock}
                                onClick={handleAddToCart}
                                className="w-full bg-green-600 hover:bg-green-500 text-black font-semibold text-base h-14 rounded-xl cursor-pointer transition-all disabled:opacity-40"
                            >
                                <ShoppingCart className="mr-2 h-5 w-5" />
                                {product.stock ? 'Add to Cart' : 'Out of Stock'}
                            </Button>
                        </motion.div>

                    </div>
                </div>
            </div>
        </div>
    );
};