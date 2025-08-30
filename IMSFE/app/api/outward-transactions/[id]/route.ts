import { type NextRequest, NextResponse } from "next/server";

export async function GET(request: NextRequest, { params }: { params: { id: string } }) {
  try {
    const { id } = params;
    const response = await fetch(`${process.env.NEXT_PUBLIC_BACKEND_URL}/api/OutwardTransaction/${id}`);

    if (!response.ok) {
      if (response.status === 404) {
        return NextResponse.json(
          {
            success: false,
            message: "Outward Transaction not found in the backend",
          },
          { status: 404 }
        );
      }
      return NextResponse.json(
        {
          success: false,
          message: "Failed to retrieve outward transaction from the backend",
        },
        { status: response.status }
      );
    }

    const data = await response.json();
    
    return NextResponse.json({
      success: true,
      data: data,
      message: "Outward transaction retrieved successfully",
    });
  } catch (error) {
    return NextResponse.json(
      {
        success: false,
        message: "Failed to retrieve outward transaction",
      },
      { status: 500 }
    );
  }
}

export async function PUT(request: NextRequest, { params }: { params: { id: string } }) {
  try {
    const transactionId = params.id;
    const updateData = await request.json();
    
    const dataWithId = { ...updateData, id: Number.parseInt(transactionId) };

    const response = await fetch(`${process.env.NEXT_PUBLIC_BACKEND_URL}/api/OutwardTransaction`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(dataWithId),
    });

    const result = await response.json();

    if (!response.ok) {
      return NextResponse.json(
        {
          success: false,
          message: result.message || "Failed to update outward transaction in the backend",
        },
        { status: response.status }
      );
    }
    
    return NextResponse.json({
      success: true,
      data: result.data,
      message: "Outward Transaction Updated Successfully",
    });
  } catch (error) {
    return NextResponse.json(
      {
        success: false,
        message: "Failed to update outward transaction",
      },
      { status: 500 }
    );
  }
}

export async function DELETE(request: NextRequest, { params }: { params: { id: string } }) {
  try {
    const { id } = params;
    const response = await fetch(`${process.env.NEXT_PUBLIC_BACKEND_URL}/api/OutwardTransaction/${id}`, {
      method: "DELETE",
    });

    if (!response.ok) {
      return NextResponse.json(
        {
          success: false,
          message: "Failed to delete outward transaction from the backend",
        },
        { status: response.status }
      );
    }

    return NextResponse.json({
      success: true,
      message: "Outward Transaction Deleted Successfully",
    });
  } catch (error) {
    return NextResponse.json(
      {
        success: false,
        message: "Failed to delete outward transaction",
      },
      { status: 500 }
    );
  }
}