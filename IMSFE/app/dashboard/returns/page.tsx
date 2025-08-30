import { DashboardLayout } from "@/components/dashboard-layout"
import { ReturnsForm } from "@/components/returns-form"
import { ReturnsTable } from "@/components/returns-table"

export default function ReturnsPage() {
  return (
    <DashboardLayout>
      <div className="flex-1 space-y-6 p-6">
        <div>
          <h1 className="text-3xl font-bold text-foreground font-[family-name:var(--font-space-grotesk)]">
            Record Item Returns
          </h1>
          <p className="text-muted-foreground">Process returned items from customers</p>
        </div>

        <div className="grid gap-6 lg:grid-cols-2">
          <ReturnsForm />
          <ReturnsTable />
        </div>
      </div>
    </DashboardLayout>
  )
}
