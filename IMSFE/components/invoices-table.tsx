"use client"

import { useState, useEffect } from "react"
import Link from "next/link"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert"
import { Eye, Download, Loader2, AlertCircle } from "lucide-react"
import { AuthService } from "@/lib/auth"

interface Invoice {
  id: string
  date: string
  customer: string
  totalAmount: number
  status: "paid" | "pending" | "overdue"
  dueDate: string
}

export function InvoicesTable() {
  const [invoices, setInvoices] = useState<Invoice[]>([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState("")

  const fetchInvoices = async () => {
    try {
      setIsLoading(true)
      const response = await fetch("/api/invoices", {
        headers: AuthService.getAuthHeaders(),
      })

      const result = await response.json()

      if (response.ok && result.success) {
        setInvoices(result.data || [])
      } else {
        throw new Error(result.message || "Failed to fetch invoices")
      }
    } catch (err) {
      console.error("Error fetching invoices:", err)
      setError("Network error occurred. Please try again.")
    } finally {
      setIsLoading(false)
    }
  }

  useEffect(() => {
    fetchInvoices()
  }, [])

  const getStatusColor = (status: string) => {
    switch (status) {
      case "paid":
        return "bg-green-100 text-green-800"
      case "pending":
        return "bg-yellow-100 text-yellow-800"
      case "overdue":
        return "bg-red-100 text-red-800"
      default:
        return "bg-gray-100 text-gray-800"
    }
  }

  const renderContent = () => {
    if (isLoading) {
      return (
        <CardContent className="p-6">
          <div className="text-center py-10">
            <Loader2 className="h-8 w-8 animate-spin text-accent mx-auto mb-4" />
            <p className="text-muted-foreground">Loading invoices...</p>
          </div>
        </CardContent>
      )
    }

    if (error) {
      return (
        <CardContent className="p-6">
          <Alert variant="destructive">
            <AlertCircle className="h-4 w-4" />
            <AlertTitle>Error</AlertTitle>
            <AlertDescription>{error}</AlertDescription>
          </Alert>
        </CardContent>
      )
    }

    if (invoices.length === 0) {
      return (
        <CardContent className="p-6">
          <div className="text-center py-8">
            <p className="text-muted-foreground">No invoices found. Add your first invoice to get started.</p>
          </div>
        </CardContent>
      )
    }

    return (
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Invoice ID</TableHead>
              <TableHead>Date</TableHead>
              <TableHead>Customer</TableHead>
              <TableHead>Total Amount</TableHead>
              <TableHead>Due Date</TableHead>
              <TableHead>Status</TableHead>
              <TableHead>Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {invoices.map((invoice) => (
              <TableRow key={invoice.id}>
                <TableCell className="font-medium">{invoice.id}</TableCell>
                <TableCell>{invoice.date}</TableCell>
                <TableCell>{invoice.customer}</TableCell>
                <TableCell>${invoice.totalAmount.toFixed(2)}</TableCell>
                <TableCell>{invoice.dueDate}</TableCell>
                <TableCell>
                  <Badge className={getStatusColor(invoice.status)} variant="secondary">
                    {invoice.status}
                  </Badge>
                </TableCell>
                <TableCell>
                  <div className="flex items-center space-x-2">
                    <Link href={`/dashboard/invoices/${invoice.id}`}>
                      <Button variant="ghost" size="sm">
                        <Eye className="h-4 w-4" />
                      </Button>
                    </Link>
                    <Button variant="ghost" size="sm">
                      <Download className="h-4 w-4" />
                    </Button>
                  </div>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </CardContent>
    )
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle className="font-[family-name:var(--font-space-grotesk)]">All Invoices</CardTitle>
        <CardDescription>Complete list of system invoices</CardDescription>
      </CardHeader>
      {renderContent()}
    </Card>
  )
}