import {useQueries} from "@tanstack/react-query";
import {productApi} from "@/entities/product";

export const useProductsByIds = (ids: string[]) => {
    const queries = useQueries({
        queries: ids.map((id) => ({
            queryKey: ['product', id],
            queryFn: () => productApi.getById(id),
            enabled: !!id,
        })),
    });

    return {
        products: queries.map((q) => q.data).filter(Boolean),
        isLoading: queries.some((q) => q.isLoading),
        isError: queries.some((q) => q.isError),
    };
};