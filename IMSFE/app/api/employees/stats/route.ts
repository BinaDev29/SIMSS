import { NextResponse } from "next/server";
import { AuthService } from "@/lib/auth";

export async function GET() {
  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_BACKEND_URL}/api/Employees/stats`, {
      headers: AuthService.getAuthHeaders(),
      cache: "no-store",
    });

    if (!response.ok) {
      throw new Error("Failed to retrieve employee stats from the backend.");
    }

    const data = await response.json();
    return NextResponse.json({
      success: true,
      data: data,
      message: "Employee stats retrieved successfully",
    });
  } catch (error) {
    console.error("Error in API route:", error);
    return NextResponse.json({ success: false, message: "Failed to retrieve employee stats." }, { status: 500 });
  }
}