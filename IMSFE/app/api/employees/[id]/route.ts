import { NextResponse } from "next/server"
import { AuthService } from "@/lib/auth"

const BACKEND_URL = `${process.env.NEXT_PUBLIC_BACKEND_URL}/api/Employee`

export async function GET(request: Request, { params }: { params: { id: string } }) {
  const { id } = params
  try {
    const response = await fetch(`${BACKEND_URL}/${id}`, {
      headers: AuthService.getAuthHeaders(),
      cache: "no-store",
    })

    if (!response.ok) {
      const errorData = await response.json()
      throw new Error(errorData.message || "Failed to fetch employee details.")
    }

    const data = await response.json()
    return NextResponse.json({ success: true, data: data, message: "Employee details retrieved successfully." })
  } catch (error) {
    console.error(`Error fetching employee with ID ${id}:`, error)
    return NextResponse.json(
      { success: false, message: error instanceof Error ? error.message : "Failed to fetch employee details." },
      { status: 500 }
    )
  }
}

export async function PUT(request: Request, { params }: { params: { id: string } }) {
  const { id } = params
  try {
    const payload = await request.json()

    const response = await fetch(`${BACKEND_URL}/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        ...AuthService.getAuthHeaders(),
      },
      body: JSON.stringify(payload),
    })

    if (!response.ok) {
      const errorData = await response.json()
      throw new Error(errorData.message || "Failed to update employee.")
    }

    const data = await response.json()
    return NextResponse.json({ success: true, data: data, message: "Employee updated successfully." })
  } catch (error) {
    console.error(`Error updating employee with ID ${id}:`, error)
    return NextResponse.json(
      { success: false, message: error instanceof Error ? error.message : "Failed to update employee." },
      { status: 500 }
    )
  }
}