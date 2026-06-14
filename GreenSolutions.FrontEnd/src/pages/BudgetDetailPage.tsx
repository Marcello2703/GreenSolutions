import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { budgetsApi } from "../services/budgets";
import {  } from "../services/budgets";
import type { BudgetDetail, BudgetDetailItem } from "../types/Budget";

export function BudgetDetailPage() {
  const { id } = useParams<{ id: string }>();

  const [budget, setBudget] = useState<BudgetDetail | null>(null);
  //const [budgetItems, setBudgetItems] = useState<BudgetItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");


  useEffect(() => {
    setLoading(true);
    setError("");

    async function loadBudget() {
      try{
        const budgetDetails  = await budgetsApi.getById(Number(id));
        setBudget(budgetDetails);
        console.log(budgetDetails);
      }catch{
        setError("Não foi possível carregar os detalhes do orçamento.");
      } finally{
        setLoading(false);
      }
    }

    void loadBudget();
  }, []);

  return (
    <div className="page-stack">
      <section className="section-card">
        <div className="page-header">
          <div>
            <h1>Detalhe do orçamento</h1>
            <p>Visualização individual do orçamento {id ? `#${id}` : ""}.</p>
          </div>
        </div>

        {loading && <h2>Carregando...</h2>}

        {error && <p className="error">{error}</p>}
        
        {!loading && budget && (
          <div className="budget-detail">
            <h2>Orçamento #{budget.id}</h2>
            <p><strong>Criado em:</strong> {new Date(budget.createdAt).toLocaleString()}</p>
            <p><strong>Cliente:</strong> {budget.clientName}</p>
            <p><strong>Empresa:</strong> {budget.companyName}</p>
            <p><strong>Usuário:</strong> {budget.userName}</p>
            <p><strong>Total:</strong> R$ {budget.totalPrice.toFixed(2)}</p>
          </div>
        )}

        {(!loading && !budget && !error) && (
          <div className="empty-state">
            <strong>Orçamento não encontrado.</strong>
            <p>O orçamento solicitado não existe ou não está disponível.</p>
          </div>
        )}
      </section>
      <section className="section-card">
        <div className="page-header">
          <div>
            <h2>Itens do orçamento</h2>
            <p>Lista de produtos, quantidades e preços do orçamento.</p>
          </div>
        </div>

        {loading && <h2>Carregando itens...</h2>}

        {error && <p className="error">{error}</p>}
        
        {!loading && budget && (
          <table className="data-table">
            <thead>
              <tr>
                <th>Produto</th>
                <th>Quantidade</th>
                <th>Preço Unitário</th>
                <th>Preço Total</th>
              </tr>
            </thead>
            <tbody>
              {budget.items.map((item: BudgetDetailItem, index) => (
                <tr key={index}>
                  <td>{item.productName}</td>
                  <td>{item.quantity}</td>
                  <td>R$ {item.unitPrice.toFixed(2)}</td>
                  <td>R$ {item.totalPrice.toFixed(2)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}

        {(!loading && budget && budget.items.length === 0) && (
          <div className="empty-state">
            <strong>Nenhum item encontrado.</strong>
            <p>Este orçamento não possui itens cadastrados.</p>
          </div>
        )}
      </section> 
    </div>     
  );
}
