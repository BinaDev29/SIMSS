"use client"

import { useEffect, useState } from "react"
import { AuthService } from "@/lib/auth"

export function DashboardHeader() {
  const [username, setUsername] = useState<string>("")

  useEffect(() => {
    const user = AuthService.getUser()
    if (user) {
      setUsername(user.username)
    }
  }, [])

  return (
    <div className="flex items-center justify-between">
      <div>
        <h1 className="text-3xl font-bold text-foreground font-[family-name:var(--font-space-grotesk)]">Dashboard</h1>
        <p className="text-muted-foreground">
          Welcome back{username ? `, ${username}` : ""}! Here's an overview of your inventory system.
        </p>
      </div>
      <div className="text-sm text-muted-foreground">
        {new Date().toLocaleDateString("en-US", {
          weekday: "long",
          year: "numeric",
          month: "long",
          day: "numeric",
        })}
      </div>
    </div>
  )
}
