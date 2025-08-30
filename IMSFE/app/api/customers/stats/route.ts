import { NextResponse } from "next/server"

export async function GET() {
  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_BACKEND_URL}/api/Customer/stats`)

    if (!response.ok) {
      return NextResponse.json(
        {
          success: false,
          message: "Failed to retrieve customer stats from the backend",
        },
        { status: response.status }
      )
    }

    const data = await response.json()

    return NextResponse.json({
      success: true,
      data: data,
      message: "Customer stats retrieved successfully",
    })
  } catch (error) {
    return NextResponse.json(
      {
        success: false,
        message: "Failed to retrieve customer stats",
      },
      { status: 500 }
    )
  }
}