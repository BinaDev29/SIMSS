import { type NextRequest, NextResponse } from "next/server"

export async function GET() {
  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_BACKEND_URL}/api/Customer`)

    if (!response.ok) {
      return NextResponse.json(
        {
          success: false,
          message: "Failed to fetch customers from the backend",
        },
        { status: response.status },
      )
    }

    const data = await response.json()

    return NextResponse.json({
      success: true,
      data: data,
      message: "Customers retrieved successfully",
    })
  } catch (error) {
    return NextResponse.json(
      {
        success: false,
        message: "Failed to retrieve customers",
      },
      { status: 500 },
    )
  }
}

export async function POST(request: NextRequest) {
  try {
    const customerData = await request.json()

    const response = await fetch(`${process.env.NEXT_PUBLIC_BACKEND_URL}/api/Customer`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(customerData),
    })

    if (!response.ok) {
        return NextResponse.json(
          {
            success: false,
            message: "Failed to create customer in the backend",
          },
          { status: response.status },
        );
    }

    const result = await response.json();
    
    return NextResponse.json({
      success: true,
      data: result.data,
      message: "Customer Created Successfully",
    })
  } catch (error) {
    return NextResponse.json(
      {
        success: false,
        message: "Failed to create customer",
      },
      { status: 500 },
    )
  }
}