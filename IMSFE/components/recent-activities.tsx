"use client";

import { useState, useEffect } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { ArrowDownToLine, ArrowUpFromLine, RotateCcw, FileText, Loader2, AlertCircle } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { AuthService } from "@/lib/auth";

// ለእንቅስቃሴ የTypeScript አይነት
interface Activity {
  id: string;
  type: "inward" | "outward" | "return" | "invoice" | "other";
  description: string;
  timestamp: string;
  status: "completed" | "pending" | "failed";
}

const getIcon = (type: string) => {
  switch (type) {
    case "inward":
      return ArrowDownToLine;
    case "outward":
      return ArrowUpFromLine;
    case "return":
      return RotateCcw;
    case "invoice":
      return FileText;
    default:
      return FileText; // Default icon
  }
};

const getStatusColor = (status: string) => {
  switch (status) {
    case "completed":
      return "bg-green-100 text-green-800";
    case "pending":
      return "bg-yellow-100 text-yellow-800";
    case "failed":
      return "bg-red-100 text-red-800";
    default:
      return "bg-gray-100 text-gray-800";
  }
};

export function RecentActivities() {
  const [activities, setActivities] = useState<Activity[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchActivities = async () => {
      try {
        setIsLoading(true);
        setError(null);
        // መረጃውን ከNext.js API route ለማምጣት
        const response = await fetch("/api/activities/recent", {
          headers: AuthService.getAuthHeaders(),
        });

        const result = await response.json();

        if (response.ok && result.success) {
          setActivities(result.data || []);
        } else {
          throw new Error(result.message || "Failed to fetch recent activities.");
        }
      } catch (err) {
        console.error("Error fetching activities:", err);
        setError("Failed to load activities. Please try again.");
      } finally {
        setIsLoading(false);
      }
    };

    fetchActivities();
  }, []);

  const renderContent = () => {
    if (isLoading) {
      return (
        <CardContent className="p-6">
          <div className="text-center py-10">
            <Loader2 className="h-8 w-8 animate-spin text-accent mx-auto mb-4" />
            <p className="text-muted-foreground">Loading recent activities...</p>
          </div>
        </CardContent>
      );
    }

    if (error) {
      return (
        <CardContent className="p-6">
          <Alert variant="destructive">
            <AlertCircle className="h-4 w-4" />
            <AlertTitle>Error</AlertTitle>
            <AlertDescription>{error}</AlertDescription>
          </Alert>
        </CardContent>
      );
    }

    if (activities.length === 0) {
      return (
        <CardContent className="p-6">
          <div className="text-center py-8">
            <p className="text-muted-foreground">No recent activities found.</p>
          </div>
        </CardContent>
      );
    }

    return (
      <CardContent>
        <div className="space-y-4">
          {activities.map((activity) => {
            const ActivityIcon = getIcon(activity.type);
            return (
              <div key={activity.id} className="flex items-start space-x-4 p-3 rounded-lg hover:bg-muted/50">
                <div className="p-2 bg-accent/10 rounded-full">
                  <ActivityIcon className="h-4 w-4 text-accent" />
                </div>
                <div className="flex-1 min-w-0">
                  <p className="text-sm font-medium text-foreground">{activity.description}</p>
                  <p className="text-xs text-muted-foreground">{new Date(activity.timestamp).toLocaleString()}</p>
                </div>
                <Badge className={getStatusColor(activity.status)} variant="secondary">
                  {activity.status}
                </Badge>
              </div>
            );
          })}
        </div>
      </CardContent>
    );
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="font-[family-name:var(--font-space-grotesk)]">Recent Activities</CardTitle>
        <CardDescription>Latest transactions and system activities</CardDescription>
      </CardHeader>
      {renderContent()}
    </Card>
  );
}