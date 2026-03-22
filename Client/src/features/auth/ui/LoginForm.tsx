import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Link } from 'react-router';
import { Loader2, Mail, Lock, Zap } from 'lucide-react';
import { Input } from '@/app/components/ui/input';
import { Label } from '@/app/components/ui/label';
import { Button } from '@/app/components/ui/button';
import { loginSchema, type LoginFormData } from '../model/validation';
import { useLogin } from '../api/useLogin';
import { motion } from 'framer-motion';

export const LoginForm = () => {
    const { mutate: login, isPending } = useLogin();

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<LoginFormData>({
        resolver: zodResolver(loginSchema),
    });

    const onSubmit = (data: LoginFormData) => login(data);

    return (
        <div className="min-h-screen bg-[#0f1115] flex items-center justify-center px-4 relative overflow-hidden">



            <motion.div
                initial={{ opacity: 0, y: 28 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.45, ease: 'easeOut' }}
                className="w-full max-w-md relative"
            >
                {/* Card */}
                <div className="bg-[#1b1c22] rounded-3xl p-8 shadow-2xl shadow-black/60 border border-white/5">

                    {/* Logo mark */}
                    <div className="mb-8 flex items-center gap-3">
                        <div className="w-10 h-10 rounded-xl bg-gradient-to-tr from-purple-600 to-indigo-500 flex items-center justify-center shadow-lg shadow-purple-900/50">
                            <Zap className="h-5 w-5 text-white" />
                        </div>
                        <span className="text-white font-bold text-lg tracking-tight">E-Commerce</span>
                    </div>

                    <h1 className="text-3xl font-bold text-white mb-1">Welcome back</h1>
                    <p className="text-gray-500 text-sm mb-8">Sign in to your account to continue</p>

                    <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">

                        {/* Email */}
                        <div className="space-y-1.5">
                            <Label htmlFor="email" className="text-gray-400 text-xs uppercase tracking-widest">
                                Email
                            </Label>
                            <div className="relative">
                                <Mail className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-600" />
                                <Input
                                    id="email"
                                    type="email"
                                    placeholder="you@example.com"
                                    {...register('email')}
                                    disabled={isPending}
                                    className="pl-10 bg-[#13141a] border-gray-800 text-white placeholder-gray-700 focus:border-purple-600 focus:ring-0 rounded-xl h-11"
                                />
                            </div>
                            {errors.email && (
                                <p className="text-xs text-red-400">{errors.email.message}</p>
                            )}
                        </div>

                        {/* Password */}
                        <div className="space-y-1.5">
                            <Label htmlFor="password" className="text-gray-400 text-xs uppercase tracking-widest">
                                Password
                            </Label>
                            <div className="relative">
                                <Lock className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-600" />
                                <Input
                                    id="password"
                                    type="password"
                                    placeholder="••••••••"
                                    {...register('password')}
                                    disabled={isPending}
                                    className="pl-10 bg-[#13141a] border-gray-800 text-white placeholder-gray-700 focus:border-purple-600 focus:ring-0 rounded-xl h-11"
                                />
                            </div>
                            {errors.password && (
                                <p className="text-xs text-red-400">{errors.password.message}</p>
                            )}
                        </div>

                        {/* Submit */}
                        <Button
                            type="submit"
                            size="lg"
                            disabled={isPending}
                            className="w-full h-12 rounded-xl bg-green-600 hover:bg-green-500 text-black font-semibold text-base transition-all cursor-pointer mt-2"
                        >
                            {isPending
                                ? <><Loader2 className="mr-2 h-4 w-4 animate-spin" />Signing in...</>
                                : 'Sign in'
                            }
                        </Button>

                        {/* Divider */}
                        <div className="flex items-center gap-3">
                            <div className="flex-1 h-px bg-gray-800" />
                            <span className="text-gray-700 text-xs">or</span>
                            <div className="flex-1 h-px bg-gray-800" />
                        </div>

                        <p className="text-center text-sm text-gray-600">
                            Don't have an account?{' '}
                            <Link to="/register" className="text-purple-400 hover:text-purple-300 font-medium transition-colors">
                                Create one
                            </Link>
                        </p>
                    </form>
                </div>
            </motion.div>
        </div>
    );
};