"use client"

import React, { useEffect } from "react"
import { useForm, type SubmitHandler } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Alert, AlertDescription } from "@/components/ui/alert"
import { Textarea } from "@/components/ui/textarea"
import { Plus, Warehouse, AlertCircle, Loader2 } from "lucide-react"
import { AuthService } from "@/lib/auth"
import { useToast } from "@/components/ui/use-toast"

// የመረጃ ማረጋገጫ ፎርም (Schema)
const GodownFormSchema = z.object({
  name: z.string().min(2, "Name must be at least 2 characters."),
  location: z.string().min(5, "Location must be a complete address."),
  capacity: z.coerce.number().min(1, "Capacity must be a positive number."),
  description: z.string().optional(),
})

type GodownFormData = z.infer<typeof GodownFormSchema>

interface GodownsFormProps {
  onGodownAdded?: () => void
  editingGodown?: GodownFormData & { id: string | number }
  onCancelEdit?: () => void
}

export function GodownsForm({ onGodownAdded, editingGodown, onCancelEdit }: GodownsFormProps) {
  const { toast } = useToast()
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting },
  } = useForm<GodownFormData>({
    resolver: zodResolver(GodownFormSchema),
    defaultValues: {
      name: "",
      location: "",
      capacity: 0,
      description: "",
    },
  })

  useEffect(() => {
    if (editingGodown) {
      reset({
        name: editingGodown.name,
        location: editingGodown.location,
        capacity: editingGodown.capacity,
        description: editingGodown.description,
      })
    } else {
      reset() // Reset form when not in edit mode
    }
  }, [editingGodown, reset])

  const onSubmit: SubmitHandler<GodownFormData> = async (data) => {
    try {
      const url = editingGodown ? `/api/godowns/${editingGodown.id}` : "/api/godowns"
      const method = editingGodown ? "PUT" : "POST"

      const response = await fetch(url, {
        method,
        headers: {
          "Content-Type": "application/json",
          ...AuthService.getAuthHeaders(),
        },
        body: JSON.stringify(data),
      })

      const result = await response.json()

      if (response.ok && result.success) {
        toast({
          title: "Success",
          description: result.message,
          variant: "default",
        })
        if (!editingGodown) {
          reset() // Reset form only for new godowns
        }
        onGodownAdded?.()
        if (editingGodown && onCancelEdit) {
          setTimeout(() => onCancelEdit(), 1000)
        }
      } else {
        throw new Error(result.message || "Operation failed.")
      }
    } catch (error) {
      toast({
        title: "Error",
        description: error instanceof Error ? error.message : "An unknown error occurred.",
        variant: "destructive",
      })
    }
  }

  return (
    <Card>
      <CardHeader>
        <div className="flex items-center space-x-2">
          <Warehouse className="h-5 w-5 text-accent" />
          <CardTitle className="font-[family-name:var(--font-space-grotesk)]">
            {editingGodown ? "Edit Godown" : "Add New Godown"}
          </CardTitle>
        </div>
        <CardDescription>
          {editingGodown ? "Update godown information" : "Register a new warehouse or storage location"}
        </CardDescription>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="name">Godown Name *</Label>
            <Input
              id="name"
              placeholder="Enter godown name"
              {...register("name")}
              disabled={isSubmitting}
            />
            {errors.name && <Alert variant="destructive"><AlertDescription>{errors.name.message}</AlertDescription></Alert>}
          </div>

          <div className="space-y-2">
            <Label htmlFor="location">Location *</Label>
            <Textarea
              id="location"
              placeholder="Enter complete address"
              {...register("location")}
              rows={2}
              disabled={isSubmitting}
            />
            {errors.location && <Alert variant="destructive"><AlertDescription>{errors.location.message}</AlertDescription></Alert>}
          </div>

          <div className="space-y-2">
            <Label htmlFor="capacity">Capacity (m³) *</Label>
            <Input
              id="capacity"
              type="number"
              placeholder="Enter capacity in cubic meters"
              {...register("capacity", { valueAsNumber: true })}
              disabled={isSubmitting}
              min="1"
            />
            {errors.capacity && <Alert variant="destructive"><AlertDescription>{errors.capacity.message}</AlertDescription></Alert>}
          </div>

          <div className="space-y-2">
            <Label htmlFor="description">Description</Label>
            <Textarea
              id="description"
              placeholder="Additional details about the godown"
              {...register("description")}
              rows={3}
              disabled={isSubmitting}
            />
          </div>

          <div className="flex gap-2">
            <Button type="submit" className="flex-1 bg-accent hover:bg-accent/90" disabled={isSubmitting}>
              {isSubmitting ? (
                <><Loader2 className="mr-2 h-4 w-4 animate-spin" /> Processing...</>
              ) : (
                <><Plus className="mr-2 h-4 w-4" /> {editingGodown ? "Update Godown" : "Add Godown"}</>
              )}
            </Button>
            {editingGodown && onCancelEdit && (
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