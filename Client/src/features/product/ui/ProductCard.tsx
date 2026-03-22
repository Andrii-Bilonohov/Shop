import { motion } from 'framer-motion';
import { Link } from 'react-router';
import { Card, CardContent } from '@/app/components/ui/card';
import { Button } from '@/app/components/ui/button';
import { Star, ShoppingCart } from 'lucide-react';
import type { Product } from '@/entities/product/model/types';

type Props = {
    product: Product;
    onAdd: (product: Product) => void;
};

export const ProductCard = ({ product, onAdd }: Props) => {
    return (
        <motion.div whileHover={{ scale: 1.05, y: -5 }}>
            <Card className="bg-[#1b1c22] border-0 shadow-xl hover:shadow-purple-900/40 transition">

                <Link to={`/product/${product.id}`}>
                    <img
                        src={product.imageUrl}
                        alt={product.name}
                        className="h-48 w-full object-cover"
                        onError={(e) => {
                            e.currentTarget.style.display = 'none';
                            e.currentTarget.nextElementSibling?.removeAttribute('style');
                        }}
                    />
                    <div style={{ display: 'none' }} className="h-48 bg-[#13141a] flex items-center justify-center relative overflow-hidden">
                        <svg viewBox="0 0 300 192" xmlns="http://www.w3.org/2000/svg" className="w-full h-full">
                            <line x1="0" y1="0" x2="300" y2="192" stroke="#1e1f28" strokeWidth="1.5"/>
                            <line x1="300" y1="0" x2="0" y2="192" stroke="#1e1f28" strokeWidth="1.5"/>
                            <line x1="0" y1="64" x2="300" y2="64" stroke="#1a1b23" strokeWidth="1"/>
                            <line x1="0" y1="128" x2="300" y2="128" stroke="#1a1b23" strokeWidth="1"/>
                            <line x1="100" y1="0" x2="100" y2="192" stroke="#1a1b23" strokeWidth="1"/>
                            <line x1="200" y1="0" x2="200" y2="192" stroke="#1a1b23" strokeWidth="1"/>
                            <rect x="118" y="72" width="64" height="48" rx="6" fill="none" stroke="#2e2f3e" strokeWidth="2"/>
                            <polyline points="122,114 138,94 150,104 162,88 178,114" fill="none" stroke="#2e2f3e" strokeWidth="2" strokeLinejoin="round"/>
                            <circle cx="132" cy="84" r="5" fill="none" stroke="#2e2f3e" strokeWidth="2"/>
                            <line x1="112" y1="126" x2="188" y2="66" stroke="#ef4444" strokeWidth="2.5" strokeLinecap="round" opacity="0.85"/>
                            <text x="150" y="140" textAnchor="middle" fontFamily="monospace" fontSize="10" fill="#3a3b4e" letterSpacing="3">NO IMAGE</text>
                        </svg>
                    </div>
                </Link>

                <CardContent className="p-4">
                    <h3 className="font-semibold mb-1 text-white">{product.name}</h3>

                    <div className="flex items-center gap-1 mb-2">
                        {Array.from({ length: 5 }).map((_, i) => (
                            <Star
                                key={i}
                                className={`h-4 w-4 ${
                                    i < Math.floor(product.rating || 0)
                                        ? 'fill-yellow-400 text-yellow-400'
                                        : 'text-gray-500'
                                }`}
                            />
                        ))}
                    </div>

                    <div className="text-green-400 font-bold text-lg">
                        ${product.price}
                    </div>

                    <Button
                        className="w-full mt-3 bg-green-600 hover:bg-green-700 text-black cursor-pointer"
                        onClick={() => onAdd(product)}
                    >
                        <ShoppingCart className="mr-2 h-4 w-4" />
                        Add to cart
                    </Button>
                </CardContent>

            </Card>
        </motion.div>
    );
};