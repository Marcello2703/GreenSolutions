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
