import { DashboardLayout } from "@/components/dashboard-layout"
import { ItemsForm } from "@/components/items-form"
import { ItemsTable } from "@/components/items-table"
import { ItemsStats } from "@/components/items-stats"

export default function ItemsPage() {
  return (
    <DashboardLayout>
      <div className="flex-1 space-y-6 p-6">
        <div>
          <h1 className="text-3xl font-bold text-foreground font-[family-name:var(--font-space-grotesk)]">
            Item Management
          </h1>
          <p className="text-muted-foreground">Manage inventory items and stock levels</p>
        </div>

        <ItemsStats />

        <div className="grid gap-6 lg:grid-cols-3">
          <div className="lg:col-span-2">
            <ItemsTable />
          </div>
          <div>
            <ItemsForm />
          </div>
        </div>
      </div>
    </DashboardLayout>
  )
}
