import { productsApi } from "../../services/products";
import { useCrudSection } from "../../hooks/useCrudSection";
import type { ProductFormData } from "../../types/Product";

const emptyForm: ProductFormData = {
  name: "",
  basePrice: 0,
};

export function ProductsPanel() {
  const crud = useCrudSection(productsApi, emptyForm);

  return (
    <section className="section-card">
      <div className="page-header">
        <div>
          <h1>Produtos</h1>
          <p>Produtos e precos-base usados na geracao dos orcamentos.</p>
        </div>
      </div>

      <form className="form-grid" onSubmit={crud.submit}>
        <input value={crud.form.name} placeholder="Nome do produto" onChange={(e) => crud.setForm({ ...crud.form, name: e.target.value })} />
        <input
          type="number"
          min="0"
          step="0.01"
          value={crud.form.basePrice}
          placeholder="Preco base"
          onChange={(e) => crud.setForm({ ...crud.form, basePrice: Number(e.target.value) })}
        />

        <div className="actions">
          <button type="submit">{crud.editingId ? "Salvar produto" : "Adicionar produto"}</button>
          {crud.editingId && <button type="button" onClick={crud.cancelEdit}>Cancelar</button>}
        </div>
      </form>

      {crud.loading ? <p>Carregando...</p> : (
        <table className="data-table">
          <thead>
            <tr>
              <th>Nome</th>
              <th>Preco base</th>
              <th />
            </tr>
          </thead>
          <tbody>
            {crud.items.map((product) => (
              <tr key={product.id}>
                <td>{product.name}</td>
                <td>{product.basePrice.toLocaleString("pt-BR", { style: "currency", currency: "BRL" })}</td>
                <td className="actions">
                  <button type="button" onClick={() => crud.startEdit(product)}>Editar</button>
                  <button type="button" onClick={() => window.confirm("Excluir produto?") && void crud.remove(product.id)}>Excluir</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </section>
  );
}
