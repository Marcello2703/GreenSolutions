export const OrderStatus = {
  0: "Em aberto",
  1: "Parcialmente orçado",
  2: "Totalmente orçado",
  3: "Parcialmente aceito",
  4: "Totalmente aceito",
  5: "Em rota",
  6: "Entregue",
  7: "Em execução",
  8: "Concluído",
  9: "Cancelado",
} as const;

export type OrderStatusCode = keyof typeof OrderStatus;