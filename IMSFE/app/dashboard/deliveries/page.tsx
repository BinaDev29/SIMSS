"use client"

import { useState } from "react"
import { DashboardLayout } from "@/components/dashboard-layout"
import { DeliveriesForm } from "@/components/deliveries-form"
import { DeliveriesTable } from "@/components/deliveries-table"
import { AuthGuard } from "@/components/auth-guard"

export default function DeliveriesPage() {
  const [refreshTrigger, setRefreshTrigger] = useState(0)
  const [editingTransaction, setEditingTransaction] = useState<any>(null)

  const handleTransactionAdded = () => {
    setRefreshTrigger((prev) => prev + 1)
  }

  const handleEditTransaction = (transaction: any) => {
    setEditingTransaction(transaction)
  }

  const handleCancelEdit = () => {
    setEditingTransaction(null)
  }

  return (
    <AuthGuard>
      <DashboardLayout>
        <div className="flex-1 space-y-6 p-6">
          <div>
            <h1 className="text-3xl font-bold text-foreground font-[family-name:var(--font-space-grotesk)]">
              Outward Transactions
            </h1>
            <p className="text-muted-foreground">Record items leaving the warehouse to customers</p>
          </div>

          <div className="grid gap-6 lg:grid-cols-2">
            <DeliveriesForm
              onTransactionAdded={handleTransactionAdded}
              editingTransaction={editingTransaction}
              onCancelEdit={handleCancelEdit}
            />
            <DeliveriesTable refreshTrigger={refreshTrigger} onEditTransaction={handleEditTransaction} />
          </div>
        </div>
      </DashboardLayout>
    </AuthGuard>
  )
}
