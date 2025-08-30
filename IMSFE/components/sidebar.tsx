"use client"
import Link from "next/link"
import { usePathname } from "next/navigation"
import { Button } from "@/components/ui/button"
import { AuthService } from "@/lib/auth"
import { useEffect, useState } from "react"
import {
  Package,
  LayoutDashboard,
  ArrowDownToLine,
  ArrowUpFromLine,
  RotateCcw,
  FileText,
  Box,
  Users,
  Truck,
  UserCheck,
  Warehouse,
  X,
  LogOut,
} from "lucide-react"

interface SidebarProps {
  onClose?: () => void
}

const navigationItems = [
  {
    name: "Dashboard",
    href: "/dashboard",
    icon: LayoutDashboard,
    roles: ["Admin", "Manager", "User"], // All roles can access dashboard
  },
  {
    name: "Inwards",
    href: "/dashboard/inwards",
    icon: ArrowDownToLine,
    roles: ["Admin", "Manager", "User"],
  },
  {
    name: "Deliveries",
    href: "/dashboard/deliveries",
    icon: ArrowUpFromLine,
    roles: ["Admin", "Manager", "User"],
  },
  {
    name: "Returns",
    href: "/dashboard/returns",
    icon: RotateCcw,
    roles: ["Admin", "Manager"],
  },
  {
    name: "Invoices",
    href: "/dashboard/invoices",
    icon: FileText,
    roles: ["Admin", "Manager"],
  },
  {
    name: "Items",
    href: "/dashboard/items",
    icon: Box,
    roles: ["Admin", "Manager"],
  },
  {
    name: "Customers",
    href: "/dashboard/customers",
    icon: Users,
    roles: ["Admin", "Manager"],
  },
  {
    name: "Suppliers",
    href: "/dashboard/suppliers",
    icon: Truck,
    roles: ["Admin", "Manager"],
  },
  {
    name: "Employees",
    href: "/dashboard/employees",
    icon: UserCheck,
    roles: ["Admin"], // Only admin can manage employees
  },
  {
    name: "Godowns",
    href: "/dashboard/godowns",
    icon: Warehouse,
    roles: ["Admin", "Manager"],
  },
]

export function Sidebar({ onClose }: SidebarProps) {
  const pathname = usePathname()
  const [userRole, setUserRole] = useState<string | null>(null)
  const [username, setUsername] = useState<string>("")

  useEffect(() => {
    const user = AuthService.getUser()
    if (user) {
      setUserRole(user.role)
      setUsername(user.username)
    }
  }, [])

  const filteredNavigationItems = navigationItems.filter((item) => (userRole ? item.roles.includes(userRole) : false))

  const handleLogout = () => {
    AuthService.logout()
    window.location.href = "/"
  }

  return (
    <div className="flex flex-col h-full">
      {/* Header */}
      <div className="flex items-center justify-between p-6 border-b border-sidebar-border">
        <div className="flex items-center space-x-2">
          <div className="p-2 bg-sidebar-accent rounded-lg">
            <Package className="h-6 w-6 text-sidebar-accent-foreground" />
          </div>
          <div>
            <h1 className="text-lg font-bold text-sidebar-foreground font-[family-name:var(--font-space-grotesk)]">
              Inventory
            </h1>
            <p className="text-xs text-sidebar-foreground/60">Management System</p>
          </div>
        </div>
        {onClose && (
          <Button variant="ghost" size="sm" onClick={onClose} className="lg:hidden">
            <X className="h-4 w-4" />
          </Button>
        )}
      </div>

      {username && userRole && (
        <div className="p-4 border-b border-sidebar-border">
          <div className="text-sm">
            <p className="font-medium text-sidebar-foreground">{username}</p>
            <p className="text-sidebar-foreground/60">{userRole}</p>
          </div>
        </div>
      )}

      {/* Navigation */}
      <nav className="flex-1 p-4 space-y-2">
        {filteredNavigationItems.map((item) => {
          const isActive = pathname === item.href
          return (
            <Link key={item.name} href={item.href}>
              <Button
                variant={isActive ? "default" : "ghost"}
                className={`w-full justify-start ${
                  isActive
                    ? "bg-sidebar-accent text-sidebar-accent-foreground"
                    : "text-sidebar-foreground hover:bg-sidebar-accent/10"
                }`}
                onClick={onClose}
              >
                <item.icon className="mr-3 h-4 w-4" />
                {item.name}
              </Button>
            </Link>
          )
        })}
      </nav>

      {/* Footer */}
      <div className="p-4 border-t border-sidebar-border">
        <Button variant="ghost" className="w-full justify-start text-sidebar-foreground" onClick={handleLogout}>
          <LogOut className="mr-3 h-4 w-4" />
          Sign Out
        </Button>
      </div>
    </div>
  )
}
