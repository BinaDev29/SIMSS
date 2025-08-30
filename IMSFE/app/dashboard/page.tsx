import { DashboardHeader } from "@/components/dashboard-header"
import { DashboardStats } from "@/components/dashboard-stats"
import { RecentActivities } from "@/components/recent-activities"
import { DashboardLayout } from "@/components/dashboard-layout"
import { AuthGuard } from "@/components/auth-guard"

export default function DashboardPage() {
  return (
    <AuthGuard>
      <DashboardLayout>
        <div className="flex-1 space-y-6 p-6">
          <DashboardHeader />
          <DashboardStats />
          <RecentActivities />
        </div>
      </DashboardLayout>
    </AuthGuard>
  )
}
