import type { BudgetItem } from "./BudgetItem";

export type CreateBudgetPayload = {
    userId: number;
    clientId: number;
    companyId: number;
    orderId: number;
    items: BudgetItem[];
  };
  