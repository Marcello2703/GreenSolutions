import { NavLink } from "react-router-dom";

const links = [
  { to: "/cadastros/clientes", label: "Cadastros" },
  { to: "/produtos", label: "Produtos" },
  { to: "/orcamentos", label: "Orçamentos" },
];

export function Sidebar() {
  return (
    <aside className="app-sidebar">
      <div className="brand">
        <h2>Green Soluções</h2>
        <p>Menu</p>
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
