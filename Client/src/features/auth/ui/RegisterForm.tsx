import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Link } from 'react-router';
import { Loader2, Mail, Lock, User, Zap } from 'lucide-react';
import { motion } from 'framer-motion';
import { Input } from '@/app/components/ui/input';
import { Label } from '@/app/components/ui/label';
import { Button } from '@/app/components/ui/button';
import { registerSchema, type RegisterFormData } from '../model/validation';
import { useRegister } from '../api/useRegister';

export const RegisterForm = () => {
    const { mutate: register, isPending } = useRegister();

    const {
        register: formRegister,
        handleSubmit,
        formState: { errors },
        watch,
    } = useForm<RegisterFormData>({
        resolver: zodResolver(registerSchema),
    });

    const selectedRole = watch('role');

    const onSubmit = (data: RegisterFormData) => {
        const { confirmPassword, ...credentials } = data;
        register(credentials);
    };

    return (
        <div className="min-h-screen bg-[#0f1115] flex items-center justify-center px-4 py-10">
            <motion.div
                initial={{ opacity: 0, y: 28 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.45, ease: 'easeOut' }}
                className="w-full max-w-md"
            >
                <div className="bg-[#1b1c22] rounded-3xl p-8 shadow-2xl shadow-black/60 border border-white/5">
                    {/* Logo */}
                    <div className="mb-8 flex items-center gap-3">
                        <div className="w-10 h-10 rounded-xl bg-gradient-to-tr from-purple-600 to-indigo-500 flex items-center justify-center shadow-lg shadow-purple-900/50">
                            <Zap className="h-5 w-5 text-white" />
                        </div>
                        <span className="text-white font-bold text-lg tracking-tight">E-Commerce</span>
                    </div>

                    <h1 className="text-3xl font-bold text-white mb-1">Create account</h1>
                    <p className="text-gray-500 text-sm mb-8">Sign up to start shopping</p>

                    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">

                        {/* First + Last name */}
                        <div className="grid grid-cols-2 gap-3">
                            <div className="space-y-1.5">
                                <Label htmlFor="firstName" className="text-gray-500 text-xs uppercase tracking-widest">
                                    First name
                                </Label>
                                <div className="relative">
                                    <User className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-600" />
                                    <Input
                                        id="firstName"
                                        type="text"
                                        placeholder="John"
                                        {...formRegister('firstName')}
                                        disabled={isPending}
                                        className="pl-10 bg-[#13141a] border-gray-800 text-white placeholder-gray-700 focus:border-purple-600 focus:ring-0 rounded-xl h-11"
                                    />
                                </div>
                                {errors.firstName && <p className="text-xs text-red-400">{errors.firstName.message}</p>}
                            </div>

                            <div className="space-y-1.5">
                                <Label htmlFor="lastName" className="text-gray-500 text-xs uppercase tracking-widest">
                                    Last name
                                </Label>
                                <div className="relative">
                                    <User className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-600" />
                                    <Input
                                        id="lastName"
                                        type="text"
                                        placeholder="Doe"
                                        {...formRegister('lastName')}
                                        disabled={isPending}
                                        className="pl-10 bg-[#13141a] border-gray-800 text-white placeholder-gray-700 focus:border-purple-600 focus:ring-0 rounded-xl h-11"
                                    />
                                </div>
                                {errors.lastName && <p className="text-xs text-red-400">{errors.lastName.message}</p>}
                            </div>
                        </div>

                        {/* Email */}
                        <div className="space-y-1.5">
                            <Label htmlFor="email" className="text-gray-500 text-xs uppercase tracking-widest">
                                Email
                            </Label>
                            <div className="relative">
                                <Mail className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-600" />
                                <Input
                                    id="email"
                                    type="email"
                                    placeholder="you@example.com"
                                    {...formRegister('email')}
                                    disabled={isPending}
                                    className="pl-10 bg-[#13141a] border-gray-800 text-white placeholder-gray-700 focus:border-purple-600 focus:ring-0 rounded-xl h-11"
                                />
                            </div>
                            {errors.email && <p className="text-xs text-red-400">{errors.email.message}</p>}
                        </div>

                        {/* Role */}
                        <div className="space-y-1.5">
                            <Label className="text-gray-500 text-xs uppercase tracking-widest">Role</Label>
                            <div className="grid grid-cols-2 gap-3">
                                {(['Buyer', 'Seller'] as const).map((role) => (
                                    <label
                                        key={role}
                                        className={`flex items-center justify-center gap-2 h-11 rounded-xl border cursor-pointer transition-all text-sm font-medium ${
                                            selectedRole === role
                                                ? 'bg-purple-900/40 border-purple-600 text-purple-300'
                                                : 'bg-[#13141a] border-gray-800 text-gray-500 hover:border-gray-600'
                                        }`}
                                    >
                                        <input
                                            type="radio"
                                            value={role}
                                            {...formRegister('role')}
                                            disabled={isPending}
                                            className="sr-only"
                                        />
                                        {role}
                                    </label>
                                ))}
                            </div>
                            {errors.role && <p className="text-xs text-red-400">{errors.role.message}</p>}
                        </div>

                        {/* Password */}
                        <div className="space-y-1.5">
                            <Label htmlFor="password" className="text-gray-500 text-xs uppercase tracking-widest">
                                Password
                            </Label>
                            <div className="relative">
                                <Lock className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-600" />
                                <Input
                                    id="password"
                                    type="password"
                                    placeholder="••••••••"
                                    {...formRegister('password')}
                                    disabled={isPending}
                                    className="pl-10 bg-[#13141a] border-gray-800 text-white placeholder-gray-700 focus:border-purple-600 focus:ring-0 rounded-xl h-11"
                                />
                            </div>
                            {errors.password && <p className="text-xs text-red-400">{errors.password.message}</p>}
                        </div>

                        {/* Confirm password */}
                        <div className="space-y-1.5">
                            <Label htmlFor="confirmPassword" className="text-gray-500 text-xs uppercase tracking-widest">
                                Confirm password
                            </Label>
                            <div className="relative">
                                <Lock className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-600" />
                                <Input
                                    id="confirmPassword"
                                    type="password"
                                    placeholder="••••••••"
                                    {...formRegister('confirmPassword')}
                                    disabled={isPending}
                                    className="pl-10 bg-[#13141a] border-gray-800 text-white placeholder-gray-700 focus:border-purple-600 focus:ring-0 rounded-xl h-11"
                                />
                            </div>
                            {errors.confirmPassword && <p className="text-xs text-red-400">{errors.confirmPassword.message}</p>}
                        </div>

                        {/* Submit */}
                        <Button
                            type="submit"
                            size="lg"
                            disabled={isPending}
                            className="w-full h-12 rounded-xl bg-green-600 hover:bg-green-500 text-black font-semibold text-base transition-all cursor-pointer mt-2"
                        >
                            {isPending
                                ? <><Loader2 className="mr-2 h-4 w-4 animate-spin" />Creating account...</>
                                : 'Create account'
                            }
                        </Button>

                        <div className="flex items-center gap-3">
                            <div className="flex-1 h-px bg-gray-800" />
                            <span className="text-gray-700 text-xs">or</span>
                            <div className="flex-1 h-px bg-gray-800" />
                        </div>

                        <p className="text-center text-sm text-gray-600">
                            Already have an account?{' '}
                            <Link to="/login" className="text-purple-400 hover:text-purple-300 font-medium transition-colors">
                                Sign in
                            </Link>
                        </p>
                    </form>
                </div>
            </motion.div>
        </div>
    );
};