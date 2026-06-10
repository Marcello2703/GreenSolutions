import { Link } from "react-router-dom";

export function BudgetListPage() {
  return (
    <section className="section-card">
      <div className="page-header">
        <div>
          <h1>Orcamentos</h1>
          <p>Essa pagina vai concentrar a listagem e o acesso rapido aos detalhes.</p>
        </div>

        <Link className="primary-link" to="/orcamentos/novo">
          Novo orcamento
        </Link>
      </div>

      <div className="empty-state">
        <strong>Estrutura pronta.</strong>
        <p>O proximo passo aqui e ligar a listagem real dos orcamentos vindos do backend.</p>
      </div>
    </section>
  );
}
