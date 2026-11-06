import type { Budget } from "./Budget";
import type { OrderItem, OrderItemResponse } from "./OrderItem";
import type { OrderStatusCode } from "./OrderStatus";

export type Order = {
    id: number;
    userName: string;
    clientId: number;
    clientName: string;
    companyId: number;
    companyName: string;
    createdAt: string;
    dueDate: string;
    description: string;
    status: OrderStatusCode;
    shippingType: number;
    totalPrice: number;
    budgetedPrice: number;
    orderItems: OrderItemResponse[];
    budgets: Budget[];
};

export type OrderSummary = {
    id: number;
    createdAt: string;
    dueDate: string;
    status: OrderStatusCode;
    shippingType: string;
    clientName: string;
    companyName: string;
    totalPrice: number;
    orderItems: OrderItemResponse[];
};

export type OrderPayload = {
    userId: number;
    clientId: number;
    companyId: number;
    dueDate: string;
    description: string;
    orderItems: OrderItem[];
};