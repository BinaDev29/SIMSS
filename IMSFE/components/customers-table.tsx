"use client"

import { useState, useEffect } from "react"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Alert, AlertDescription } from "@/components/ui/alert"
import { Edit, Trash2, Mail, Phone, AlertCircle } from "lucide-react"
import { AuthService } from "@/lib/auth"

interface Customer {
  id: number
  name: string
  contactPerson: string
  phoneNumber: string
  email: string
}

interface CustomersTableProps {
  refreshTrigger?: number
  onEditCustomer?: (customer: Customer) => void
}

export function CustomersTable({ refreshTrigger, onEditCustomer }: CustomersTableProps) {
  const [customers, setCustomers] = useState<Customer[]>([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState("")

  const fetchCustomers = async () => {
    try {
      setIsLoading(true)
      const response = await fetch("/api/customers", {
        headers: {
          ...AuthService.getAuthHeaders(),
        },
      })

      const result = await response.json()

      if (result.success) {
        setCustomers(result.data || [])
      } else {
        setError(result.message || "Failed to fetch customers")
      }
    } catch (error) {
      setError("Network error occurred")
    } finally {
      setIsLoading(false)
    }
  }

  const handleDeleteCustomer = async (customerId: number) => {
    if (!confirm("Are you sure you want to delete this customer?")) {
      return
    }

    try {
      const response = await fetch(`/api/customers/${customerId}`, {
        method: "DELETE",
        headers: {
          ...AuthService.getAuthHeaders(),
        },
      })

      const result = await response.json()

      if (result.success) {
        setCustomers(customers.filter((c) => c.id !== customerId))
      } else {
        setError(result.message || "Failed to delete customer")
      }
    } catch (error) {
      setError("Network error occurred")
    }
  }

  useEffect(() => {
    fetchCustomers()
  }, [refreshTrigger])

  if (isLoading) {
    return (
      <Card>
        <CardContent className="p-6">
          <div className="text-center">
            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-accent mx-auto mb-4"></div>
            <p className="text-muted-foreground">Loading customers...</p>
          </div>
        </CardContent>
      </Card>
    )
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle className="font-[family-name:var(--font-space-grotesk)]">All Customers</CardTitle>
        <CardDescription>Complete customer database ({customers.length} customers)</CardDescription>
      </CardHeader>
      <CardContent>
        {error && (
          <Alert variant="destructive" className="mb-4">
            <AlertCircle className="h-4 w-4" />
            <AlertDescription>{error}</AlertDescription>
          </Alert>
        )}

        {customers.length === 0 ? (
          <div className="text-center py-8">
            <p className="text-muted-foreground">No customers found. Add your first customer to get started.</p>
          </div>
        ) : (
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>ID</TableHead>
                <TableHead>Company Name</TableHead>
                <TableHead>Contact Person</TableHead>
                <TableHead>Contact Info</TableHead>
                <TableHead>Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {customers.map((customer) => (
                <TableRow key={customer.id}>
                  <TableCell className="font-medium">#{customer.id}</TableCell>
                  <TableCell>
                    <div className="font-medium">{customer.name}</div>
                  </TableCell>
                  <TableCell>
                    <div className="font-medium">{customer.contactPerson}</div>
                  </TableCell>
                  <TableCell>
                    <div className="space-y-1">
                      <div className="text-sm flex items-center space-x-2">
                        <Mail className="h-3 w-3 text-muted-foreground" />
                        <span>{customer.email}</span>
                      </div>
                      <div className="text-sm flex items-center space-x-2">
                        <Phone className="h-3 w-3 text-muted-foreground" />
                        <span>{customer.phoneNumber}</span>
                      </div>
                    </div>
                  </TableCell>
                  <TableCell>
                    <div className="flex items-center space-x-2">
                      <Button variant="ghost" size="sm" onClick={() => onEditCustomer?.(customer)}>
                        <Edit className="h-4 w-4" />
                      </Button>
                      <Button variant="ghost" size="sm" onClick={() => handleDeleteCustomer(customer.id)}>
                        <Trash2 className="h-4 w-4" />
                      </Button>
                    </div>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        )}
      </CardContent>
    </Card>
  )
}
