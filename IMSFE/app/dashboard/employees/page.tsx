import { DashboardLayout } from "@/components/dashboard-layout"
import { EmployeesForm } from "@/components/employees-form"
import { EmployeesTable } from "@/components/employees-table"
import { EmployeesStats } from "@/components/employees-stats"

export default function EmployeesPage() {
  return (
    <DashboardLayout>
      <div className="flex-1 space-y-6 p-6">
        <div>
          <h1 className="text-3xl font-bold text-foreground font-[family-name:var(--font-space-grotesk)]">
            Employee Management
          </h1>
          <p className="text-muted-foreground">Manage employee records and information</p>
        </div>

        <EmployeesStats />

        <div className="grid gap-6 lg:grid-cols-3">
          <div className="lg:col-span-2">
            <EmployeesTable />
          </div>
          <div>
            <EmployeesForm />
          </div>
        </div>
      </div>
    </DashboardLayout>
  )
}
