export type OrderItem = {
    productId: number;
    requestedQuantity: number;
  };

export type OrderItemResponse = {
  orderItemId: number;
  productId: number;
  productName: string;
  requestedQuantity: number;
  budgetedQuantity: number;
};