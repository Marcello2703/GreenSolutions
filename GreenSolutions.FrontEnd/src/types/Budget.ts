export type Budget = {
    id: number;
    userName: string;
    clientName: string;
    companyName: string;
    orderId: number;
    createdAt: string;
    totalPrice: number;
    status: number;
    items: BudgetDetailItem[];
}

export type BudgetSummary = {
    id: number;
    createdAt: string;
    clientName: string;
    companyName: string;
    orderId: number;
    userName: string;
    status: number;
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
  status: number;
  orderId: number;
  items: BudgetDetailItem[];
};
