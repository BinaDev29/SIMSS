"use client"

import { AuthService } from "./auth"
import { useState } from "react"

export interface ApiResponse<T = any> {
  success: boolean
  message: string
  data?: T
  errors?: string[]
}

export interface ApiError {
  message: string
  status: number
  errors?: string[]
}

export class ApiClient {
  private static readonly BASE_URL = process.env.NEXT_PUBLIC_API_URL || ""

  private static async handleResponse<T>(response: Response): Promise<ApiResponse<T>> {
    try {
      const data = await response.json()

      if (!response.ok) {
        throw new ApiError({
          message: data.message || `HTTP ${response.status}: ${response.statusText}`,
          status: response.status,
          errors: data.errors,
        })
      }

      return data
    } catch (error) {
      if (error instanceof ApiError) {
        throw error
      }

      throw new ApiError({
        message: "Failed to parse response",
        status: response.status,
      })
    }
  }

  private static async makeRequest<T>(endpoint: string, options: RequestInit = {}): Promise<ApiResponse<T>> {
    try {
      const url = `${this.BASE_URL}${endpoint}`
      const headers = {
        "Content-Type": "application/json",
        ...AuthService.getAuthHeaders(),
        ...options.headers,
      }

      const response = await fetch(url, {
        ...options,
        headers,
      })

      // Handle authentication errors
      if (response.status === 401) {
        AuthService.logout()
        window.location.href = "/"
        throw new ApiError({
          message: "Session expired. Please login again.",
          status: 401,
        })
      }

      return await this.handleResponse<T>(response)
    } catch (error) {
      if (error instanceof ApiError) {
        throw error
      }

      // Network or other errors
      throw new ApiError({
        message: "Network error occurred. Please check your connection.",
        status: 0,
      })
    }
  }

  // Generic CRUD operations
  static async get<T>(endpoint: string): Promise<ApiResponse<T>> {
    return this.makeRequest<T>(endpoint, { method: "GET" })
  }

  static async post<T>(endpoint: string, data: any): Promise<ApiResponse<T>> {
    return this.makeRequest<T>(endpoint, {
      method: "POST",
      body: JSON.stringify(data),
    })
  }

  static async put<T>(endpoint: string, data: any): Promise<ApiResponse<T>> {
    return this.makeRequest<T>(endpoint, {
      method: "PUT",
      body: JSON.stringify(data),
    })
  }

  static async delete<T>(endpoint: string): Promise<ApiResponse<T>> {
    return this.makeRequest<T>(endpoint, { method: "DELETE" })
  }

  // Specific API methods for inventory system
  static async getCustomers() {
    return this.get<any[]>("/api/customers")
  }

  static async createCustomer(customerData: any) {
    return this.post("/api/customers", customerData)
  }

  static async updateCustomer(id: number, customerData: any) {
    return this.put(`/api/customers/${id}`, customerData)
  }

  static async deleteCustomer(id: number) {
    return this.delete(`/api/customers/${id}`)
  }

  static async getGodowns() {
    return this.get<any[]>("/api/godowns")
  }

  static async createGodown(godownData: any) {
    return this.post("/api/godowns", godownData)
  }

  static async updateGodown(id: number, godownData: any) {
    return this.put(`/api/godowns/${id}`, godownData)
  }

  static async deleteGodown(id: number) {
    return this.delete(`/api/godowns/${id}`)
  }

  static async getOutwardTransactions() {
    return this.get<any[]>("/api/outward-transactions")
  }

  static async createOutwardTransaction(transactionData: any) {
    return this.post("/api/outward-transactions", transactionData)
  }

  static async updateOutwardTransaction(id: number, transactionData: any) {
    return this.put(`/api/outward-transactions/${id}`, transactionData)
  }

  static async deleteOutwardTransaction(id: number) {
    return this.delete(`/api/outward-transactions/${id}`)
  }
}

// Custom error class for API errors
class ApiError extends Error {
  public status: number
  public errors?: string[]

  constructor({ message, status, errors }: { message: string; status: number; errors?: string[] }) {
    super(message)
    this.name = "ApiError"
    this.status = status
    this.errors = errors
  }
}

// Utility functions for error handling
export const handleApiError = (error: unknown): string => {
  if (error instanceof ApiError) {
    if (error.errors && error.errors.length > 0) {
      return error.errors.join(", ")
    }
    return error.message
  }

  if (error instanceof Error) {
    return error.message
  }

  return "An unexpected error occurred"
}

export const useApiCall = () => {
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<string>("")
  \
  const execute = async <T>(apiCall: () => Promise<ApiResponse<T>>)
  : Promise<T | null> =>
  setIsLoading(true)
  setError("")

  try {
    const response = await apiCall()
    if (response.success) {
      return response.data || null
    } else {
      setError(response.message)
      return null
    }
  } catch (error) {
    setError(handleApiError(error))
    return null
  } finally {
    setIsLoading(false)
  }

  return { execute, isLoading, error, setError }
}
