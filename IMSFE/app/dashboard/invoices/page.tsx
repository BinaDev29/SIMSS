import { DashboardLayout } from "@/components/dashboard-layout"
import { InvoicesTable } from "@/components/invoices-table"
import { InvoiceStats } from "@/components/invoice-stats"
import { CreateInvoiceForm } from "@/components/create-invoice-form"

export default function InvoicesPage() {
  return (
    <DashboardLayout>
      <div className="flex-1 space-y-6 p-6">
        <div>
          <h1 className="text-3xl font-bold text-foreground font-[family-name:var(--font-space-grotesk)]">
            Invoice Management
          </h1>
          <p className="text-muted-foreground">Manage all system invoices and billing</p>
        </div>

        <InvoiceStats />

        <div className="grid gap-6 lg:grid-cols-3">
          <div className="lg:col-span-2">
            <InvoicesTable />
          </div>
          <div>
            <CreateInvoiceForm />
          </div>
        </div>
      </div>
    </DashboardLayout>
  )
}
