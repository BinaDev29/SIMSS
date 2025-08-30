"use client";

import { useEffect, useState } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Package, ArrowDownToLine, ArrowUpFromLine, TrendingUp, Loader2, AlertCircle } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";

interface StatData {
  title: string;
  value: string | number;
  change: string;
  changeType: "positive" | "negative";
  icon: React.ElementType;
  description: string;
}

export function DashboardStats() {
  const [stats, setStats] = useState<StatData[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        const response = await fetch("/api/dashboard/stats");
        if (!response.ok) {
          throw new Error("Failed to fetch data");
        }
        const result = await response.json();
        const fetchedStats = result.data;

        // Transform fetched data to the desired format
        const transformedStats: StatData[] = [
          {
            title: "Total In-Stock Items",
            value: fetchedStats.totalInStockItems.value,
            change: fetchedStats.totalInStockItems.change,
            changeType: fetchedStats.totalInStockItems.changeType,
            icon: Package,
            description: "Items currently in inventory",
          },
          {
            title: "Today's Inwards",
            value: fetchedStats.todaysInwards.value,
            change: fetchedStats.todaysInwards.change,
            changeType: fetchedStats.todaysInwards.changeType,
            icon: ArrowDownToLine,
            description: "Items received today",
          },
          {
            title: "Today's Deliveries",
            value: fetchedStats.todaysDeliveries.value,
            change: fetchedStats.todaysDeliveries.change,
            changeType: fetchedStats.todaysDeliveries.changeType,
            icon: ArrowUpFromLine,
            description: "Items delivered today",
          },
          {
            title: "Stock Value",
            value: fetchedStats.stockValue.value,
            change: fetchedStats.stockValue.change,
            changeType: fetchedStats.stockValue.changeType,
            icon: TrendingUp,
            description: "Total inventory value",
          },
        ];
        setStats(transformedStats);
      } catch (err) {
        setError("Failed to load dashboard statistics.");
        console.error("Error fetching dashboard stats:", err);
      } finally {
        setIsLoading(false);
      }
    };
    fetchStats();
  }, []);

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
              <span>from yesterday</span>
            </div>
            <p className="text-xs text-muted-foreground mt-1">{stat.description}</p>
          </CardContent>
        </Card>
      ))}
    </div>
  );
}