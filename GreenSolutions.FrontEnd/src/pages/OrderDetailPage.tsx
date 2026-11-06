import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { ordersApi } from "../services/ordersService";
import { budgetsApi } from "../services/budgetsService";
import type { Budget } from "../types/Budget";
import type { Order } from "../types/Order";
import type { OrderItemResponse } from "../types/OrderItem";

function getCoverage(items: OrderItemResponse[]) {
    const requested = items.reduce((acc, item) => acc + item.requestedQuantity, 0);
    const budgeted = items.reduce((acc, item) => acc + item.budgetedQuantity, 0);
    return {
        requested,
        budgeted,
        coverage: requested === 0 ? 0 : Math.round((budgeted / requested) * 100)
    };
}

export function OrderDetailPage() {
  const { id } = useParams<{ id: string }>();

  const [order, setOrder] = useState<Order | null>(null);
  //const [budget, setBudget] = useState<Budget[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const shippingTypeMap: Record<number, string> = {
    0: "Motorista",
    1: "Correios",
  };

  const budgetTypeMap: Record<number, string> = {
    0: "Rascunho",
    1: "Enviado",
    2: "Aceito",
    3: "Recusado",
    4: "Expirado",
    5: "Cancelado",
  };

  useEffect(() => {
    setLoading(true);
    setError("");

    void loadOrder();
  }, []);

    async function loadOrder() {
      try{
        const orderDetails  = await ordersApi.getById(Number(id));
        setOrder(orderDetails);
        console.log(orderDetails);
      }catch{
        setError("Não foi possível carregar os detalhes do pedido.");
      } finally{
        setLoading(false);
      }
    }


  async function handleDeleteBudget(id:number) {
    const confirmed = window.confirm("Deseja realmente excluir esse orçamento?");
    if(!confirmed)return;

    try{
      await budgetsApi.remove(id);
      loadOrder();
    } catch {
      setError("Não foi possível excluir o orçamento.");
    }
  }

  return (
    <div className="page-stack">
      <section className="section-card">
        <div className="page-header">
          <div>
            <h1>Pedido {id ? `#${id}` : ""}</h1>
            <p><strong>{order?.clientName}</strong></p>
            <p>{order?.companyName}</p>
          </div>
        </div>

        {loading && <h2>Carregando...</h2>}

        {error && <p className="error">{error}</p>}
        
        {!loading && order && (
            <>
              <section className="section-card">
                <dl className="order-summary-grid">
                    <div className="order-summary">
                        <dt>Total do Pedido</dt>
                        <dd> 
                            {order.totalPrice.toLocaleString("pt-BR", {
                                style: "currency",
                                currency: "BRL",
                            })}
                        </dd>
                    </div>
                    <div className="order-summary">
                        <dt>Total Orçado</dt>
                        <dd>
                            {order.budgetedPrice.toLocaleString("pt-BR", {
                                style: "currency",
                                currency: "BRL",
                            })}
                        </dd>
                    </div>
                    <div className="order-summary">
                        <dt>Cobertura</dt>
                        <dd>{getCoverage(order.orderItems).coverage}%</dd>
                    </div>
                    <div className="order-summary">
                        <dt>Entrega</dt>
                        <dd>{shippingTypeMap[order.shippingType]}</dd>
                    </div>
                    <div className="order-summary">
                        <dt>Prazo</dt>
                        <dd>{new Date(order.dueDate).toLocaleDateString('pt-BR')}</dd>
                    </div>
                  </dl>
                </section>
              <section className="section-card">
                <h2>Itens Solicitados</h2>
                <p>Confira o que precisa ser orçado.</p>
                <table className="data-table">
                    <thead>
                        <tr>
                            <th>Produto</th>
                            <th>Solicitado</th>
                            <th>Orçado</th>
                            <th>Restante</th>
                        </tr>
                    </thead>
                    <tbody>
                    {order.orderItems.map((item: OrderItemResponse, index) => (
                        <tr key={index}>
                        <td>{item.productName}</td>
                        <td>{item.requestedQuantity}</td>
                        <td>{item.budgetedQuantity}</td>
                        <td>{item.requestedQuantity - item.budgetedQuantity}</td>
                        </tr>
                    ))}
                    </tbody>
                </table>        
            </section>
            <section className="section-card">
                <h2>Orçamentos vinculados: {order.budgets.length}</h2>
                <Link className="primary-link" to={`/orcamentos/novo?orderId=${order.id}`}>
                    Novo orçamento
                </Link>
                <table className="data-table">
                    <thead>
                        <tr>
                            <th>Orçamento</th>
                            <th>Data</th>
                            <th>Itens</th>
                            <th>Total</th>
                            <th>Status</th>
                            <th>Ações</th>
                        </tr>
                    </thead>
                    <tbody>
                    {order.budgets.map((budget: Budget, index) => (
                        <tr key={index}>
                            <td>#{budget.id}</td>
                            <td>{new Date(budget.createdAt).toLocaleDateString("pt-BR")}</td>
                            <td>{budget.items.length} produtos - {budget.items.reduce((acc, item) => acc + item.quantity, 0)} unidades</td>
                            <td>{budget.totalPrice.toLocaleString("pt-BR", {
                                style: "currency",
                                currency: "BRL"
                            })}</td>
                            <td>{budgetTypeMap[budget.status] || "Desconhecido"}</td>
                            <td className="actions">
                                <Link to={`/orcamentos/${budget.id}`}>Visualizar</Link>
                                <button onClick={() => void handleDeleteBudget(budget.id)}> Excluir </button>
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>        
            </section>
          </>
        )}

        {(!loading && !order && !error) && (
          <div className="empty-state">
            <strong>Pedido não encontrado.</strong>
            <p>O pedido solicitado não existe ou não está disponível.</p>
          </div>
        )}

        
        {(!loading && order && order.orderItems.length === 0) && (
          <div className="empty-state">
            <strong>Nenhum item encontrado.</strong>
            <p>Este pedido não possui itens.</p>
          </div>
        )}

      </section> 
    </div>     
  );
}
