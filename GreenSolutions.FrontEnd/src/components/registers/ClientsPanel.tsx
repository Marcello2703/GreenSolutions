import { clientsApi } from "../../services/clients";
import { useCrudSection } from "../../hooks/useCrudSection";
import type { ClientFormData } from "../../types/Client";

const emptyForm: ClientFormData = {
  name: "",
  cnpj: "",
  address: "",
  phone: "",
  state: "SP",
};

export function ClientsPanel() {
  const crud = useCrudSection(clientsApi, emptyForm);

  return (
    <section className="section-card">
      <div className="page-header">
        <div>
          <h1>Clientes</h1>
          <p>Cadastro, edicao, exclusao e visualizacao de clientes.</p>
        </div>
      </div>

      <form className="form-grid" onSubmit={crud.submit}>
        <input value={crud.form.name} placeholder="Nome" onChange={(e) => crud.setForm({ ...crud.form, name: e.target.value })} />
        <input value={crud.form.cnpj} placeholder="CNPJ" onChange={(e) => crud.setForm({ ...crud.form, cnpj: e.target.value })} />
        <input value={crud.form.address} placeholder="Endereco" onChange={(e) => crud.setForm({ ...crud.form, address: e.target.value })} />
        <input value={crud.form.phone} placeholder="Telefone" onChange={(e) => crud.setForm({ ...crud.form, phone: e.target.value })} />
        <input value={crud.form.state} placeholder="UF" onChange={(e) => crud.setForm({ ...crud.form, state: e.target.value.toUpperCase() })} />

        <div className="actions">
          <button type="submit">{crud.editingId ? "Salvar cliente" : "Adicionar cliente"}</button>
          {crud.editingId && <button type="button" onClick={crud.cancelEdit}>Cancelar</button>}
        </div>
      </form>

      {crud.loading ? <p>Carregando...</p> : (
        <table className="data-table">
          <thead>
            <tr>
              <th>Nome</th>
              <th>CNPJ</th>
              <th>UF</th>
              <th>Telefone</th>
              <th />
            </tr>
          </thead>
          <tbody>
            {crud.items.map((client) => (
              <tr key={client.id}>
                <td>{client.name}</td>
                <td>{client.cnpj}</td>
                <td>{client.state}</td>
                <td>{client.phone}</td>
                <td className="actions">
                  <button type="button" onClick={() => crud.startEdit(client)}>Editar</button>
                  <button type="button" onClick={() => window.confirm("Excluir cliente?") && void crud.remove(client.id)}>Excluir</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </section>
  );
}
