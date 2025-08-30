"use client";

import { useState, useEffect } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Package, AlertTriangle, TrendingUp, Archive, Loader2, AlertCircle } from "lucide-react";
import { AuthService } from "@/lib/auth";

// ለዕቃ ዝርዝር ስታቲስቲክስ የTypeScript አይነት
interface ItemsStatsData {
  totalItems: number;
  lowStockItems: number;
  highValueItems: number;
  outOfStockItems: number;
}

// የለውጥ መረጃ አይነት
interface ChangeData {
  totalItemsChange: number;
  lowStockItemsChange: number;
  highValueItemsChange: number;
  outOfStockItemsChange: number;
}

// ለስታቲስቲክስ ካርዶች የሚያገለግል በይነገጽ
interface StatCardProps {
  title: string;
  value: string | number;
  change?: string;
  changeType?: "positive" | "negative";
  icon: React.ComponentType<{ className?: string }>;
  description: string;
}

export function ItemsStats() {
  const [stats, setStats] = useState<ItemsStatsData | null>(null);
  const [changes, setChanges] = useState<ChangeData | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        setIsLoading(true);
        setError(null);
        // መረጃውን ከNext.js API route ለማምጣት
        const response = await fetch("/api/items/stats", {
          headers: AuthService.getAuthHeaders(),
        });

        const result = await response.json();

        if (response.ok && result.success) {
          setStats(result.data.stats);
          setChanges(result.data.changes);
        } else {
          throw new Error(result.message || "Failed to fetch stats data.");
        }
      } catch (err) {
        console.error("Error fetching items stats:", err);
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

    const statCards: StatCardProps[] = [
      {
        title: "Total Items",
        value: stats.totalItems.toLocaleString(),
        icon: Package,
        description: "Active inventory items",
      },
      {
        title: "Low Stock Items",
        value: stats.lowStockItems.toLocaleString(),
        icon: AlertTriangle,
        description: "Items below threshold",
      },
      {
        title: "High Value Items",
        value: stats.highValueItems.toLocaleString(),
        icon: TrendingUp,
        description: "Items over $500",
      },
      {
        title: "Out of Stock",
        value: stats.outOfStockItems.toLocaleString(),
        icon: Archive,
        description: "Items with zero stock",
      },
    ];

    // የለውጥ መረጃ ካለ ያክላል
    if (changes) {
      statCards[0].change = `${changes.totalItemsChange >= 0 ? "+" : ""}${changes.totalItemsChange}%`;
      statCards[0].changeType = changes.totalItemsChange >= 0 ? "positive" : "negative";

      statCards[1].change = `${changes.lowStockItemsChange >= 0 ? "+" : ""}${changes.lowStockItemsChange}%`;
      statCards[1].changeType = changes.lowStockItemsChange >= 0 ? "positive" : "negative";

      statCards[2].change = `${changes.highValueItemsChange >= 0 ? "+" : ""}${changes.highValueItemsChange}%`;
      statCards[2].changeType = changes.highValueItemsChange >= 0 ? "positive" : "negative";

      statCards[3].change = `${changes.outOfStockItemsChange >= 0 ? "" : "-"}${Math.abs(changes.outOfStockItemsChange)}%`;
      statCards[3].changeType = changes.outOfStockItemsChange >= 0 ? "positive" : "negative";
    }

    return statCards;
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
              {stat.change && stat.changeType && (
                <div className="flex items-center space-x-2 text-xs text-muted-foreground">
                  <span className={`font-medium ${stat.changeType === "positive" ? "text-green-600" : "text-red-600"}`}>
                    {stat.change}
                  </span>
                  <span>from last month</span>
                </div>
              )}
              <p className="text-xs text-muted-foreground mt-1">{stat.description}</p>
            </CardContent>
          </Card>
        ))}
      </div>
    );
  };

  return <div>{renderContent()}</div>;
}