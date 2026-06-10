import { companiesApi } from "../../services/administration";
import { useCrudSection } from "../../hooks/useCrudSection";
import type { CompanyFormData } from "../../types/Administration";

const emptyForm: CompanyFormData = {
  name: "",
  address: "",
  phone: "",
  email: "",
  cnpj: "",
  responsible: "",
};

export function CompaniesPanel() {
  const crud = useCrudSection(companiesApi, emptyForm);

  return (
    <section className="section-card">
      <div className="page-header">
        <div>
          <h1>Companhias</h1>
          <p>Empresas próprias e parceiras para uso no orçamento.</p>
        </div>
      </div>

      <form className="form-grid" onSubmit={crud.submit}>
        <input value={crud.form.name} placeholder="Nome" onChange={(e) => crud.setForm({ ...crud.form, name: e.target.value })} />
        <input value={crud.form.cnpj} placeholder="CNPJ" onChange={(e) => crud.setForm({ ...crud.form, cnpj: e.target.value })} />
        <input value={crud.form.email} placeholder="E-mail" onChange={(e) => crud.setForm({ ...crud.form, email: e.target.value })} />
        <input value={crud.form.phone} placeholder="Telefone" onChange={(e) => crud.setForm({ ...crud.form, phone: e.target.value })} />
        <input value={crud.form.address} placeholder="Endereço" onChange={(e) => crud.setForm({ ...crud.form, address: e.target.value })} />
        <input value={crud.form.responsible} placeholder="Responsável" onChange={(e) => crud.setForm({ ...crud.form, responsible: e.target.value })} />

        <div className="actions">
          <button type="submit">{crud.editingId ? "Salvar companhia" : "Adicionar companhia"}</button>
          {crud.editingId && <button type="button" onClick={crud.cancelEdit}>Cancelar</button>}
        </div>
      </form>

      {crud.loading ? <p>Carregando...</p> : (
        <table className="data-table">
          <thead>
            <tr>
              <th>Nome</th>
              <th>CNPJ</th>
              <th>E-mail</th>
              <th>Responsável</th>
              <th />
            </tr>
          </thead>
          <tbody>
            {crud.items.map((company) => (
              <tr key={company.id}>
                <td>{company.name}</td>
                <td>{company.cnpj}</td>
                <td>{company.email}</td>
                <td>{company.responsible}</td>
                <td className="actions">
                  <button type="button" onClick={() => crud.startEdit(company)}>Editar</button>
                  <button type="button" onClick={() => window.confirm("Excluir companhia?") && void crud.remove(company.id)}>Excluir</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </section>
  );
}