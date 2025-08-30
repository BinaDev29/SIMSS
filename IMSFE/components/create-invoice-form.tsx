"use client"

import type React from "react"

import { useEffect, useState } from "react"
import { useForm, type SubmitHandler } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"
import { toast } from "react-hot-toast"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Textarea } from "@/components/ui/textarea"
import { Plus, FileText, Loader2 } from "lucide-react"

// Define the shape of your form data using Zod for validation
const formSchema = z.object({
  customerId: z.string({
    required_error: "Please select a customer.",
  }),
  invoiceDate: z.string().min(1, { message: "Invoice date is required." }),
  dueDate: z.string().min(1, { message: "Due date is required." }),
  notes: z.string().optional(),
})

type FormData = z.infer<typeof formSchema>

// Define the shape of a Customer
interface Customer {
  id: number
  name: string
}

export function CreateInvoiceForm() {
  const [customers, setCustomers] = useState<Customer[]>([])
  const [isLoadingCustomers, setIsLoadingCustomers] = useState(true)

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
  } = useForm<FormData>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      customerId: "",
      invoiceDate: "",
      dueDate: "",
      notes: "",
    },
  })

  // Fetch customers from your Next.js API route
  useEffect(() => {
    const fetchCustomers = async () => {
      try {
        const response = await fetch("/api/customers")
        if (!response.ok) {
          throw new Error("Failed to fetch customers")
        }
        const result = await response.json()
        setCustomers(result.data)
      } catch (error) {
        console.error("Error fetching customers:", error)
        toast.error("Failed to load customers.")
      } finally {
        setIsLoadingCustomers(false)
      }
    }

    fetchCustomers()
  }, [])

  const onSubmit: SubmitHandler<FormData> = async (data) => {
    console.log("Submitting form data:", data)
    try {
      const response = await fetch("/api/invoices", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          ...data,
          customerId: Number.parseInt(data.customerId),
        }),
      })

      if (!response.ok) {
        throw new Error("Failed to create invoice")
      }

      const result = await response.json()
      console.log("Invoice created successfully:", result)
      toast.success("Invoice created successfully!")
      reset() // Reset form after successful submission
    } catch (error) {
      console.error("Failed to create invoice:", error)
      toast.error("Failed to create invoice. Please try again.")
    }
  }

  return (
    <Card>
      <CardHeader>
        <div className="flex items-center space-x-2">
          <FileText className="h-5 w-5 text-accent" />
          <CardTitle className="font-[family-name:var(--font-space-grotesk)]">Create New Invoice</CardTitle>
        </div>
        <CardDescription>Generate a new invoice for a customer</CardDescription>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="customerId">Customer</Label>
            <Select
              onValueChange={(value) => setValue("customerId", value)}
              {...register("customerId")}
            >
              <SelectTrigger>
                <SelectValue placeholder={isLoadingCustomers ? "Loading customers..." : "Select customer"} />
              </SelectTrigger>
              <SelectContent>
                {customers.map((customer) => (
                  <SelectItem key={customer.id} value={customer.id.toString()}>
                    {customer.name}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
            {errors.customerId && (
              <p className="text-sm font-medium text-destructive">
                {errors.customerId.message}
              </p>
            )}
          </div>

          <div className="grid gap-4 md:grid-cols-2">
            <div className="space-y-2">
              <Label htmlFor="invoiceDate">Invoice Date</Label>
              <Input
                id="invoiceDate"
                type="date"
                {...register("invoiceDate")}
              />
              {errors.invoiceDate && (
                <p className="text-sm font-medium text-destructive">
                  {errors.invoiceDate.message}
                </p>
              )}
            </div>
            <div className="space-y-2">
              <Label htmlFor="dueDate">Due Date</Label>
              <Input
                id="dueDate"
                type="date"
                {...register("dueDate")}
              />
              {errors.dueDate && (
                <p className="text-sm font-medium text-destructive">
                  {errors.dueDate.message}
                </p>
              )}
            </div>
          </div>

          <div className="space-y-2">
            <Label htmlFor="notes">Notes (Optional)</Label>
            <Textarea
              id="notes"
              placeholder="Additional notes for this invoice"
              {...register("notes")}
              rows={3}
            />
          </div>

          <Button type="submit" className="w-full bg-accent hover:bg-accent/90" disabled={isSubmitting}>
            {isSubmitting ? (
              <>
                <Loader2 className="mr-2 h-4 w-4 animate-spin" /> Creating...
              </>
            ) : (
              <>
                <Plus className="mr-2 h-4 w-4" /> Create Invoice
              </>
            )}
          </Button>
        </form>
      </CardContent>
    </Card>
  )
}