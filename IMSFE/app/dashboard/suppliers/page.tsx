import { DashboardLayout } from "@/components/dashboard-layout"
import { SuppliersForm } from "@/components/suppliers-form"
import { SuppliersTable } from "@/components/suppliers-table"
import { SuppliersStats } from "@/components/suppliers-stats"

export default function SuppliersPage() {
  return (
    <DashboardLayout>
      <div className="flex-1 space-y-6 p-6">
        <div>
          <h1 className="text-3xl font-bold text-foreground font-[family-name:var(--font-space-grotesk)]">
            Supplier Management
          </h1>
          <p className="text-muted-foreground">Manage supplier information and partnerships</p>
        </div>

        <SuppliersStats />

        <div className="grid gap-6 lg:grid-cols-3">
          <div className="lg:col-span-2">
            <SuppliersTable />
          </div>
          <div>
            <SuppliersForm />
          </div>
        </div>
      </div>
    </DashboardLayout>
  )
}
