export const ProductCardSkeleton = () => (
    <div className="bg-[#1a1b23] rounded-xl overflow-hidden animate-pulse">
        <div className="w-full h-48 bg-[#2a2b35]" />

        <div className="p-4 space-y-3">
            <div className="h-3 w-16 bg-[#2a2b35] rounded-full" />

            <div className="h-5 w-3/4 bg-[#2a2b35] rounded-md" />

            <div className="space-y-1.5">
                <div className="h-3 w-full bg-[#2a2b35] rounded-md" />
                <div className="h-3 w-2/3 bg-[#2a2b35] rounded-md" />
            </div>

            <div className="flex items-center justify-between pt-2">
                <div className="h-6 w-20 bg-[#2a2b35] rounded-md" />
                <div className="h-9 w-24 bg-[#2a2b35] rounded-lg" />
            </div>
        </div>
    </div>
);
