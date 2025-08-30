import { DashboardLayout } from "@/components/dashboard-layout"
import { InwardsForm } from "@/components/inwards-form"
import { InwardsTable } from "@/components/inwards-table"

export default function InwardsPage() {
  return (
    <DashboardLayout>
      <div className="flex-1 space-y-6 p-6">
        <div>
          <h1 className="text-3xl font-bold text-foreground font-[family-name:var(--font-space-grotesk)]">
            Record New Inwards
          </h1>
          <p className="text-muted-foreground">Record items entering the warehouse from suppliers</p>
        </div>

        <div className="grid gap-6 lg:grid-cols-2">
          <InwardsForm />
          <InwardsTable />
        </div>
      </div>
    </DashboardLayout>
  )
}
