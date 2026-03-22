import { Link } from "react-router";
import { motion, useScroll, useTransform } from "framer-motion";
import { useRef } from "react";
import { ArrowRight, ShoppingBag, Zap, Shield, Star } from "lucide-react";

const PRODUCTS = [
    { title: "iPhone 15 Pro", price: 999, category: "Electronics", image: "https://images.unsplash.com/photo-1695048133142-1a20484d2569?w=400&q=80" },
    { title: "Gaming Laptop", price: 1499, category: "Electronics", image: "https://images.unsplash.com/photo-1603302576837-37561b2e2302?w=400&q=80" },
    { title: "Headphones", price: 199, category: "Audio", image: "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=400&q=80" },
    { title: "Smart Watch", price: 299, category: "Wearables", image: "https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=400&q=80" },
    { title: "Sneakers", price: 149, category: "Clothing", image: "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400&q=80" },
    { title: "Camera", price: 799, category: "Electronics", image: "https://images.unsplash.com/photo-1516035069371-29a1b244cc32?w=400&q=80" },
];

const FEATURES = [
    { icon: <Zap className="h-5 w-5" />, title: "Fast delivery", desc: "Same-day shipping on thousands of items" },
    { icon: <Shield className="h-5 w-5" />, title: "Secure payments", desc: "End-to-end encrypted transactions" },
    { icon: <Star className="h-5 w-5" />, title: "Top rated", desc: "Only verified reviews from real buyers" },
    { icon: <ShoppingBag className="h-5 w-5" />, title: "Easy returns", desc: "30-day hassle-free return policy" },
];

const stagger = {
    visible: { transition: { staggerChildren: 0.1 } },
};
const fadeUp = {
    hidden: { opacity: 0, y: 32 },
    visible: { opacity: 1, y: 0, transition: { duration: 0.6, ease: [0.22, 1, 0.36, 1] } },
};

export const GuestHomePage = () => {
    const heroRef = useRef(null);
    const { scrollYProgress } = useScroll({ target: heroRef, offset: ["start start", "end start"] });
    const heroY = useTransform(scrollYProgress, [0, 1], ["0%", "30%"]);
    const heroOpacity = useTransform(scrollYProgress, [0, 0.8], [1, 0]);

    return (
        <div className="min-h-screen bg-[#0a0b0f] text-gray-200 overflow-x-hidden">

            <nav className="fixed top-0 left-0 right-0 z-50 flex items-center justify-between px-8 py-4 bg-[#0a0b0f]/70 backdrop-blur-xl border-b border-white/5">
                <div className="flex items-center gap-2">
                    <div className="w-8 h-8 rounded-lg bg-gradient-to-tr from-purple-600 to-indigo-500 flex items-center justify-center">
                        <Zap className="h-4 w-4 text-white" />
                    </div>
                    <span className="font-bold text-white text-lg tracking-tight">E-Commerce</span>
                </div>

                <div className="hidden md:flex items-center gap-8 text-sm text-gray-500">
                    <a href="#products" className="hover:text-white transition-colors">Products</a>
                    <a href="#features" className="hover:text-white transition-colors">Features</a>
                    <a href="#about" className="hover:text-white transition-colors">About</a>
                </div>

                <div className="flex items-center gap-3">
                    <Link to="/login">
                        <button className="px-4 py-2 text-sm text-gray-400 hover:text-white transition-colors cursor-pointer">
                            Sign in
                        </button>
                    </Link>
                    <Link to="/register">
                        <button className="px-4 py-2 text-sm bg-white text-black font-semibold rounded-lg hover:bg-gray-100 transition-colors cursor-pointer">
                            Get started
                        </button>
                    </Link>
                </div>
            </nav>

            <section ref={heroRef} className="relative min-h-screen flex items-center justify-center overflow-hidden pt-20">
                <div className="absolute inset-0 opacity-[0.03]"
                     style={{ backgroundImage: 'linear-gradient(white 1px, transparent 1px), linear-gradient(90deg, white 1px, transparent 1px)', backgroundSize: '60px 60px' }}
                />

                <motion.div style={{ y: heroY }} className="absolute inset-0 pointer-events-none">
                    <div className="absolute top-1/4 left-1/4 w-96 h-96 bg-purple-600/15 rounded-full blur-[120px]" />
                    <div className="absolute bottom-1/4 right-1/4 w-80 h-80 bg-indigo-600/10 rounded-full blur-[100px]" />
                    <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[600px] h-[300px] bg-green-600/5 rounded-full blur-[80px]" />
                </motion.div>

                <motion.div
                    style={{ y: heroY, opacity: heroOpacity }}
                    className="relative text-center max-w-5xl mx-auto px-6"
                >
                    <motion.div
                        initial={{ opacity: 0, scale: 0.9 }}
                        animate={{ opacity: 1, scale: 1 }}
                        transition={{ duration: 0.5 }}
                        className="inline-flex items-center gap-2 px-3 py-1.5 rounded-full border border-white/10 bg-white/5 text-xs text-gray-400 mb-8"
                    >
                        <span className="w-1.5 h-1.5 rounded-full bg-green-400 animate-pulse" />
                        Now with 50,000+ products
                    </motion.div>

                    <motion.h1
                        initial={{ opacity: 0, y: 40 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.7, delay: 0.1, ease: [0.22, 1, 0.36, 1] }}
                        className="text-6xl md:text-8xl font-black leading-[0.9] tracking-tight text-white mb-6"
                    >
                        Shop the
                        <br />
                        <span className="bg-gradient-to-r from-green-400 via-emerald-300 to-teal-400 bg-clip-text text-transparent">
                            future
                        </span>
                        <br />
                        today.
                    </motion.h1>

                    <motion.p
                        initial={{ opacity: 0, y: 20 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.6, delay: 0.3 }}
                        className="text-gray-500 text-lg max-w-xl mx-auto mb-10"
                    >
                        Thousands of products. Unbeatable prices. Delivered to your door faster than ever before.
                    </motion.p>

                    <motion.div
                        initial={{ opacity: 0, y: 20 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.6, delay: 0.4 }}
                        className="flex items-center justify-center gap-4"
                    >
                        <Link to="/register">
                            <motion.button
                                whileHover={{ scale: 1.03 }}
                                whileTap={{ scale: 0.97 }}
                                className="flex items-center gap-2 px-7 py-3.5 bg-white text-black font-bold rounded-xl text-sm hover:bg-gray-100 transition-colors cursor-pointer shadow-2xl shadow-white/10"
                            >
                                Start shopping
                                <ArrowRight className="h-4 w-4" />
                            </motion.button>
                        </Link>
                        <Link to="/login">
                            <button className="px-7 py-3.5 border border-white/10 text-gray-400 hover:text-white hover:border-white/20 rounded-xl text-sm transition-all cursor-pointer">
                                Sign in
                            </button>
                        </Link>
                    </motion.div>
                </motion.div>

                <motion.div
                    initial={{ opacity: 0 }}
                    animate={{ opacity: 1 }}
                    transition={{ delay: 1.2 }}
                    className="absolute bottom-10 left-1/2 -translate-x-1/2 flex flex-col items-center gap-2"
                >
                    <span className="text-xs text-gray-700 tracking-widest uppercase">Scroll</span>
                    <motion.div
                        animate={{ y: [0, 8, 0] }}
                        transition={{ repeat: Infinity, duration: 1.5 }}
                        className="w-px h-10 bg-gradient-to-b from-gray-700 to-transparent"
                    />
                </motion.div>
            </section>

            {/* ─── PRODUCTS ─── */}
            <section id="products" className="py-24 px-6">
                <div className="max-w-7xl mx-auto">
                    <motion.div
                        initial="hidden"
                        whileInView="visible"
                        viewport={{ once: true, margin: "-100px" }}
                        variants={stagger}
                        className="mb-14"
                    >
                        <motion.p variants={fadeUp} className="text-xs uppercase tracking-widest text-gray-600 mb-3">Featured</motion.p>
                        <motion.h2 variants={fadeUp} className="text-4xl md:text-5xl font-black text-white">Popular right now</motion.h2>
                    </motion.div>

                    <motion.div
                        initial="hidden"
                        whileInView="visible"
                        viewport={{ once: true, margin: "-50px" }}
                        variants={stagger}
                        className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-5"
                    >
                        {PRODUCTS.map((p, i) => (
                            <motion.div
                                key={i}
                                variants={fadeUp}
                                whileHover={{ y: -6 }}
                                transition={{ type: "spring", stiffness: 300, damping: 20 }}
                                className="group bg-[#111217] rounded-2xl overflow-hidden border border-white/5 hover:border-white/10 transition-colors"
                            >
                                <div className="relative h-52 overflow-hidden">
                                    <img src={p.image} alt={p.title} className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500" />
                                    <div className="absolute inset-0 bg-gradient-to-t from-[#111217] to-transparent opacity-60" />
                                    <span className="absolute top-3 left-3 text-xs px-2.5 py-1 rounded-full bg-black/50 backdrop-blur-sm text-gray-400 border border-white/10">
                                        {p.category}
                                    </span>
                                </div>
                                <div className="p-5 flex items-center justify-between">
                                    <div>
                                        <h3 className="text-white font-semibold mb-0.5">{p.title}</h3>
                                        <span className="text-green-400 font-bold">${p.price}</span>
                                    </div>
                                    <Link to="/register">
                                        <motion.button
                                            whileHover={{ scale: 1.05 }}
                                            whileTap={{ scale: 0.95 }}
                                            className="px-4 py-2 bg-white/10 hover:bg-white/20 text-white text-sm rounded-lg transition-colors cursor-pointer"
                                        >
                                            Buy
                                        </motion.button>
                                    </Link>
                                </div>
                            </motion.div>
                        ))}
                    </motion.div>
                </div>
            </section>

            <section id="features" className="py-24 px-6 border-t border-white/5">
                <div className="max-w-7xl mx-auto">
                    <motion.div
                        initial="hidden"
                        whileInView="visible"
                        viewport={{ once: true, margin: "-100px" }}
                        variants={stagger}
                        className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-5"
                    >
                        {FEATURES.map((f, i) => (
                            <motion.div
                                key={i}
                                variants={fadeUp}
                                className="bg-[#111217] rounded-2xl p-6 border border-white/5"
                            >
                                <div className="w-10 h-10 rounded-xl bg-white/5 flex items-center justify-center mb-4 text-purple-400">
                                    {f.icon}
                                </div>
                                <h3 className="text-white font-semibold mb-1">{f.title}</h3>
                                <p className="text-gray-600 text-sm">{f.desc}</p>
                            </motion.div>
                        ))}
                    </motion.div>
                </div>
            </section>

            <section id="about" className="py-32 px-6">
                <div className="max-w-3xl mx-auto text-center relative">
                    <div className="absolute inset-0 bg-purple-600/5 rounded-3xl blur-3xl" />
                    <motion.div
                        initial="hidden"
                        whileInView="visible"
                        viewport={{ once: true }}
                        variants={stagger}
                        className="relative bg-[#111217] rounded-3xl p-14 border border-white/5"
                    >
                        <motion.h2 variants={fadeUp} className="text-4xl md:text-5xl font-black text-white mb-4">
                            Ready to start?
                        </motion.h2>
                        <motion.p variants={fadeUp} className="text-gray-500 mb-10 text-lg">
                            Join thousands of shoppers who already trust MyMarket for their daily needs.
                        </motion.p>
                        <motion.div variants={fadeUp} className="flex items-center justify-center gap-4">
                            <Link to="/register">
                                <motion.button
                                    whileHover={{ scale: 1.03 }}
                                    whileTap={{ scale: 0.97 }}
                                    className="flex items-center gap-2 px-8 py-4 bg-white text-black font-bold rounded-xl hover:bg-gray-100 transition-colors cursor-pointer"
                                >
                                    Create free account
                                    <ArrowRight className="h-4 w-4" />
                                </motion.button>
                            </Link>
                        </motion.div>
                    </motion.div>
                </div>
            </section>

            <footer className="border-t border-white/5 py-10 px-6">
                <div className="max-w-7xl mx-auto flex flex-col md:flex-row items-center justify-between gap-4">
                    <div className="flex items-center gap-2">
                        <div className="w-6 h-6 rounded bg-gradient-to-tr from-purple-600 to-indigo-500 flex items-center justify-center">
                            <Zap className="h-3 w-3 text-white" />
                        </div>
                        <span className="text-white font-bold text-sm">E-Commerce</span>
                    </div>
                    <span className="text-gray-700 text-sm">© 2026 MyMarket. All rights reserved.</span>
                    <div className="flex gap-6 text-sm text-gray-700">
                        <a href="#" className="hover:text-gray-400 transition-colors">Privacy</a>
                        <a href="#" className="hover:text-gray-400 transition-colors">Terms</a>
                    </div>
                </div>
            </footer>

        </div>
    );
};