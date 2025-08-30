"use client"

import type React from "react"
import { useState, useEffect } from "react"
import { useForm, Controller, type SubmitHandler } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"
import { toast } from "react-hot-toast"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Textarea } from "@/components/ui/textarea"
import { Truck, Loader2 } from "lucide-react"
import { AuthService } from "@/lib/auth"

// Define Zod schemas for data validation
const formSchema = z.object({
  itemId: z.number().int().positive({ message: "Item is required." }),
  customerId: z.number().int().positive({ message: "Customer is required." }),
  godownId: z.number().int().positive({ message: "Source Godown is required." }),
  quantity: z.number().int().min(1, { message: "Quantity must be at least 1." }),
  transactionDate: z.string().min(1, { message: "Transaction Date is required." }),
  notes: z.string().optional(),
})

type OutwardTransactionFormData = z.infer<typeof formSchema>

interface DeliveriesFormProps {
  onTransactionAdded?: () => void
  editingTransaction?: {
    id: number
    itemId: number
    customerId: number
    godownId: number
    quantity: number
    transactionDate: string
    notes?: string
  }
  onCancelEdit?: () => void
}

export function DeliveriesForm({ onTransactionAdded, editingTransaction, onCancelEdit }: DeliveriesFormProps) {
  const [customers, setCustomers] = useState<any[]>([])
  const [godowns, setGodowns] = useState<any[]>([])
  const [items, setItems] = useState<any[]>([])

  const {
    register,
    handleSubmit,
    control,
    formState: { errors, isSubmitting },
    reset,
    setValue,
  } = useForm<OutwardTransactionFormData>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      itemId: 0,
      customerId: 0,
      godownId: 0,
      quantity: 0,
      transactionDate: new Date().toISOString().split("T")[0],
      notes: "",
    },
  })

  // Populate the form fields with the editing transaction's data
  useEffect(() => {
    if (editingTransaction) {
      const formattedDate = new Date(editingTransaction.transactionDate).toISOString().split("T")[0]
      setValue("itemId", editingTransaction.itemId)
      setValue("customerId", editingTransaction.customerId)
      setValue("godownId", editingTransaction.godownId)
      setValue("quantity", editingTransaction.quantity)
      setValue("transactionDate", formattedDate)
      setValue("notes", editingTransaction.notes)
    } else {
      reset() // Reset form when creating a new transaction
    }
  }, [editingTransaction, reset, setValue])

  useEffect(() => {
    const loadDropdownData = async () => {
      try {
        const [customersRes, godownsRes, itemsRes] = await Promise.all([
          fetch("/api/customers", { headers: AuthService.getAuthHeaders() }),
          fetch("/api/godowns", { headers: AuthService.getAuthHeaders() }),
          fetch("/api/items", { headers: AuthService.getAuthHeaders() }),
        ])

        const customersData = await customersRes.json()
        const godownsData = await godownsRes.json()
        const itemsData = await itemsRes.json()

        if (customersData.success) setCustomers(customersData.data || [])
        if (godownsData.success) setGodowns(godownsData.data || [])
        if (itemsData.success) setItems(itemsData.data || [])
      } catch (error) {
        console.error("Failed to load dropdown data:", error)
        toast.error("Failed to load necessary form data.")
      }
    }
    loadDropdownData()
  }, [])

  const onSubmit: SubmitHandler<OutwardTransactionFormData> = async (data) => {
    try {
      const url = editingTransaction ? `/api/outward-transactions/${editingTransaction.id}` : "/api/outward-transactions"
      const method = editingTransaction ? "PUT" : "POST"

      const payload = editingTransaction ? { id: editingTransaction.id, ...data } : data

      const response = await fetch(url, {
        method,
        headers: {
          "Content-Type": "application/json",
          ...AuthService.getAuthHeaders(),
        },
        body: JSON.stringify(payload),
      })

      const result = await response.json()

      if (response.ok && result.success) {
        toast.success(result.message)
        if (!editingTransaction) {
          reset() // Reset form only for new transactions
        }
        onTransactionAdded?.()
        if (editingTransaction && onCancelEdit) {
          setTimeout(() => onCancelEdit(), 1000)
        }
      } else {
        throw new Error(result.message || "Operation failed")
      }
    } catch (error) {
      console.error("API call failed:", error)
      toast.error(error instanceof Error ? error.message : "Network error occurred")
    }
  }

  return (
    <Card>
      <CardHeader>
        <div className="flex items-center space-x-2">
          <Truck className="h-5 w-5 text-accent" />
          <CardTitle className="font-[family-name:var(--font-space-grotesk)]">
            {editingTransaction ? "Edit Outward Transaction" : "New Outward Transaction"}
          </CardTitle>
        </div>
        <CardDescription>
          {editingTransaction ? "Update outward transaction details" : "Record items delivered to customers"}
        </CardDescription>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div className="grid gap-4 md:grid-cols-2">
            <div className="space-y-2">
              <Label htmlFor="transactionDate">Transaction Date *</Label>
              <Input
                id="transactionDate"
                type="date"
                {...register("transactionDate")}
                disabled={isSubmitting}
              />
              {errors.transactionDate && (
                <p className="text-sm font-medium text-destructive">
                  {errors.transactionDate.message}
                </p>
              )}
            </div>
            <div className="space-y-2">
              <Label htmlFor="quantity">Quantity *</Label>
              <Input
                id="quantity"
                type="number"
                placeholder="Enter quantity"
                {...register("quantity", { valueAsNumber: true })}
                disabled={isSubmitting}
                min="1"
              />
              {errors.quantity && (
                <p className="text-sm font-medium text-destructive">
                  {errors.quantity.message}
                </p>
              )}
            </div>
          </div>

          <div className="space-y-2">
            <Label htmlFor="itemId">Item *</Label>
            <Controller
              control={control}
              name="itemId"
              render={({ field }) => (
                <Select
                  onValueChange={(value) => field.onChange(Number.parseInt(value))}
                  value={field.value ? field.value.toString() : "0"}
                  disabled={isSubmitting}
                >
                  <SelectTrigger>
                    <SelectValue placeholder="Select an item" />
                  </SelectTrigger>
                  <SelectContent>
                    {items.map((item) => (
                      <SelectItem key={item.id} value={item.id.toString()}>
                        {item.name} - {item.category}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              )}
            />
            {errors.itemId && (
              <p className="text-sm font-medium text-destructive">
                {errors.itemId.message}
              </p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="customerId">Customer *</Label>
            <Controller
              control={control}
              name="customerId"
              render={({ field }) => (
                <Select
                  onValueChange={(value) => field.onChange(Number.parseInt(value))}
                  value={field.value ? field.value.toString() : "0"}
                  disabled={isSubmitting}
                >
                  <SelectTrigger>
                    <SelectValue placeholder="Select a customer" />
                  </SelectTrigger>
                  <SelectContent>
                    {customers.map((customer) => (
                      <SelectItem key={customer.id} value={customer.id.toString()}>
                        {customer.name}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              )}
            />
            {errors.customerId && (
              <p className="text-sm font-medium text-destructive">
                {errors.customerId.message}
              </p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="godownId">Source Godown *</Label>
            <Controller
              control={control}
              name="godownId"
              render={({ field }) => (
                <Select
                  onValueChange={(value) => field.onChange(Number.parseInt(value))}
                  value={field.value ? field.value.toString() : "0"}
                  disabled={isSubmitting}
                >
                  <SelectTrigger>
                    <SelectValue placeholder="Select source godown" />
                  </SelectTrigger>
                  <SelectContent>
                    {godowns.map((godown) => (
                      <SelectItem key={godown.id} value={godown.id.toString()}>
                        {godown.name}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              )}
            />
            {errors.godownId && (
              <p className="text-sm font-medium text-destructive">
                {errors.godownId.message}
              </p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="notes">Notes</Label>
            <Textarea
              id="notes"
              placeholder="Additional notes about this transaction"
              {...register("notes")}
              rows={3}
              disabled={isSubmitting}
            />
          </div>

          <div className="flex gap-2">
            <Button type="submit" className="flex-1 bg-accent hover:bg-accent/90" disabled={isSubmitting}>
              {isSubmitting ? (
                <>
                  <Loader2 className="mr-2 h-4 w-4 animate-spin" /> Processing...
                </>
              ) : (
                editingTransaction ? "Update Transaction" : "Record Transaction"
              )}
            </Button>
            {editingTransaction && onCancelEdit && (
              <Button type="button" variant="outline" onClick={onCancelEdit} disabled={isSubmitting}>
                Cancel
              </Button>
            )}
          </div>
        </form>
      </CardContent>
    </Card>
  )
}