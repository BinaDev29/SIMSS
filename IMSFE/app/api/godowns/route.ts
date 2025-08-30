import { type NextRequest, NextResponse } from "next/server";

export async function GET() {
  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_BACKEND_URL}/api/Godown`);

    if (!response.ok) {
      return NextResponse.json(
        {
          success: false,
          message: "Failed to retrieve godowns from the backend",
        },
        { status: response.status }
      );
    }

    const data = await response.json();
    
    return NextResponse.json({
      success: true,
      data: data,
      message: "Godowns retrieved successfully",
    });
  } catch (error) {
    return NextResponse.json(
      {
        success: false,
        message: "Failed to retrieve godowns",
      },
      { status: 500 }
    );
  }
}

export async function POST(request: NextRequest) {
  try {
    const godownData = await request.json();

    const response = await fetch(`${process.env.NEXT_PUBLIC_BACKEND_URL}/api/Godown`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(godownData),
    });

    const result = await response.json();

    if (!response.ok) {
      return NextResponse.json(
        {
          success: false,
          message: result.message || "Failed to create godown in the backend",
        },
        { status: response.status }
      );
    }
    
    return NextResponse.json({
      success: true,
      data: result.data,
      message: "Godown Created Successfully",
    });
  } catch (error) {
    return NextResponse.json(
      {
        success: false,
        message: "Failed to create godown",
      },
      { status: 500 }
    );
  }
}