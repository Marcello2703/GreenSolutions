import { useParams } from "react-router-dom";

export function BudgetDetailPage() {
  const { id } = useParams<{ id: string }>();

  return (
    <section className="section-card">
      <div className="page-header">
        <div>
          <h1>Detalhe do orcamento</h1>
          <p>Visualizacao individual do orcamento {id ? `#${id}` : ""}.</p>
        </div>
      </div>

      <div className="empty-state">
        <strong>Subrota preparada.</strong>
        <p>O detalhe vai receber os dados completos do orcamento e o download do PDF.</p>
      </div>
    </section>
  );
}
