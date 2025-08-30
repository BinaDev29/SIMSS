"use client";

import { useState, useEffect } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Edit, Trash2, AlertTriangle, Loader2, AlertCircle } from "lucide-react";
import { AuthService } from "@/lib/auth";

// ለዕቃ የTypeScript አይነት
interface Item {
  id: string;
  name: string;
  description: string;
  category: string;
  unitPrice: number;
  currentStock: number;
  minStockLevel: number;
  unit: string;
}

export function ItemsTable() {
  const [items, setItems] = useState<Item[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchItems = async () => {
    try {
      setIsLoading(true);
      setError(null);
      const response = await fetch("/api/items", {
        headers: AuthService.getAuthHeaders(),
      });
      const result = await response.json();

      if (response.ok && result.success) {
        setItems(result.data || []);
      } else {
        throw new Error(result.message || "Failed to fetch items.");
      }
    } catch (err) {
      console.error("Error fetching items:", err);
      setError("Network error occurred. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchItems();
  }, []);

  const getStatus = (item: Item): "in-stock" | "low-stock" | "out-of-stock" => {
    if (item.currentStock === 0) {
      return "out-of-stock";
    }
    if (item.currentStock <= item.minStockLevel) {
      return "low-stock";
    }
    return "in-stock";
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case "in-stock":
        return "bg-green-100 text-green-800";
      case "low-stock":
        return "bg-yellow-100 text-yellow-800";
      case "out-of-stock":
        return "bg-red-100 text-red-800";
      default:
        return "bg-gray-100 text-gray-800";
    }
  };

  const renderContent = () => {
    if (isLoading) {
      return (
        <CardContent className="p-6">
          <div className="text-center py-10">
            <Loader2 className="h-8 w-8 animate-spin text-accent mx-auto mb-4" />
            <p className="text-muted-foreground">Loading items...</p>
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

    if (items.length === 0) {
      return (
        <CardContent className="p-6">
          <div className="text-center py-8">
            <p className="text-muted-foreground">No items found. Add your first item to get started.</p>
          </div>
        </CardContent>
      );
    }

    return (
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Item ID</TableHead>
              <TableHead>Name</TableHead>
              <TableHead>Category</TableHead>
              <TableHead>Unit Price</TableHead>
              <TableHead>Current Stock</TableHead>
              <TableHead>Status</TableHead>
              <TableHead>Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {items.map((item) => {
              const status = getStatus(item);
              return (
                <TableRow key={item.id}>
                  <TableCell className="font-medium">{item.id}</TableCell>
                  <TableCell>
                    <div>
                      <div className="font-medium">{item.name}</div>
                      <div className="text-sm text-muted-foreground">{item.description}</div>
                    </div>
                  </TableCell>
                  <TableCell>{item.category}</TableCell>
                  <TableCell>${item.unitPrice.toFixed(2)}</TableCell>
                  <TableCell>
                    <div className="flex items-center space-x-2">
                      <span>
                        {item.currentStock} {item.unit}
                      </span>
                      {status === "low-stock" && <AlertTriangle className="h-4 w-4 text-yellow-600" />}
                    </div>
                  </TableCell>
                  <TableCell>
                    <Badge className={getStatusColor(status)} variant="secondary">
                      {status.replace("-", " ")}
                    </Badge>
                  </TableCell>
                  <TableCell>
                    <div className="flex items-center space-x-2">
                      <Button variant="ghost" size="sm">
                        <Edit className="h-4 w-4" />
                      </Button>
                      <Button variant="ghost" size="sm">
                        <Trash2 className="h-4 w-4" />
                      </Button>
                    </div>
                  </TableCell>
                </TableRow>
              );
            })}
          </TableBody>
        </Table>
      </CardContent>
    );
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="font-[family-name:var(--font-space-grotesk)]">All Items</CardTitle>
        <CardDescription>Complete inventory item list with stock levels</CardDescription>
      </CardHeader>
      {renderContent()}
    </Card>
  );
}