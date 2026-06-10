export type Client = {
    id: number;
    name: string;
    cnpj: string;
    address: string;
    phone: string;
    email: string;
    state: string;
  };

  export type ClientFormData = Omit<Client, "id">;