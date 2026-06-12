import { http } from "./http";
import type { Company, CompanyFormData } from "../types/Company";

type CompanyApiModel = {
  id: number;
  name: string;
  address: string;
  phone: string;
  email: string;
  cnpj: string;
  responsible: string;
};

const mapCompany = (item: CompanyApiModel): Company => ({
  id: item.id,
  name: item.name,
  address: item.address ?? "",
  phone: item.phone ?? "",
  email: item.email ?? "",
  cnpj: item.cnpj,
  responsible: item.responsible ?? "",
});

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
      responsible: payload.responsible,
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
      responsible: payload.responsible,
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
