import { type NextRequest, NextResponse } from "next/server"

export async function GET(request: NextRequest, { params }: { params: { id: string } }) {
  try {
    const { id } = params
    const response = await fetch(`${process.env.NEXT_PUBLIC_BACKEND_URL}/api/Godown/${id}`)

    if (!response.ok) {
      if (response.status === 404) {
        return NextResponse.json(
          {
            success: false,
            message: "Godown not found in the backend",
          },
          { status: 404 },
        )
      }
      return NextResponse.json(
        {
          success: false,
          message: "Failed to retrieve godown from the backend",
        },
        { status: response.status },
      )
    }

    const data = await response.json()

    return NextResponse.json({
      success: true,
      data: data,
      message: "Godown retrieved successfully",
    })
  } catch (error) {
    return NextResponse.json(
      {
        success: false,
        message: "Failed to retrieve godown",
      },
      { status: 500 },
    )
  }
}

export async function PUT(request: NextRequest, { params }: { params: { id: string } }) {
  try {
    const godownId = params.id
    const updateData = await request.json()
    
    // Add the ID to the update data object to send it to the backend
    const dataWithId = { ...updateData, id: Number.parseInt(godownId) }

    const response = await fetch(`${process.env.NEXT_PUBLIC_BACKEND_URL}/api/Godown`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(dataWithId),
    })

    const result = await response.json()

    if (!response.ok) {
      return NextResponse.json(
        {
          success: false,
          message: result.message || "Failed to update godown in the backend",
        },
        { status: response.status },
      )
    }
    
    return NextResponse.json({
      success: true,
      data: result.data,
      message: "Godown Updated Successfully",
    })
  } catch (error) {
    return NextResponse.json(
      {
        success: false,
        message: "Failed to update godown",
      },
      { status: 500 },
    )
  }
}

export async function DELETE(request: NextRequest, { params }: { params: { id: string } }) {
  try {
    const { id } = params
    const response = await fetch(`${process.env.NEXT_PUBLIC_BACKEND_URL}/api/Godown/${id}`, {
      method: "DELETE",
    })

    if (!response.ok) {
        return NextResponse.json(
          {
            success: false,
            message: "Failed to delete godown from the backend",
          },
          { status: response.status },
        )
    }

    return NextResponse.json({
      success: true,
      message: "Godown Deleted Successfully",
    })
  } catch (error) {
    return NextResponse.json(
      {
        success: false,
        message: "Failed to delete godown",
      },
      { status: 500 },
    )
  }
}