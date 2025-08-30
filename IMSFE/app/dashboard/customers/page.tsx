"use client"

import { useState } from "react"
import { DashboardLayout } from "@/components/dashboard-layout"
import { CustomersForm } from "@/components/customers-form"
import { CustomersTable } from "@/components/customers-table"
import { CustomersStats } from "@/components/customers-stats"
import { AuthGuard } from "@/components/auth-guard"

export default function CustomersPage() {
  const [refreshTrigger, setRefreshTrigger] = useState(0)
  const [editingCustomer, setEditingCustomer] = useState<any>(null)

  const handleCustomerAdded = () => {
    setRefreshTrigger((prev) => prev + 1)
  }

  const handleEditCustomer = (customer: any) => {
    setEditingCustomer(customer)
  }

  const handleCancelEdit = () => {
    setEditingCustomer(null)
  }

  return (
    <AuthGuard requiredRole="Manager">
      <DashboardLayout>
        <div className="flex-1 space-y-6 p-6">
          <div>
            <h1 className="text-3xl font-bold text-foreground font-[family-name:var(--font-space-grotesk)]">
              Customer Management
            </h1>
            <p className="text-muted-foreground">Manage customer information and relationships</p>
          </div>

          <CustomersStats refreshTrigger={refreshTrigger} />

          <div className="grid gap-6 lg:grid-cols-3">
            <div className="lg:col-span-2">
              <CustomersTable refreshTrigger={refreshTrigger} onEditCustomer={handleEditCustomer} />
            </div>
            <div>
              <CustomersForm
                onCustomerAdded={handleCustomerAdded}
                editingCustomer={editingCustomer}
                onCancelEdit={handleCancelEdit}
              />
            </div>
          </div>
        </div>
      </DashboardLayout>
    </AuthGuard>
  )
}
