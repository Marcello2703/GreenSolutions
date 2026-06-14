import type { BudgetItem } from "./BudgetItem";

export type Budget = {
    budgetId: number;
    userId: number;
    clientId: number;
    companyId: number;
    budgetItems: BudgetItem[]
}