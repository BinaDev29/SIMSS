import { DashboardLayout } from "@/components/dashboard-layout"
import { InvoiceDetail } from "@/components/invoice-detail"

interface InvoiceDetailPageProps {
  params: {
    id: string
  }
}

export default function InvoiceDetailPage({ params }: InvoiceDetailPageProps) {
  return (
    <DashboardLayout>
      <div className="flex-1 p-6">
        <InvoiceDetail invoiceId={params.id} />
      </div>
    </DashboardLayout>
  )
}
