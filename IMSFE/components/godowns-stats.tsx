"use client"

import React, { useState, useEffect } from "react"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Warehouse, Package, AlertTriangle, TrendingUp, Loader2, AlertCircle } from "lucide-react"
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert"
import { AuthService } from "@/lib/auth"

// ለGodown ስታቲስቲክስ የሚሆን የTypeScript አይነት
interface GodownStats {
  totalGodowns: number;
  totalCapacity: number;
  capacityUsedPercentage: number;
  maintenanceDue: number;
}

// የDashboard ካርዶችን ለመወሰን የሚያገለግል በይነገጽ (interface)
interface StatCardProps {
  title: string;
  value: string | number;
  change?: string;
  changeType?: "positive" | "negative";
  icon: React.ComponentType<{ className?: string }>;
  description: string;
}

export function GodownsStats() {
  const [stats, setStats] = useState<GodownStats | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        setIsLoading(true);
        setError(null);
        // መረጃውን ከNext.js API route ለማምጣት
        const response = await fetch("/api/godowns/stats", {
          headers: AuthService.getAuthHeaders(),
        });
        const result = await response.json();

        if (response.ok && result.success) {
          setStats(result.data);
        } else {
          throw new Error(result.message || "Failed to fetch stats data.");
        }
      } catch (err) {
        console.error("Error fetching stats:", err);
        setError("Failed to load statistics. Please try again.");
      } finally {
        setIsLoading(false);
      }
    };

    fetchStats();
  }, []);

  const getStatCards = (): StatCardProps[] => {
    if (!stats) {
      return [];
    }

    return [
      {
        title: "Total Godowns",
        value: stats.totalGodowns.toLocaleString(),
        icon: Warehouse,
        description: "Active warehouses",
      },
      {
        title: "Total Capacity",
        value: stats.totalCapacity.toLocaleString() + " m³",
        icon: Package,
        description: "Total cubic meters",
      },
      {
        title: "Capacity Used",
        value: `${stats.capacityUsedPercentage}%`,
        icon: TrendingUp,
        description: "Current utilization",
      },
      {
        title: "Maintenance Due",
        value: stats.maintenanceDue.toLocaleString(),
        icon: AlertTriangle,
        description: "Godowns needing service",
      },
    ];
  };

  const statCards = getStatCards();

  const renderContent = () => {
    if (isLoading) {
      return (
        <div className="text-center py-8">
          <Loader2 className="h-8 w-8 animate-spin text-accent mx-auto mb-4" />
          <p className="text-muted-foreground">Loading statistics...</p>
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
        {statCards.map((stat) => (
          <Card key={stat.title}>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">{stat.title}</CardTitle>
              <stat.icon className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold font-[family-name:var(--font-space-grotesk)]">{stat.value}</div>
              <p className="text-xs text-muted-foreground mt-1">{stat.description}</p>
            </CardContent>
          </Card>
        ))}
      </div>
    );
  };

  return <div>{renderContent()}</div>;
}