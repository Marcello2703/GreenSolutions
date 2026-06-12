import type { BudgetSummary, BudgetDetail } from "../types/Budget";
import { http } from "./http";

export const budgetsApi = {
    list: async () => {
        const { data } = await http.get<BudgetSummary[]>("/budgets");
        return data;
    },
    
    remove: async (id:number) => {
        await http.delete(`/budgets/${id}`);
    },

    print:  async (id:number) => {
        const response = await http.get(`/budgets/${id}/pdf`, {
            responseType: "blob",
        });
        return response.data;
    },
    
    getById: async (id:number) => {
        const { data } = await http.get<BudgetDetail>(`/budgets/${id}`);
        return data;
    }
};