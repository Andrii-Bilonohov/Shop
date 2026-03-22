import { Link } from 'react-router';
import { Zap, Github, Twitter, Instagram } from 'lucide-react';

const NAV = [
    { label: 'Catalog', to: '/catalog' },
    { label: 'Orders', to: '/orders' },
    { label: 'Cart', to: '/cart' },
];

const LEGAL = [
    { label: 'Privacy', to: '#' },
    { label: 'Terms', to: '#' },
    { label: 'Cookies', to: '#' },
];

export const Footer = () => {
    return (
        <footer className="bg-[#0a0b0f] border-t border-white/5 mt-auto">
            <div className="max-w-7xl mx-auto px-6 py-12">
                <div className="grid grid-cols-1 md:grid-cols-3 gap-10 mb-10">

                    <div>
                        <div className="flex items-center gap-2 mb-4">
                            <div className="w-8 h-8 rounded-lg bg-gradient-to-tr from-purple-600 to-indigo-500 flex items-center justify-center shadow-lg shadow-purple-900/40">
                                <Zap className="h-4 w-4 text-white" />
                            </div>
                            <span className="text-white font-bold text-lg tracking-tight">E-Commerce</span>
                        </div>
                        <p className="text-gray-600 text-sm leading-relaxed max-w-xs">
                            The smartest way to shop. Thousands of products, unbeatable prices, delivered fast.
                        </p>
                    </div>

                    <div>
                        <h4 className="text-xs uppercase tracking-widest text-gray-600 mb-4">Navigation</h4>
                        <ul className="space-y-2">
                            {NAV.map(({ label, to }) => (
                                <li key={to}>
                                    <Link to={to} className="text-gray-500 hover:text-white transition-colors text-sm">
                                        {label}
                                    </Link>
                                </li>
                            ))}
                        </ul>
                    </div>

                    <div>
                        <h4 className="text-xs uppercase tracking-widest text-gray-600 mb-4">Legal</h4>
                        <ul className="space-y-2 mb-6">
                            {LEGAL.map(({ label, to }) => (
                                <li key={to}>
                                    <a href={to} className="text-gray-500 hover:text-white transition-colors text-sm">
                                        {label}
                                    </a>
                                </li>
                            ))}
                        </ul>

                        <div className="flex gap-3">
                            {[Github, Twitter, Instagram].map((Icon, i) => (
                                <a
                                    key={i}
                                    href="#"
                                    className="w-8 h-8 rounded-lg bg-white/5 hover:bg-white/10 flex items-center justify-center transition-colors"
                                >
                                    <Icon className="h-4 w-4 text-gray-500 hover:text-white transition-colors" />
                                </a>
                            ))}
                        </div>
                    </div>
                </div>

                <div className="h-px bg-white/5 mb-6" />
                <div className="flex flex-col sm:flex-row items-center justify-between gap-3">
                    <span className="text-gray-700 text-xs">© 2026 MyMarket. All rights reserved.</span>
                    <span className="text-gray-800 text-xs">Built with ❤️ for great shopping</span>
                </div>
            </div>
        </footer>
    );
};
