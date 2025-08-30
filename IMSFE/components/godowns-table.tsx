"use client"

import { useState, useEffect } from "react"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Alert, AlertDescription } from "@/components/ui/alert"
import { Edit, Trash2, MapPin, AlertCircle } from "lucide-react"
import { AuthService } from "@/lib/auth"

interface Godown {
  id: number
  name: string
  location: string
  capacity: number
  description?: string
}

interface GodownsTableProps {
  refreshTrigger?: number
  onEditGodown?: (godown: Godown) => void
}

export function GodownsTable({ refreshTrigger, onEditGodown }: GodownsTableProps) {
  const [godowns, setGodowns] = useState<Godown[]>([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState("")

  const fetchGodowns = async () => {
    try {
      setIsLoading(true)
      const response = await fetch("/api/godowns", {
        headers: {
          ...AuthService.getAuthHeaders(),
        },
      })

      const result = await response.json()

      if (result.success) {
        setGodowns(result.data || [])
      } else {
        setError(result.message || "Failed to fetch godowns")
      }
    } catch (error) {
      setError("Network error occurred")
    } finally {
      setIsLoading(false)
    }
  }

  const handleDeleteGodown = async (godownId: number) => {
    if (!confirm("Are you sure you want to delete this godown?")) {
      return
    }

    try {
      const response = await fetch(`/api/godowns/${godownId}`, {
        method: "DELETE",
        headers: {
          ...AuthService.getAuthHeaders(),
        },
      })

      const result = await response.json()

      if (result.success) {
        setGodowns(godowns.filter((g) => g.id !== godownId))
      } else {
        setError(result.message || "Failed to delete godown")
      }
    } catch (error) {
      setError("Network error occurred")
    }
  }

  useEffect(() => {
    fetchGodowns()
  }, [refreshTrigger])

  if (isLoading) {
    return (
      <Card>
        <CardContent className="p-6">
          <div className="text-center">
            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-accent mx-auto mb-4"></div>
            <p className="text-muted-foreground">Loading godowns...</p>
          </div>
        </CardContent>
      </Card>
    )
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle className="font-[family-name:var(--font-space-grotesk)]">All Godowns</CardTitle>
        <CardDescription>Complete warehouse and storage location database ({godowns.length} godowns)</CardDescription>
      </CardHeader>
      <CardContent>
        {error && (
          <Alert variant="destructive" className="mb-4">
            <AlertCircle className="h-4 w-4" />
            <AlertDescription>{error}</AlertDescription>
          </Alert>
        )}

        {godowns.length === 0 ? (
          <div className="text-center py-8">
            <p className="text-muted-foreground">No godowns found. Add your first godown to get started.</p>
          </div>
        ) : (
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>ID</TableHead>
                <TableHead>Name & Location</TableHead>
                <TableHead>Capacity</TableHead>
                <TableHead>Description</TableHead>
                <TableHead>Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {godowns.map((godown) => (
                <TableRow key={godown.id}>
                  <TableCell className="font-medium">#{godown.id}</TableCell>
                  <TableCell>
                    <div>
                      <div className="font-medium">{godown.name}</div>
                      <div className="text-sm text-muted-foreground flex items-center space-x-1">
                        <MapPin className="h-3 w-3" />
                        <span>{godown.location}</span>
                      </div>
                    </div>
                  </TableCell>
                  <TableCell>{godown.capacity.toLocaleString()} m³</TableCell>
                  <TableCell>
                    <div className="text-sm text-muted-foreground max-w-xs truncate">
                      {godown.description || "No description"}
                    </div>
                  </TableCell>
                  <TableCell>
                    <div className="flex items-center space-x-2">
                      <Button variant="ghost" size="sm" onClick={() => onEditGodown?.(godown)}>
                        <Edit className="h-4 w-4" />
                      </Button>
                      <Button variant="ghost" size="sm" onClick={() => handleDeleteGodown(godown.id)}>
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
