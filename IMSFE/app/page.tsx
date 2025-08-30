import { LoginForm } from "@/components/login-form"

export default function LoginPage() {
  return (
    <div className="min-h-screen bg-background flex items-center justify-center p-4">
      <div className="w-full max-w-md">
        <div className="text-center mb-8">
          <h1 className="text-3xl font-bold text-foreground font-[family-name:var(--font-space-grotesk)]">
            Inventory Management System
          </h1>
          <p className="text-muted-foreground mt-2">Professional warehouse and inventory control</p>
        </div>
        <LoginForm />
      </div>
    </div>
  )
}
