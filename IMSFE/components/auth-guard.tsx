"use client"

import type React from "react"

import { useEffect, useState } from "react"
import { useRouter } from "next/navigation"
import { AuthService } from "@/lib/auth"

interface AuthGuardProps {
  children: React.ReactNode
  requiredRole?: string
}

export function AuthGuard({ children, requiredRole }: AuthGuardProps) {
  const [isAuthenticated, setIsAuthenticated] = useState(false)
  const [isLoading, setIsLoading] = useState(true)
  const router = useRouter()

  useEffect(() => {
    const checkAuth = () => {
      const authenticated = AuthService.isAuthenticated()

      if (!authenticated) {
        router.push("/")
        return
      }

      if (requiredRole && !AuthService.hasRole(requiredRole)) {
        router.push("/unauthorized")
        return
      }

      setIsAuthenticated(true)
      setIsLoading(false)
    }

    checkAuth()
  }, [requiredRole, router])

  if (isLoading) {
    return (
      <div className="min-h-screen bg-background flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-accent mx-auto mb-4"></div>
          <p className="text-muted-foreground">Loading...</p>
        </div>
      </div>
    )
  }

  return isAuthenticated ? <>{children}</> : null
}