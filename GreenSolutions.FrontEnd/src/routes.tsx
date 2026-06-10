import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { Sidebar } from "./components/layout/Sidebar";
import { BudgetDetailPage } from "./pages/BudgetDetailPage";
import { BudgetListPage } from "./pages/BudgetListPage";
import { BudgetCreatePage } from "./pages/BudgetCreatePage";
import { CadastrosPage } from "./pages/CadastrosPage";
import { ProductsPage } from "./pages/ProductsPage";

function AppLayout() {
  return (
    <div className="app-shell">
      <Sidebar />
      <main className="content">
        <Routes>
          <Route path="/" element={<Navigate to="/cadastros/clientes" replace />} />
          <Route path="/cadastros" element={<Navigate to="/cadastros/clientes" replace />} />
          <Route path="/cadastros/:section" element={<CadastrosPage />} />
          <Route path="/produtos" element={<ProductsPage />} />
          <Route path="/orcamentos" element={<BudgetListPage />} />
          <Route path="/orcamentos/novo" element={<BudgetCreatePage />} />
          <Route path="/orcamentos/:id" element={<BudgetDetailPage />} />
        </Routes>
      </main>
    </div>
  );
}

export function AppRouter() {
  return (
    <BrowserRouter>
      <AppLayout />
    </BrowserRouter>
  );
}
