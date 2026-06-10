import { NavLink } from "react-router-dom";

const links = [
  { to: "/administration/clients", label: "Clientes" },
  { to: "/administration/companies", label: "Companhias" },
  { to: "/administration/products", label: "Produtos" },
  { to: "/budgets/new", label: "Novo orçamento" },
];

export function AdministrationSidebar() {
  return (
    <aside className="admin-sidebar">
      <div className="brand">
        <h2>GreenSolutions</h2>
        <p>Administration</p>
      </div>

      <nav className="nav-list">
        {links.map((link) => (
          <NavLink
            key={link.to}
            to={link.to}
            className={({ isActive }) =>
              isActive ? "nav-link nav-link--active" : "nav-link"
            }
          >
            {link.label}
          </NavLink>
        ))}
      </nav>
    </aside>
  );
}