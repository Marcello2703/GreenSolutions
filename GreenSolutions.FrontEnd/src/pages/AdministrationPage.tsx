// src/pages/AdministrationPage.tsx
import { Navigate, useParams } from "react-router-dom";
import { AdministrationSidebar } from "../components/administration/AdministrationSidebar";
import { ClientsPanel } from "../components/administration/ClientsPanel";
import { CompaniesPanel } from "../components/administration/CompaniesPanel";
import { ProductsPanel } from "../components/administration/ProductsPanel";

type AdministrationSection = "clients" | "companies" | "products"; 

export function AdministrationPage() {
    const { section } = useParams<{section: AdministrationSection }>();

    if (!section || !["clients", "companies", "products"].includes(section)){
        return <Navigate to="/administration/products" replace />;
    }

    return (
        <div className="app-shell">
          <AdministrationSidebar />
    
          <main className="content">
            {section === "clients" && <ClientsPanel />}
            {section === "companies" && <CompaniesPanel />}
            {section === "products" && <ProductsPanel />}
          </main>
        </div>
    );
    
}