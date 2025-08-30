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
import { UserCheck, Loader2, Save } from "lucide-react"
import { AuthService } from "@/lib/auth"

// Define Zod schema for validation
const employeeSchema = z.object({
  id: z.number(),
  firstName: z.string().min(1, { message: "First Name is required." }),
  lastName: z.string().min(1, { message: "Last Name is required." }),
  email: z.string().email({ message: "Invalid email address." }),
  phone: z.string().min(1, { message: "Phone number is required." }),
  position: z.string().min(1, { message: "Position is required." }),
  department: z.string().min(1, { message: "Department is required." }),
  hireDate: z.string().min(1, { message: "Hire Date is required." }),
  salary: z.coerce.number().min(0, { message: "Salary must be a positive number." }),
  address: z.string().min(1, { message: "Address is required." }),
  emergencyContact: z.string().min(1, { message: "Emergency Contact is required." }),
})

type EmployeeFormData = z.infer<typeof employeeSchema>

interface EmployeesEditFormProps {
  employeeId: number
  onSuccess: () => void
}

export function EmployeesEditForm({ employeeId, onSuccess }: EmployeesEditFormProps) {
  const [positions, setPositions] = useState<any[]>([])
  const [departments, setDepartments] = useState<any[]>([])
  const [isDataLoading, setIsDataLoading] = useState(true)

  const {
    register,
    handleSubmit,
    control,
    formState: { errors, isSubmitting },
    reset,
  } = useForm<EmployeeFormData>({
    resolver: zodResolver(employeeSchema),
  })

  useEffect(() => {
    const loadFormData = async () => {
      try {
        const [employeeRes, positionsRes, departmentsRes] = await Promise.all([
          fetch(`/api/employees/${employeeId}`, { headers: AuthService.getAuthHeaders() }),
          fetch("/api/positions", { headers: AuthService.getAuthHeaders() }),
          fetch("/api/departments", { headers: AuthService.getAuthHeaders() }),
        ])

        const employeeData = await employeeRes.json()
        const positionsData = await positionsRes.json()
        const departmentsData = await departmentsRes.json()

        if (!employeeData.success) throw new Error(employeeData.message)
        if (positionsData.success) setPositions(positionsData.data || [])
        if (departmentsData.success) setDepartments(departmentsData.data || [])

        // Pre-populate the form with the fetched employee data
        reset({
          ...employeeData.data,
          hireDate: new Date(employeeData.data.hireDate).toISOString().split("T")[0],
        })
      } catch (error) {
        console.error("Failed to load form data:", error)
        toast.error("Failed to load employee data for editing.")
      } finally {
        setIsDataLoading(false)
      }
    }

    if (employeeId) {
      loadFormData()
    }
  }, [employeeId, reset])

  const onSubmit: SubmitHandler<EmployeeFormData> = async (data) => {
    const toastId = toast.loading("Updating employee...")

    try {
      const response = await fetch(`/api/employees/${data.id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          ...AuthService.getAuthHeaders(),
        },
        body: JSON.stringify(data),
      })

      const result = await response.json()

      if (response.ok && result.success) {
        toast.success(result.message, { id: toastId })
        onSuccess()
      } else {
        throw new Error(result.message || "Operation failed")
      }
    } catch (error) {
      console.error("API call failed:", error)
      toast.error(error instanceof Error ? error.message : "Network error occurred", { id: toastId })
    }
  }

  return (
    <Card>
      <CardHeader>
        <div className="flex items-center space-x-2">
          <UserCheck className="h-5 w-5 text-accent" />
          <CardTitle className="font-[family-name:var(--font-space-grotesk)]">Edit Employee</CardTitle>
        </div>
        <CardDescription>Update employee details</CardDescription>
      </CardHeader>
      <CardContent>
        {isDataLoading ? (
          <div className="text-center py-8">
            <Loader2 className="h-8 w-8 animate-spin text-accent mx-auto mb-4" />
            <p className="text-muted-foreground">Loading employee data...</p>
          </div>
        ) : (
          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <div className="grid gap-4 md:grid-cols-2">
              <div className="space-y-2">
                <Label htmlFor="firstName">First Name</Label>
                <Input id="firstName" placeholder="Enter first name" {...register("firstName")} disabled={isSubmitting} />
                {errors.firstName && <p className="text-sm font-medium text-destructive">{errors.firstName.message}</p>}
              </div>
              <div className="space-y-2">
                <Label htmlFor="lastName">Last Name</Label>
                <Input id="lastName" placeholder="Enter last name" {...register("lastName")} disabled={isSubmitting} />
                {errors.lastName && <p className="text-sm font-medium text-destructive">{errors.lastName.message}</p>}
              </div>
            </div>

            <div className="grid gap-4 md:grid-cols-2">
              <div className="space-y-2">
                <Label htmlFor="email">Email</Label>
                <Input
                  id="email"
                  type="email"
                  placeholder="Enter email address"
                  {...register("email")}
                  disabled={isSubmitting}
                />
                {errors.email && <p className="text-sm font-medium text-destructive">{errors.email.message}</p>}
              </div>
              <div className="space-y-2">
                <Label htmlFor="phone">Phone</Label>
                <Input
                  id="phone"
                  type="tel"
                  placeholder="Enter phone number"
                  {...register("phone")}
                  disabled={isSubmitting}
                />
                {errors.phone && <p className="text-sm font-medium text-destructive">{errors.phone.message}</p>}
              </div>
            </div>

            <div className="grid gap-4 md:grid-cols-2">
              <div className="space-y-2">
                <Label htmlFor="position">Position</Label>
                <Controller
                  control={control}
                  name="position"
                  render={({ field }) => (
                    <Select
                      onValueChange={field.onChange}
                      value={field.value}
                      disabled={isSubmitting || isDataLoading}
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Select position" />
                      </SelectTrigger>
                      <SelectContent>
                        {positions.map((pos) => (
                          <SelectItem key={pos.id} value={pos.name}>
                            {pos.name}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  )}
                />
                {errors.position && <p className="text-sm font-medium text-destructive">{errors.position.message}</p>}
              </div>
              <div className="space-y-2">
                <Label htmlFor="department">Department</Label>
                <Controller
                  control={control}
                  name="department"
                  render={({ field }) => (
                    <Select
                      onValueChange={field.onChange}
                      value={field.value}
                      disabled={isSubmitting || isDataLoading}
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Select department" />
                      </SelectTrigger>
                      <SelectContent>
                        {departments.map((dep) => (
                          <SelectItem key={dep.id} value={dep.name}>
                            {dep.name}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  )}
                />
                {errors.department && <p className="text-sm font-medium text-destructive">{errors.department.message}</p>}
              </div>
            </div>

            <div className="grid gap-4 md:grid-cols-2">
              <div className="space-y-2">
                <Label htmlFor="hireDate">Hire Date</Label>
                <Input
                  id="hireDate"
                  type="date"
                  {...register("hireDate")}
                  disabled={isSubmitting}
                />
                {errors.hireDate && <p className="text-sm font-medium text-destructive">{errors.hireDate.message}</p>}
              </div>
              <div className="space-y-2">
                <Label htmlFor="salary">Salary</Label>
                <Input
                  id="salary"
                  type="number"
                  step="0.01"
                  placeholder="Enter salary"
                  {...register("salary", { valueAsNumber: true })}
                  disabled={isSubmitting}
                />
                {errors.salary && <p className="text-sm font-medium text-destructive">{errors.salary.message}</p>}
              </div>
            </div>

            <div className="space-y-2">
              <Label htmlFor="address">Address</Label>
              <Textarea
                id="address"
                placeholder="Enter complete address"
                {...register("address")}
                rows={2}
                disabled={isSubmitting}
              />
              {errors.address && <p className="text-sm font-medium text-destructive">{errors.address.message}</p>}
            </div>

            <div className="space-y-2">
              <Label htmlFor="emergencyContact">Emergency Contact</Label>
              <Input
                id="emergencyContact"
                placeholder="Enter emergency contact details"
                {...register("emergencyContact")}
                disabled={isSubmitting}
              />
              {errors.emergencyContact && <p className="text-sm font-medium text-destructive">{errors.emergencyContact.message}</p>}
            </div>

            <Button type="submit" className="w-full bg-accent hover:bg-accent/90" disabled={isSubmitting}>
              {isSubmitting ? (
                <>
                  <Loader2 className="mr-2 h-4 w-4 animate-spin" /> Saving...
                </>
              ) : (
                <>
                  <Save className="mr-2 h-4 w-4" />
                  Save Changes
                </>
              )}
            </Button>
          </form>
        )}
      </CardContent>
    </Card>
  )
}