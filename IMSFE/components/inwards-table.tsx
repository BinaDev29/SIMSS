"use client";

import { useState, useEffect } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Loader2, AlertCircle } from "lucide-react";
import { AuthService } from "@/lib/auth";

// ለገቢ ግብይት የTypeScript አይነት
interface InwardTransaction {
  id: string;
  transactionDate: string;
  itemName: string;
  supplierName: string;
  quantity: number;
  totalPrice: number;
  status: "completed" | "pending" | "failed";
}

export function InwardsTable() {
  const [inwards, setInwards] = useState<InwardTransaction[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchInwards = async () => {
    try {
      setIsLoading(true);
      setError(null);
      // መረጃውን ከNext.js API route ለማምጣት
      const response = await fetch("/api/transactions/inwards", {
        headers: AuthService.getAuthHeaders(),
      });

      const result = await response.json();

      if (response.ok && result.success) {
        setInwards(result.data || []);
      } else {
        throw new Error(result.message || "Failed to fetch inward transactions.");
      }
    } catch (err) {
      console.error("Error fetching inwards:", err);
      setError("Failed to load inward transactions. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchInwards();
  }, []);

  const getStatusVariant = (status: string) => {
    switch (status) {
      case "completed":
        return "default";
      case "pending":
        return "secondary";
      case "failed":
        return "destructive";
      default:
        return "outline";
    }
  };

  const renderContent = () => {
    if (isLoading) {
      return (
        <CardContent className="p-6">
          <div className="text-center py-10">
            <Loader2 className="h-8 w-8 animate-spin text-accent mx-auto mb-4" />
            <p className="text-muted-foreground">Loading inward transactions...</p>
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

    if (inwards.length === 0) {
      return (
        <CardContent className="p-6">
          <div className="text-center py-8">
            <p className="text-muted-foreground">No inward transactions found.</p>
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
              <TableHead>Supplier</TableHead>
              <TableHead>Qty</TableHead>
              <TableHead>Total</TableHead>
              <TableHead>Status</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {inwards.map((inward) => (
              <TableRow key={inward.id}>
                <TableCell className="font-medium">{inward.id}</TableCell>
                <TableCell>{new Date(inward.transactionDate).toLocaleDateString()}</TableCell>
                <TableCell>{inward.itemName}</TableCell>
                <TableCell>{inward.supplierName}</TableCell>
                <TableCell>{inward.quantity}</TableCell>
                <TableCell>${inward.totalPrice.toFixed(2)}</TableCell>
                <TableCell>
                  <Badge variant={getStatusVariant(inward.status)}>{inward.status}</Badge>
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
        <CardTitle className="font-[family-name:var(--font-space-grotesk)]">Recent Inwards</CardTitle>
        <CardDescription>Latest inward transactions</CardDescription>
      </CardHeader>
      {renderContent()}
    </Card>
  );
}