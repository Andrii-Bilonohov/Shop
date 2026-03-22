import { useState, useMemo } from 'react';
import { Search, SlidersHorizontal, X } from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';

import { Input } from '@/app/components/ui/input';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/app/components/ui/select';

import { useProducts } from '@/features/product/api/useProducts';
import { useCartStore } from '@/features/cart/model/store';
import { toast } from 'sonner';

import type { SortOption, Product } from '@/entities/product/model/types';
import { ProductCard } from "@/features/product/ui/ProductCard.tsx";
import { ProductCardSkeleton } from "@/features/product/ui/ProductCardSkeleton.tsx";

const categories = ['Electronics', 'Clothing', 'Home Appliance', 'Books', 'Toys', 'Sports', 'Beauty', 'Automotive', 'Grocery', 'Health', 'Furniture'];

export const CatalogPage = () => {
    const { data: products = [], isLoading } = useProducts();
    const addItem = useCartStore(s => s.addItem);

    const [search, setSearch] = useState('');
    const [selectedCategories, setSelectedCategories] = useState<string[]>([]);
    const [sort, setSort] = useState<SortOption>('rating');
    const [sidebarOpen, setSidebarOpen] = useState(false);

    const filtered = useMemo(() => {
        let result = [...products];
        if (search) result = result.filter(p => p.name.toLowerCase().includes(search.toLowerCase()));
        if (selectedCategories.length) result = result.filter(p => selectedCategories.includes(p.category || ''));
        if (sort === 'price-asc') result.sort((a, b) => a.price - b.price);
        if (sort === 'price-desc') result.sort((a, b) => b.price - a.price);
        if (sort === 'rating') result.sort((a, b) => (b.rating || 0) - (a.rating || 0));
        return result;
    }, [products, search, selectedCategories, sort]);

    const toggleCategory = (cat: string) => {
        setSelectedCategories(prev =>
            prev.includes(cat) ? prev.filter(c => c !== cat) : [...prev, cat]
        );
    };

    const handleAdd = (product: Product) => {
        addItem(product);
        toast.success(`${product.name} added`);
    };

    const SidebarContent = () => (
        <div className="space-y-1">
            <div className="flex items-center justify-between mb-4">
                <h3 className="text-xs uppercase tracking-widest text-gray-500 font-medium">Categories</h3>
                {selectedCategories.length > 0 && (
                    <button
                        onClick={() => setSelectedCategories([])}
                        className="text-xs text-gray-600 hover:text-red-400 transition-colors cursor-pointer"
                    >
                        Clear
                    </button>
                )}
            </div>
            {categories.map(cat => {
                const active = selectedCategories.includes(cat);
                return (
                    <button
                        key={cat}
                        onClick={() => toggleCategory(cat)}
                        className={`w-full flex items-center justify-between px-3 py-2 rounded-lg text-sm transition-all cursor-pointer ${
                            active
                                ? 'bg-purple-900/40 text-purple-300 border border-purple-700/40'
                                : 'text-gray-500 hover:text-white hover:bg-white/5'
                        }`}
                    >
                        <span>{cat}</span>
                        {active && <span className="w-1.5 h-1.5 rounded-full bg-purple-400" />}
                    </button>
                );
            })}
        </div>
    );

    return (
        <div className="min-h-screen bg-[#0f1115] text-gray-200">
            <div className="pointer-events-none fixed top-0 left-1/2 -translate-x-1/2 w-[800px] h-[200px] bg-purple-700/8 blur-[100px] rounded-full" />

            <div className="max-w-7xl mx-auto px-4 py-10 relative">

                <div className="flex items-end justify-between mb-8">
                    <div>
                        <p className="text-xs uppercase tracking-widest text-gray-600 mb-1">Browse</p>
                        <h1 className="text-4xl font-black text-white">
                            Catalog
                            {!isLoading && (
                                <span className="ml-3 text-lg font-normal text-gray-600">
                                    {filtered.length} items
                                </span>
                            )}
                        </h1>
                    </div>

                    <button
                        onClick={() => setSidebarOpen(true)}
                        className="md:hidden flex items-center gap-2 px-3 py-2 rounded-lg bg-[#1b1c22] text-gray-400 hover:text-white transition-colors cursor-pointer text-sm"
                    >
                        <SlidersHorizontal className="h-4 w-4" />
                        Filters
                        {selectedCategories.length > 0 && (
                            <span className="w-5 h-5 rounded-full bg-purple-600 text-white text-xs flex items-center justify-center">
                                {selectedCategories.length}
                            </span>
                        )}
                    </button>
                </div>

                <div className="flex flex-col sm:flex-row gap-3 mb-8">
                    <div className="flex-1 relative">
                        <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-600" />
                        <Input
                            value={search}
                            onChange={(e) => setSearch(e.target.value)}
                            placeholder="Search products..."
                            className="pl-10 bg-[#1b1c22] border-0 text-white placeholder-gray-700 rounded-xl h-11 focus:ring-1 focus:ring-purple-600/50"
                        />
                        {search && (
                            <button
                                onClick={() => setSearch('')}
                                className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-600 hover:text-white transition-colors cursor-pointer"
                            >
                                <X className="h-4 w-4" />
                            </button>
                        )}
                    </div>

                    <Select value={sort} onValueChange={(v) => setSort(v as SortOption)}>
                        <SelectTrigger className="bg-[#1b1c22] border-0 text-gray-400 w-full sm:w-44 rounded-xl h-11">
                            <SelectValue />
                        </SelectTrigger>
                        <SelectContent className="bg-[#1b1c22] border-gray-800 text-white">
                            <SelectItem value="rating" className="text-gray-300 focus:bg-white/10 focus:text-white cursor-pointer">Top rated</SelectItem>
                            <SelectItem value="price-asc" className="text-gray-300 focus:bg-white/10 focus:text-white cursor-pointer">Price ↑</SelectItem>
                            <SelectItem value="price-desc" className="text-gray-300 focus:bg-white/10 focus:text-white cursor-pointer">Price ↓</SelectItem>
                        </SelectContent>
                    </Select>
                </div>
                <AnimatePresence>
                    {selectedCategories.length > 0 && (
                        <motion.div
                            initial={{ opacity: 0, height: 0 }}
                            animate={{ opacity: 1, height: 'auto' }}
                            exit={{ opacity: 0, height: 0 }}
                            className="flex flex-wrap gap-2 mb-6"
                        >
                            {selectedCategories.map(cat => (
                                <motion.button
                                    key={cat}
                                    initial={{ opacity: 0, scale: 0.8 }}
                                    animate={{ opacity: 1, scale: 1 }}
                                    exit={{ opacity: 0, scale: 0.8 }}
                                    onClick={() => toggleCategory(cat)}
                                    className="flex items-center gap-1.5 px-3 py-1 rounded-full bg-purple-900/40 border border-purple-700/40 text-purple-300 text-xs hover:bg-red-900/30 hover:border-red-700/40 hover:text-red-400 transition-all cursor-pointer"
                                >
                                    {cat}
                                    <X className="h-3 w-3" />
                                </motion.button>
                            ))}
                        </motion.div>
                    )}
                </AnimatePresence>

                <div className="flex gap-6">
                    <aside className="hidden md:block w-52 shrink-0">
                        <div className="sticky top-24 bg-[#1b1c22] rounded-2xl p-4">
                            <SidebarContent />
                        </div>
                    </aside>

                    <div className="flex-1">
                        {isLoading ? (
                            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-5">
                                {Array.from({ length: 8 }).map((_, i) => (
                                    <ProductCardSkeleton key={i} />
                                ))}
                            </div>
                        ) : filtered.length === 0 ? (
                            <motion.div
                                initial={{ opacity: 0 }}
                                animate={{ opacity: 1 }}
                                className="flex flex-col items-center justify-center py-24 text-center"
                            >
                                <Search className="h-12 w-12 text-gray-800 mb-4" />
                                <p className="text-gray-500 font-medium">No products found</p>
                                <p className="text-gray-700 text-sm mt-1">Try adjusting your search or filters</p>
                            </motion.div>
                        ) : (
                            <motion.div
                                layout
                                className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-5"
                            >
                                <AnimatePresence>
                                    {filtered.map(product => (
                                        <motion.div
                                            key={product.id}
                                            layout
                                            initial={{ opacity: 0, scale: 0.95 }}
                                            animate={{ opacity: 1, scale: 1 }}
                                            exit={{ opacity: 0, scale: 0.95 }}
                                            transition={{ duration: 0.2 }}
                                        >
                                            <ProductCard product={product} onAdd={handleAdd} />
                                        </motion.div>
                                    ))}
                                </AnimatePresence>
                            </motion.div>
                        )}
                    </div>
                </div>
            </div>

            <AnimatePresence>
                {sidebarOpen && (
                    <>
                        <motion.div
                            initial={{ opacity: 0 }}
                            animate={{ opacity: 1 }}
                            exit={{ opacity: 0 }}
                            onClick={() => setSidebarOpen(false)}
                            className="fixed inset-0 bg-black/60 z-40 md:hidden backdrop-blur-sm"
                        />
                        <motion.div
                            initial={{ x: '-100%' }}
                            animate={{ x: 0 }}
                            exit={{ x: '-100%' }}
                            transition={{ type: 'spring', damping: 30, stiffness: 300 }}
                            className="fixed left-0 top-0 bottom-0 w-72 bg-[#1b1c22] z-50 p-6 md:hidden overflow-y-auto"
                        >
                            <div className="flex items-center justify-between mb-6">
                                <h2 className="text-white font-semibold">Filters</h2>
                                <button onClick={() => setSidebarOpen(false)} className="text-gray-500 hover:text-white cursor-pointer">
                                    <X className="h-5 w-5" />
                                </button>
                            </div>
                            <SidebarContent />
                        </motion.div>
                    </>
                )}
            </AnimatePresence>
        </div>
    );
};