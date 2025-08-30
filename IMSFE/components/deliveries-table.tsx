"use client";

import { useState, useEffect, type ReactNode } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Edit, Trash2, AlertCircle, Loader2 } from "lucide-react";
import { AuthService } from "@/lib/auth";
import { toast } from "react-hot-toast";

interface OutwardTransaction {
  id: number;
  itemId: number;
  customerId: number;
  godownId: number;
  quantity: number;
  transactionDate: string;
  notes?: string;
  // Populated fields from the new API endpoint
  itemName?: string;
  customerName?: string;
  godownName?: string;
}

interface DeliveriesTableProps {
  refreshTrigger?: number;
  onEditTransaction?: (transaction: OutwardTransaction) => void;
}

export function DeliveriesTable({ refreshTrigger, onEditTransaction }: DeliveriesTableProps) {
  const [transactions, setTransactions] = useState<OutwardTransaction[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");

  const fetchTransactions = async () => {
    try {
      setIsLoading(true);
      const response = await fetch("/api/outward-transactions", {
        headers: AuthService.getAuthHeaders(),
      });

      const result = await response.json();

      if (response.ok && result.success) {
        setTransactions(result.data || []);
      } else {
        throw new Error(result.message || "Failed to fetch transactions");
      }
    } catch (error) {
      console.error("Error fetching transactions:", error);
      setError("Network error occurred. Please try again later.");
    } finally {
      setIsLoading(false);
    }
  };

  const handleDeleteTransaction = async (transactionId: number) => {
    const confirmation = window.confirm("Are you sure you want to delete this transaction?");
    if (!confirmation) {
      return;
    }

    const toastId = toast.loading("Deleting transaction...");
    try {
      const response = await fetch("/api/outward-transactions", {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
          ...AuthService.getAuthHeaders(),
        },
        body: JSON.stringify({ id: transactionId }),
      });

      const result = await response.json();
      if (response.ok && result.success) {
        setTransactions(transactions.filter((t) => t.id !== transactionId));
        toast.success("Transaction deleted successfully.", { id: toastId });
      } else {
        throw new Error(result.message || "Failed to delete transaction");
      }
    } catch (error) {
      console.error("Error deleting transaction:", error);
      toast.error(error instanceof Error ? error.message : "Network error occurred.", { id: toastId });
    }
  };

  useEffect(() => {
    fetchTransactions();
  }, [refreshTrigger]);

  const renderTableContent = (): ReactNode => {
    if (isLoading) {
      return (
        <div className="text-center py-8">
          <Loader2 className="h-8 w-8 animate-spin text-accent mx-auto mb-4" />
          <p className="text-muted-foreground">Loading transactions...</p>
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

    if (transactions.length === 0) {
      return (
        <div className="text-center py-8">
          <p className="text-muted-foreground">
            No transactions found. Record your first outward transaction to get started.
          </p>
        </div>
      );
    }

    return (
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>ID</TableHead>
            <TableHead>Date</TableHead>
            <TableHead>Item</TableHead>
            <TableHead>Customer</TableHead>
            <TableHead>Godown</TableHead>
            <TableHead>Quantity</TableHead>
            <TableHead>Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {transactions.map((transaction) => (
            <TableRow key={transaction.id}>
              <TableCell className="font-medium">#{transaction.id}</TableCell>
              <TableCell>{new Date(transaction.transactionDate).toLocaleDateString()}</TableCell>
              <TableCell>{transaction.itemName || `Item #${transaction.itemId}`}</TableCell>
              <TableCell>{transaction.customerName || `Customer #${transaction.customerId}`}</TableCell>
              <TableCell>{transaction.godownName || `Godown #${transaction.godownId}`}</TableCell>
              <TableCell>
                <Badge variant="outline">{transaction.quantity}</Badge>
              </TableCell>
              <TableCell>
                <div className="flex items-center space-x-2">
                  <Button variant="ghost" size="sm" onClick={() => onEditTransaction?.(transaction)}>
                    <Edit className="h-4 w-4" />
                  </Button>
                  <Button variant="ghost" size="sm" onClick={() => handleDeleteTransaction(transaction.id)}>
                    <Trash2 className="h-4 w-4" />
                  </Button>
                </div>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    );
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="font-[family-name:var(--font-space-grotesk)]">Outward Transactions</CardTitle>
        <CardDescription>Recent delivery transactions ({transactions.length} records)</CardDescription>
      </CardHeader>
      <CardContent>
        {renderTableContent()}
      </CardContent>
    </Card>
  );
}