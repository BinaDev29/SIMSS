"use client";

import { useState, useEffect } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Truck, UserPlus, Star, TrendingUp, Loader2, AlertCircle } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { AuthService } from "@/lib/auth";

// የስታትስቲክስ አይነት
interface StatData {
  title: string;
  value: string;
  change: string;
  changeType: "positive" | "negative";
  icon: any;
  description: string;
}

export function SuppliersStats() {
  const [stats, setStats] = useState<StatData[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        setIsLoading(true);
        setError(null);

        // መረጃውን ከNext.js API route ለማምጣት
        const response = await fetch("/api/dashboard/suppliers-stats", {
          headers: AuthService.getAuthHeaders(),
        });

        const result = await response.json();

        if (response.ok && result.success) {
          // ከባክኤንድ የመጣውን መረጃ ወደ StatData አይነት ያዛምዳል
          const apiStats = result.data;
          const formattedStats: StatData[] = [
            {
              title: "Total Suppliers",
              value: apiStats.totalSuppliers.toString(),
              change: `${apiStats.totalSuppliersChange > 0 ? "+" : ""}${apiStats.totalSuppliersChange}%`,
              changeType: apiStats.totalSuppliersChange >= 0 ? "positive" : "negative",
              icon: Truck,
              description: "Active suppliers",
            },
            {
              title: "New This Month",
              value: apiStats.newSuppliersThisMonth.toString(),
              change: `${apiStats.newSuppliersChange > 0 ? "+" : ""}${apiStats.newSuppliersChange}%`,
              changeType: apiStats.newSuppliersChange >= 0 ? "positive" : "negative",
              icon: UserPlus,
              description: "New partnerships",
            },
            {
              title: "Preferred Suppliers",
              value: apiStats.preferredSuppliers.toString(),
              change: `${apiStats.preferredSuppliersChange > 0 ? "+" : ""}${apiStats.preferredSuppliersChange}%`,
              changeType: apiStats.preferredSuppliersChange >= 0 ? "positive" : "negative",
              icon: Star,
              description: "Top-rated suppliers",
            },
            {
              title: "Supplier Growth",
              value: `${apiStats.supplierGrowthRate}%`,
              change: `${apiStats.supplierGrowthRateChange > 0 ? "+" : ""}${apiStats.supplierGrowthRateChange}%`,
              changeType: apiStats.supplierGrowthRateChange >= 0 ? "positive" : "negative",
              icon: TrendingUp,
              description: "Monthly growth rate",
            },
          ];
          setStats(formattedStats);
        } else {
          throw new Error(result.message || "Failed to fetch supplier statistics.");
        }
      } catch (err) {
        console.error("Error fetching stats:", err);
        setError("Failed to load supplier statistics. Please try again.");
      } finally {
        setIsLoading(false);
      }
    };

    fetchStats();
  }, []); // ኮምፖነንቱ ለመጀመሪያ ጊዜ ሲጫን ብቻ መረጃ ያመጣል

  if (isLoading) {
    return (
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4 min-h-[150px] items-center justify-center">
        <Loader2 className="h-8 w-8 animate-spin text-accent mx-auto" />
      </div>
    );
  }

  if (error) {
    return (
      <div className="col-span-full">
        <Alert variant="destructive">
          <AlertCircle className="h-4 w-4" />
          <AlertTitle>Error</AlertTitle>
          <AlertDescription>{error}</AlertDescription>
        </Alert>
      </div>
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
}