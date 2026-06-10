export type Product = {
    id: number;
    name: string;
    basePrice: number;
  };

  export type ProductFormData = Omit<Product, "id">;
