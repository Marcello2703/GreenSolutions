import { Link } from "react-router-dom";
import { useEffect, useState } from "react";
import { budgetsApi } from "../services/budgets";
import type { BudgetSummary } from "../types/Budget";


export function BudgetListPage() {

  //const navigate = useNavigate();
  const [loading, setLoading] = useState(true);
  const [budgets, setBudgets] = useState<BudgetSummary[]>([]);
  const [error, setError] = useState("");

  async function loadBudgets() {
    setLoading(true);
    setError("");

    try{
      const data = await budgetsApi.list();
      setBudgets(data);
    }catch{
      setError("Não foi possível carregar os orçamentos");
    } finally{
      setLoading(false);
    }
  }

  useEffect (() => {
    void loadBudgets();
  }, []);

  async function handleDelete(id:number) {
    const confirmed = window.confirm("Deseja realmente excluir esse orçamento?");
    if(!confirmed)return;

    try{
      await budgetsApi.remove(id);
      await loadBudgets();
    } catch {
      setError("Não foi possível excluir o orçamento.");
    }
  }

  return (
    <section className="section-card">
      <div className="page-header">
        <div>
          <h1>Orçamentos</h1>
          <h2>Listagem e o acesso rapido aos detalhes dos orçamentos.</h2>
        </div>

        <Link className="primary-link" to="/orcamentos/novo">
          Novo orçamento
        </Link>
      </div>

      {loading && <h2>Carregando...</h2>}
      {error && <h2>{error}</h2>}

      {!loading && !budgets.length && (
        <div className="empty-state">
          <strong>Nenhum orçamento encontrado.</strong>
          <h2> Crie um orçamento pra começar! </h2>
        </div>
      )}

      {!!budgets.length && (
        <table className="data-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Data</th>
              <th>Cliente</th>
              <th>Empresa</th>
              <th>Responsável</th>
              <th>Total</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {budgets.map((budget) => (
              <tr key={budget.id}>
                <td>{budget.id}</td>
                <td>{new Date(budget.createdAt).toLocaleDateString("pt-BR")}</td>
                <td>{budget.clientName}</td>
                <td>{budget.companyName}</td>
                <td>{budget.userName}</td>
                <td>
                  {budget.totalPrice.toLocaleString("pt-BR", {
                    style: "currency",
                    currency: "BRL",
                  })}
                </td>
                <td className="actions">
                  <Link className="table-link" to={`/orcamentos/${budget.id}`}>Ver detalhes</Link>
                  <button type="button" disabled> Imprimir PDF </button>
                  <button type="button" onClick={() => void handleDelete(budget.id)}> Excluir </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </section>
  );
}
