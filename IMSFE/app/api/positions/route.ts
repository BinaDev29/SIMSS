import { NextResponse } from "next/server"
import { AuthService } from "@/lib/auth"

export async function GET() {
  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_BACKEND_URL}/api/Position`, {
      headers: AuthService.getAuthHeaders(),
      cache: "no-store",
    })

    if (!response.ok) {
      throw new Error("Failed to fetch positions from the backend.")
    }

    const data = await response.json()
    return NextResponse.json({ success: true, data: data, message: "Positions retrieved successfully" })
  } catch (error) {
    console.error("Error fetching positions:", error)
    return NextResponse.json(
      { success: false, message: "Failed to retrieve positions." },
      { status: 500 }
    )
  }
}