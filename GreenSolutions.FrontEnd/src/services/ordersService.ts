import { http } from "./http";
import type { Order, OrderSummary } from "../types/Order.ts";

export const ordersApi = {
    list: async () => {
        const { data } = await http.get<OrderSummary[]>("/order");
        return data;
    }, 

    remove: async (id:number) => {
        await http.delete(`/order/${id}`);
    },
     
    getById: async (id:number) => {
        const { data } = await http.get<Order>(`/order/${id}`);
        return data;
    }
}