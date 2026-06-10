import { http } from "./http";
import type {
  Client,
  ClientFormData,
  Company,
  CompanyFormData,
  Product,
  ProductFormData,
} from "../types/Administration";

type ClientApiModel = {
  id: number;
  name: string;
  cnpj: string;
  adress: string;
  phone: string;
  state: string;
};

type CompanyApiModel = {
  id: number;
  name: string;
  address: string;
  phone: string;
  email: string;
  cnpj: string;
  resposible: string;
};

const mapClient = (item: ClientApiModel): Client => ({
  id: item.id,
  name: item.name,
  cnpj: item.cnpj,
  address: item.adress ?? "",
  phone: item.phone ?? "",
  state: item.state ?? "",
});

const mapCompany = (item: CompanyApiModel): Company => ({
  id: item.id,
  name: item.name,
  address: item.address ?? "",
  phone: item.phone ?? "",
  email: item.email ?? "",
  cnpj: item.cnpj,
  responsible: item.resposible ?? "",
});

export const clientsApi = {
  list: async () => {
    const { data } = await http.get<ClientApiModel[]>("/Client/getAllClients");
    return data.map(mapClient);
  },
  create: async (payload: ClientFormData) => {
    const { data } = await http.post<ClientApiModel>("/Client", {
      name: payload.name,
      cnpj: payload.cnpj,
      adress: payload.address,
      phone: payload.phone,
      state: payload.state,
    });
    return mapClient(data);
  },
  update: async (id: number, payload: ClientFormData) => {
    const { data } = await http.put<ClientApiModel>(`/Client/${id}`, {
      name: payload.name,
      cnpj: payload.cnpj,
      adress: payload.address,
      phone: payload.phone,
      state: payload.state,
    });
    return mapClient(data);
  },
  remove: async (id: number) => {
    await http.delete("/Client", { params: { id } });
  },
  toForm: (client: Client): ClientFormData => ({
    name: client.name,
    cnpj: client.cnpj,
    address: client.address,
    phone: client.phone,
    state: client.state,
  }),
};

export const companiesApi = {
  list: async () => {
    const { data } = await http.get<CompanyApiModel[]>("/Company");
    return data.map(mapCompany);
  },
  create: async (payload: CompanyFormData) => {
    const { data } = await http.post<CompanyApiModel>("/Company", {
      name: payload.name,
      address: payload.address,
      phone: payload.phone,
      email: payload.email,
      cnpj: payload.cnpj,
      resposible: payload.responsible,
    });
    return mapCompany(data);
  },
  update: async (id: number, payload: CompanyFormData) => {
    const { data } = await http.put<CompanyApiModel>(`/Company/${id}`, {
      name: payload.name,
      address: payload.address,
      phone: payload.phone,
      email: payload.email,
      cnpj: payload.cnpj,
      resposible: payload.responsible,
    });
    return mapCompany(data);
  },
  remove: async (id: number) => {
    await http.delete(`/Company/${id}`);
  },
  toForm: (company: Company): CompanyFormData => ({
    name: company.name,
    address: company.address,
    phone: company.phone,
    email: company.email,
    cnpj: company.cnpj,
    responsible: company.responsible,
  }),
};

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