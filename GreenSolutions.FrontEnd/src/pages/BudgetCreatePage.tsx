import { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { http } from "../services/http";
import type { Client } from "../types/Client";
import type { Company } from "../types/Company";
import type { Product } from "../types/Product";
import type { CreateBudgetPayload } from "../types/CreateBudgetPayload";

type User = {
  id: number;
  name: string;
};

type BudgetReferenceData = {
  users: User[];
  clients: Client[];
  companies: Company[];
  products: Product[];
};

type DraftItem = {
  productId: number;
  quantity: number;
};

const emptyForm: CreateBudgetPayload = {
  userId: 0,
  clientId: 0,
  companyId: 0,
  items: [],
};

export function BudgetCreatePage() {

  const navigate = useNavigate();
  const [reference, setReference] = useState<BudgetReferenceData | null>(null);
  const [form, setForm] = useState<CreateBudgetPayload>(emptyForm);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    let cancelled = false;

    async function load(){
      setLoading(true);
      try {
        const { data } = await http.get<BudgetReferenceData>("/budgets/reference-data");
        if(!cancelled){
          setReference(data);
          setForm((current) => ({
            userId: current.userId || data.users[0]?.id || 0,
            clientId: current.clientId || 0,
            companyId: current.companyId || 0,
            items: current.items.length ? current.items : [{ productId: data.products[0]?.id || 0, quantity: 1 }],
          }));
        }
      }catch{
        if (!cancelled) setError("Nao foi possivel carregar os dados do formulario.");
      } finally {
        if (!cancelled) setLoading(false);
    }
  }

  void load();
    return () => {
      cancelled = true;
    };
  }, []);

  const selectedClient = reference?.clients.find((client) => client.id === form.clientId);

  const itemsPreview = useMemo(() => {
    if (!reference) return [];
    const surcharge = selectedClient?.state === "SP" ? 1 : 1.18;

    return form.items.map((item) => {
      const product = reference.products.find((p) => p.id === item.productId);
      const unitPrice = (product?.basePrice ?? 0) * surcharge;
      const totalPrice = unitPrice * item.quantity;

      return {
        ...item,
        productName: product?.name ?? "Produto",
        unitPrice,
        totalPrice,
      };
    });
  }, [form.items, reference, selectedClient]);

  const total = useMemo(
    () => itemsPreview.reduce((sum, item) => sum + item.totalPrice, 0),
    [itemsPreview]
  );

  function updateItem(index: number, patch: Partial<DraftItem>) {
    setForm((current) => ({
      ...current,
      items: current.items.map((item, itemIndex) =>
        itemIndex === index ? { ...item, ...patch } : item
      ),
    }));
  }

  function addItem() {
    setForm((current) => ({
      ...current,
      items: [...current.items, { productId: reference?.products[0]?.id || 0, quantity: 1 }],
    }));
  }

  function removeItem(index: number) {
    setForm((current) => ({
      ...current,
      items: current.items.filter((_, itemIndex) => itemIndex !== index),
    }));
  }

  async function submit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError("");

    if (!form.userId || !form.clientId || !form.companyId || form.items.length === 0) {
      setError("Preencha usuario, cliente, empresa e pelo menos um item.");
      return;
    }

    setSaving(true);
    try {
      const { data } = await http.post<{ id: number }>("/budgets", form);
      navigate(`/orcamentos/${data.id}`);
    } catch {
      setError("Nao foi possivel criar o orcamento.");
    } finally {
      setSaving(false);
    }
  }

  if (loading) return <section className="section-card"><p>Carregando...</p></section>;

  return (
    <form className="page-stack" onSubmit={submit}>
      <section className="section-card">
        <div className="page-header">
          <div>
            <h1>Novo orçamento</h1>
            <h2>Olá Usuário,</h2>
            <p>Selecione os dados principais e monte os itens do orcamento.</p>
          </div>
        </div>

        <div className="form-grid">
          <select value={form.userId} onChange={(e) => setForm({ ...form, userId: Number(e.target.value) })}>
            <option value={0}>Selecione o usuario</option>
            {reference?.users.map((user) => <option key={user.id} value={user.id}>{user.name}</option>)}
          </select>

          <select value={form.clientId} onChange={(e) => setForm({ ...form, clientId: Number(e.target.value) })}>
            <option value={0}>Selecione o cliente</option>
            {reference?.clients.map((client) => <option key={client.id} value={client.id}>{client.name}</option>)}
          </select>

          <select value={form.companyId} onChange={(e) => setForm({ ...form, companyId: Number(e.target.value) })}>
            <option value={0}>Selecione a empresa</option>
            {reference?.companies.map((company) => <option key={company.id} value={company.id}>{company.name}</option>)}
          </select>
        </div>
      </section>

      <section className="section-card">
        <div className="page-header">
          <div>
            <h2>Itens</h2>
            <p>Adicione produtos e quantidades.</p>
          </div>
        </div>

        <table className="data-table">
          <thead>
            <tr>
              <th>Produto</th>
              <th>Quantidade</th>
              <th>Unitario</th>
              <th>Total</th>
              <th />
            </tr>
          </thead>
          <tbody>
            {form.items.map((item, index) => (
              <tr key={index}>
                <td>
                  <select value={item.productId} onChange={(e) => updateItem(index, { productId: Number(e.target.value) })}>
                    {reference?.products.map((product) => (
                      <option key={product.id} value={product.id}>{product.name}</option>
                    ))}
                  </select>
                </td>
                <td>
                  <input
                    type="number"
                    min="1"
                    value={item.quantity}
                    onChange={(e) => updateItem(index, { quantity: Number(e.target.value) })}
                  />
                </td>
                <td>{itemsPreview[index]?.unitPrice.toLocaleString("pt-BR", { style: "currency", currency: "BRL" })}</td>
                <td>{itemsPreview[index]?.totalPrice.toLocaleString("pt-BR", { style: "currency", currency: "BRL" })}</td>
                <td><button type="button" onClick={() => removeItem(index)}>Remover</button></td>
              </tr>
            ))}
          </tbody>
        </table>
        <div className="actions">
          <button type="button" onClick={addItem}>Adicionar item</button>
        </div>
      </section>

      <section className="section-card">
        <h2>Resumo</h2>
        <p>Total estimado: {total.toLocaleString("pt-BR", { style: "currency", currency: "BRL" })}</p>
        {error && <p>{error}</p>}

        <div className="actions">
          <button type="submit" disabled={saving}>
            {saving ? "Salvando..." : "Criar orcamento"}
          </button>
        </div>
      </section>
    </form>
  );
}