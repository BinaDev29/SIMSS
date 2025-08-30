"use client"

import type React from "react"
import { useEffect } from "react"
import { useForm, type SubmitHandler } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"
import { toast } from "react-hot-toast"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Plus, Users, Loader2 } from "lucide-react"
import { AuthService } from "@/lib/auth"

// Define the shape and validation rules for your form data using Zod
const formSchema = z.object({
  name: z.string().min(1, { message: "Company name is required." }),
  contactPerson: z.string().min(1, { message: "Contact person is required." }),
  phoneNumber: z.string().min(1, { message: "Phone number is required." }),
  email: z.string().min(1, { message: "Email is required." }).email({ message: "Invalid email address." }),
})

type FormData = z.infer<typeof formSchema>

interface CustomersFormProps {
  onCustomerAdded?: () => void
  editingCustomer?: {
    id: number
    name: string
    contactPerson: string
    phoneNumber: string
    email: string
  }
  onCancelEdit?: () => void
}

export function CustomersForm({ onCustomerAdded, editingCustomer, onCancelEdit }: CustomersFormProps) {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
  } = useForm<FormData>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      name: "",
      contactPerson: "",
      phoneNumber: "",
      email: "",
    },
  })

  // Populate the form fields with the editing customer's data
  useEffect(() => {
    if (editingCustomer) {
      setValue("name", editingCustomer.name)
      setValue("contactPerson", editingCustomer.contactPerson)
      setValue("phoneNumber", editingCustomer.phoneNumber)
      setValue("email", editingCustomer.email)
    } else {
      reset() // Reset form when creating a new customer
    }
  }, [editingCustomer, reset, setValue])

  const onSubmit: SubmitHandler<FormData> = async (data) => {
    try {
      const url = editingCustomer ? `/api/customers/${editingCustomer.id}` : "/api/customers";
      const method = editingCustomer ? "PUT" : "POST";

      const payload = editingCustomer ? { id: editingCustomer.id, ...data } : data;

      const response = await fetch(url, {
        method,
        headers: {
          "Content-Type": "application/json",
          ...AuthService.getAuthHeaders(),
        },
        body: JSON.stringify(payload),
      });

      const result = await response.json();

      if (response.ok && result.success) {
        toast.success(result.message);
        if (!editingCustomer) {
          reset(); // Reset form for new customer creation
        }
        onCustomerAdded?.();
        if (editingCustomer && onCancelEdit) {
          setTimeout(() => onCancelEdit(), 1000);
        }
      } else {
        throw new Error(result.message || "Operation failed.");
      }
    } catch (error) {
      console.error("API call failed:", error);
      toast.error(error instanceof Error ? error.message : "Network error occurred.");
    }
  };

  return (
    <Card>
      <CardHeader>
        <div className="flex items-center space-x-2">
          <Users className="h-5 w-5 text-accent" />
          <CardTitle className="font-[family-name:var(--font-space-grotesk)]">
            {editingCustomer ? "Edit Customer" : "Add New Customer"}
          </CardTitle>
        </div>
        <CardDescription>{editingCustomer ? "Update customer information" : "Register a new customer"}</CardDescription>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="name">Company Name *</Label>
            <Input
              id="name"
              placeholder="Enter company name"
              {...register("name")}
              disabled={isSubmitting}
            />
            {errors.name && (
              <p className="text-sm font-medium text-destructive">
                {errors.name.message}
              </p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="contactPerson">Contact Person *</Label>
            <Input
              id="contactPerson"
              placeholder="Enter contact person name"
              {...register("contactPerson")}
              disabled={isSubmitting}
            />
            {errors.contactPerson && (
              <p className="text-sm font-medium text-destructive">
                {errors.contactPerson.message}
              </p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="phoneNumber">Phone Number *</Label>
            <Input
              id="phoneNumber"
              type="tel"
              placeholder="Enter phone number"
              {...register("phoneNumber")}
              disabled={isSubmitting}
            />
            {errors.phoneNumber && (
              <p className="text-sm font-medium text-destructive">
                {errors.phoneNumber.message}
              </p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="email">Email Address *</Label>
            <Input
              id="email"
              type="email"
              placeholder="Enter email address"
              {...register("email")}
              disabled={isSubmitting}
            />
            {errors.email && (
              <p className="text-sm font-medium text-destructive">
                {errors.email.message}
              </p>
            )}
          </div>

          <div className="flex gap-2">
            <Button type="submit" className="flex-1 bg-accent hover:bg-accent/90" disabled={isSubmitting}>
              {isSubmitting ? (
                <>
                  <Loader2 className="mr-2 h-4 w-4 animate-spin" /> Processing...
                </>
              ) : (
                <>
                  <Plus className="mr-2 h-4 w-4" /> {editingCustomer ? "Update Customer" : "Add Customer"}
                </>
              )}
            </Button>
            {editingCustomer && onCancelEdit && (
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