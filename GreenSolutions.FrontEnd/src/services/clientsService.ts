import { http } from "./http";
import type { Client, ClientFormData } from "../types/Client";

type ClientApiModel = {
  id: number;
  name: string;
  cnpj: string;
  adress: string;
  phone: string;
  state: string;
};

const mapClient = (item: ClientApiModel): Client => ({
  id: item.id,
  name: item.name,
  cnpj: item.cnpj,
  address: item.adress ?? "",
  phone: item.phone ?? "",
  state: item.state ?? "",
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
