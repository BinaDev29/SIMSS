"use client";

import { useState, useEffect } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Loader2, AlertCircle } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { AuthService } from "@/lib/auth";

// ለተመለሱ እቃዎች የTypeScript አይነት
interface ReturnTransaction {
  id: string;
  returnDate: string;
  itemName: string;
  customerName: string;
  quantity: number;
  reason: string;
  condition: string;
  status: "pending" | "processed" | "rejected";
}

interface ReturnsTableProps {
  refreshTrigger?: number;
}

export function ReturnsTable({ refreshTrigger }: ReturnsTableProps) {
  const [returns, setReturns] = useState<ReturnTransaction[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchReturns = async () => {
      try {
        setIsLoading(true);
        setError(null);
        // መረጃውን ከNext.js API route ለማምጣት
        const response = await fetch("/api/returns", {
          headers: AuthService.getAuthHeaders(),
        });

        const result = await response.json();

        if (response.ok && result.success) {
          // የመረጃውን ቅርፅ ከባክኤንድ ምላሽ ጋር ያዛምዳል
          setReturns(result.data.map((item: any) => ({
            id: item.id,
            returnDate: item.returnDate.split('T')[0], // ቀን ብቻ እንዲያሳይ
            itemName: item.item.name, // ከእቃው ዝርዝር ውስጥ ስሙን ያወጣል
            customerName: item.customer.name, // ከደንበኛው ዝርዝር ውስጥ ስሙን ያወጣል
            quantity: item.quantity,
            reason: item.reason,
            condition: item.condition,
            status: item.status,
          })) || []);
        } else {
          throw new Error(result.message || "Failed to fetch returns.");
        }
      } catch (err) {
        console.error("Error fetching returns:", err);
        setError("Failed to load return data. Please try again.");
      } finally {
        setIsLoading(false);
      }
    };

    fetchReturns();
  }, [refreshTrigger]); // የ`refreshTrigger` ዋጋ ሲቀየር ዳግም እንዲጫን ያደርጋል

  const getStatusVariant = (status: string) => {
    switch (status) {
      case "processed":
        return "default";
      case "pending":
        return "secondary";
      default:
        return "destructive";
    }
  };

  const renderContent = () => {
    if (isLoading) {
      return (
        <CardContent className="p-6">
          <div className="text-center py-10">
            <Loader2 className="h-8 w-8 animate-spin text-accent mx-auto mb-4" />
            <p className="text-muted-foreground">Loading returns data...</p>
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
    
    if (returns.length === 0) {
        return (
          <CardContent className="p-6">
            <div className="text-center py-8 text-muted-foreground">
              <p>No recent return transactions found.</p>
            </div>
          </CardContent>
        );
      }

    return (
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>ID</TableHead>
              <TableHead>Date</TableHead>
              <TableHead>Item</TableHead>
              <TableHead>Customer</TableHead>
              <TableHead>Qty</TableHead>
              <TableHead>Reason</TableHead>
              <TableHead>Status</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {returns.map((returnItem) => (
              <TableRow key={returnItem.id}>
                <TableCell className="font-medium">{returnItem.id}</TableCell>
                <TableCell>{returnItem.returnDate}</TableCell>
                <TableCell>{returnItem.itemName}</TableCell>
                <TableCell>{returnItem.customerName}</TableCell>
                <TableCell>{returnItem.quantity}</TableCell>
                <TableCell>{returnItem.reason}</TableCell>
                <TableCell>
                  <Badge variant={getStatusVariant(returnItem.status)}>
                    {returnItem.status}
                  </Badge>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </CardContent>
    );
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="font-[family-name:var(--font-space-grotesk)]">Recent Returns</CardTitle>
        <CardDescription>Latest return transactions</CardDescription>
      </CardHeader>
      {renderContent()}
    </Card>
  );
}