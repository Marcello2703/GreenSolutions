import { http } from "./http";
import type { Product, ProductFormData } from "../types/Product";

export const productsApi = {
  list: async () => {
    const { data } = await http.get<Product[]>("/Product/getAllProducts");
    return data;
  },
  create: async (payload: ProductFormData) => {
    const { data } = await http.post<Product>("/Product", payload);
    return data;
  },
  update: async (id: number, payload: ProductFormData) => {
    const { data } = await http.put<Product>(`/Product/${id}`, payload);
    return data;
  },
  remove: async (id: number) => {
    await http.delete(`/Product/${id}`);
  },
  toForm: (product: Product): ProductFormData => ({
    name: product.name,
    basePrice: product.basePrice,
  }),
};
