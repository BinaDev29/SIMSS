"use client";

import { useEffect, useState } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { UserCheck, Users, Clock, Award, Loader2, AlertCircle } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { AuthService } from "@/lib/auth";

interface StatData {
  title: string;
  value: string;
  change: string;
  changeType: "positive" | "negative";
  icon: React.ElementType;
  description: string;
}

export function EmployeesStats() {
  const [stats, setStats] = useState<StatData[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        const response = await fetch("/api/employees/stats", {
          headers: AuthService.getAuthHeaders(),
          cache: "no-store",
        });
        const result = await response.json();

        if (response.ok && result.success) {
          const fetchedStats = result.data;
          const transformedStats: StatData[] = [
            {
              title: "Total Employees",
              value: fetchedStats.totalEmployees.value,
              change: fetchedStats.totalEmployees.change,
              changeType: fetchedStats.totalEmployees.changeType,
              icon: Users,
              description: "Active employees",
            },
            {
              title: "New Hires",
              value: fetchedStats.newHires.value,
              change: fetchedStats.newHires.change,
              changeType: fetchedStats.newHires.changeType,
              icon: UserCheck,
              description: "This month",
            },
            {
              title: "On Leave",
              value: fetchedStats.onLeave.value,
              change: fetchedStats.onLeave.change,
              changeType: fetchedStats.onLeave.changeType,
              icon: Clock,
              description: "Currently on leave",
            },
            {
              title: "Top Performers",
              value: fetchedStats.topPerformers.value,
              change: fetchedStats.topPerformers.change,
              changeType: fetchedStats.topPerformers.changeType,
              icon: Award,
              description: "High-rated employees",
            },
          ];
          setStats(transformedStats);
        } else {
          throw new Error(result.message || "Failed to fetch employee statistics.");
        }
      } catch (err) {
        console.error("Error fetching employee stats:", err);
        setError("Failed to load employee statistics.");
      } finally {
        setIsLoading(false);
      }
    };
    fetchStats();
  }, []);

  const renderContent = () => {
    if (isLoading) {
      return (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
          {[...Array(4)].map((_, index) => (
            <Card key={index} className="animate-pulse">
              <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                <div className="h-4 w-1/2 rounded-md bg-gray-200" />
                <div className="h-4 w-4 rounded-full bg-gray-200" />
              </CardHeader>
              <CardContent className="space-y-2">
                <div className="h-6 w-3/4 rounded-md bg-gray-200" />
                <div className="h-4 w-1/2 rounded-md bg-gray-200" />
                <div className="h-3 w-full rounded-md bg-gray-200" />
              </CardContent>
            </Card>
          ))}
        </div>
      );
    }

    if (error) {
      return (
        <Alert variant="destructive">
          <AlertCircle className="h-4 w-4" />
          <AlertTitle>Error</AlertTitle>
          <AlertDescription>{error}</AlertDescription>
        </Alert>
      );
    }

    return (
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        {stats.map((stat) => (
          <Card key={stat.title}>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">{stat.title}</CardTitle>
              <stat.icon className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold font-[family-name:var(--font-space-grotesk)]">{stat.value}</div>
              <div className="flex items-center space-x-2 text-xs text-muted-foreground">
                <span className={`font-medium ${stat.changeType === "positive" ? "text-green-600" : "text-red-600"}`}>
                  {stat.change}
                </span>
                <span>from last month</span>
              </div>
              <p className="text-xs text-muted-foreground mt-1">{stat.description}</p>
            </CardContent>
          </Card>
        ))}
      </div>
    );
  };

  return <>{renderContent()}</>;
}