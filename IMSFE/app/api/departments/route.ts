import { NextResponse } from "next/server"
import { AuthService } from "@/lib/auth"

export async function GET() {
  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_BACKEND_URL}/api/Department`, {
      headers: AuthService.getAuthHeaders(),
      cache: "no-store",
    })

    if (!response.ok) {
      throw new Error("Failed to fetch departments from the backend.")
    }

    const data = await response.json()
    return NextResponse.json({ success: true, data: data, message: "Departments retrieved successfully" })
  } catch (error) {
    console.error("Error fetching departments:", error)
    return NextResponse.json(
      { success: false, message: "Failed to retrieve departments." },
      { status: 500 }
    )
  }
}