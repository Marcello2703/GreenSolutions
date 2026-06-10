export type Client = {
    id: number;
    name: string;
    cnpj: string;
    address: string;
    phone: string;
    state: string;
  };
  
  export type ClientFormData = Omit<Client, "id">;
  
  export type Company = {
    id: number;
    name: string;
    address: string;
    phone: string;
    email: string;
    cnpj: string;
    responsible: string;
  };
  
  export type CompanyFormData = Omit<Company, "id">;
  
  export type Product = {
    id: number;
    name: string;
    basePrice: number;
  };
  
  export type ProductFormData = Omit<Product, "id">;