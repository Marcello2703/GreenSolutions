import type { BudgetItem } from "./BudgetItem";

export type Budget = {
    budgetId: number;
    userId: number;
    clientId: number;
    companyId: number;
    budgetItems: BudgetItem[]
}

export type BudgetSummary = {
    id: number;
    createdAt: string;
    clientName: string;
    companyName: string;
    userName: string;
    totalPrice: number;
};

export type BudgetDetailItem = {
  productId: number;
  productName: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
};

export type BudgetDetail = {
  id: number;
  createdAt: string;
  totalPrice: number;
  clientName: string;
  companyName: string;
  userName: string;
  items: BudgetDetailItem[];
};