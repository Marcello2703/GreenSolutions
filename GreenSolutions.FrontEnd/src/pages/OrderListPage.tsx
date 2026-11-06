import { Link } from "react-router-dom";
import { useEffect, useState } from "react";
import type { OrderSummary } from "../types/Order";
import { ordersApi } from "../services/ordersService";
import type { OrderItemResponse } from "../types/OrderItem";
import { OrderStatus } from "../types/OrderStatus";

function getCoverage(items: OrderItemResponse[]) {
    const requested = items.reduce((acc, item) => acc + item.requestedQuantity, 0);
    const budgeted = items.reduce((acc, item) => acc + item.budgetedQuantity, 0);
    return {
        requested,
        budgeted,
        coverage: requested === 0 ? 0 : Math.round((budgeted / requested) * 100)
    };
}

export function OrderListPage() {
  const [loading, setLoading] = useState(true);
  const [orders, setOrders] = useState<OrderSummary[]>([]);
  const [error, setError] = useState("");

  const summaryStatuses = [0, 1, 2] as const;

  async function loadOrders() {
    setLoading(true);
    setError("");
    try {
      const orders = await ordersApi.list();
      setOrders(orders);
    } catch (err) {
      setError("Failed to load orders.");
    } finally {
      setLoading(false);
    }
  }

  async function handleDelete(id:number) {
    const confirmed = window.confirm("Deseja realmente excluir esse pedido?");
    if(!confirmed)
        return;
    try{
      await ordersApi.remove(id);
      await loadOrders();
    } catch {
      setError("Não foi possível excluir o pedido.");
    }
  }

  useEffect(() => {
    void loadOrders();
  }, []);

  const orderRows = orders.map((order) => ({
    ...order,
    coverage: getCoverage(order.orderItems)
  }));


  const statusCards = summaryStatuses.map(status => ({
  status,
  label: OrderStatus[status],
  count: orders.filter(order => order.status === status).length,
  }));

  return (
    <section className="section-card">
      <div className="page-header">
        <div>
          <h1>Pedidos</h1>
          <h2>Acompanhe as informações dos seus pedidos</h2>
        </div>

        <Link className="primary-link" to="/pedidos/novo">
          Novo pedido
        </Link>
      </div>

      {loading && <h2>Carregando...</h2>}
      {error && <h2>{error}</h2>}

      {!loading && !!orders.length && (
        <div className="order-status-summary">
            {statusCards.map(card => (
                <article className="status-card" key={card.status}>
                    <p>{card.label}</p>
                    <strong>{card.count}</strong>
                </article>
            ))}
        </div>
      )}

      {!loading && !orders.length && (
        <div className="empty-state">
          <strong>Nenhum pedido encontrado.</strong>
          <h2> Crie um pedido pra começar! </h2>
        </div>
      )}

      {!!orders.length && (
        <table className="data-table">
          <thead>
            <tr>
              <th>Pedido</th>
              <th>Criado em</th>
              <th>Prazo</th>
              <th>Cliente</th>
              <th>Empresa</th>
              <th>Status</th>
              <th>Cobertura</th>
              <th>Total</th>
            </tr>
          </thead>
          <tbody>
            {orderRows.map((order) => (
              <tr key={order.id}>
                <td>#{order.id}</td>
                <td>{new Date(order.createdAt).toLocaleDateString("pt-BR")}</td>
                <td>{new Date(order.dueDate).toLocaleDateString("pt-BR")}</td>
                <td>{order.clientName}</td>
                <td>{order.companyName}</td>
                <td>{OrderStatus[order.status]}</td>
                <td>
                    <progress 
                        max={100}
                        value={order.coverage.coverage}
                        aria-label={`Cobertura do pedido: ${order.id}`}
                    />
                    <span>{order.coverage.coverage}%</span>
                    <small>({order.coverage.budgeted} de {order.coverage.requested}) unidades</small>
                </td>
                <td>
                  {order.totalPrice.toLocaleString("pt-BR", {
                    style: "currency",
                    currency: "BRL",
                  })}
                </td>
                <td className="actions">
                  <Link className="table-link" to={`/pedidos/${order.id}`}>Ver detalhes</Link>
                  <button type="button" onClick={() => void handleDelete(order.id)}> Excluir </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </section>
  );
}