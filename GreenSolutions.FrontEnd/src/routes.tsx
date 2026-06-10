import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { AdministrationPage } from "./pages/AdministrationPage";
//import { BudgetCreatePage } from "./pages/BudgetCreatePage";

export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Navigate to="/administration/:section" replace />} />
        <Route path="/administration/:section" element={<AdministrationPage />} />
      </Routes>
    </BrowserRouter>
  );
}