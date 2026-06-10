import { NavLink, Navigate, useParams } from "react-router-dom";
import { ClientsPanel } from "../components/administration/ClientsPanel";
import { CompaniesPanel } from "../components/administration/CompaniesPanel";

type CadastroSection = "clientes" | "empresas";

const tabs = [
  { to: "/cadastros/clientes", label: "Clientes" },
  { to: "/cadastros/empresas", label: "Empresas" },
] as const;

export function CadastrosPage() {
  const { section } = useParams<{ section: CadastroSection }>();

  if (!section || !["clientes", "empresas"].includes(section)) {
    return <Navigate to="/cadastros/clientes" replace />;
  }

  return (
    <div className="page-stack">
      <div className="page-header">
        <div>
          <h1>Cadastros</h1>
          <p>Gerencie clientes e empresas.</p>
        </div>
      </div>

      <div className="tab-list" role="tablist" aria-label="Secoes de cadastro">
        {tabs.map((tab) => (
          <NavLink
            key={tab.to}
            to={tab.to}
            className={({ isActive }) =>
              isActive ? "tab-link tab-link--active" : "tab-link"
            }
          >
            {tab.label}
          </NavLink>
        ))}
      </div>

      <div className="tab-panel">
        {section === "clientes" && <ClientsPanel />}
        {section === "empresas" && <CompaniesPanel />}
      </div>
    </div>
  );
}
