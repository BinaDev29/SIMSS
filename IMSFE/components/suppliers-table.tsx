"use client";

import { useState, useEffect } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Edit, Trash2, Mail, Phone, Loader2, AlertCircle } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { AuthService } from "@/lib/auth";

// የአቅራቢ አይነት
interface Supplier {
  id: string;
  companyName: string;
  contactPerson: string;
  email: string;
  phone: string;
  address: string;
  supplierType: "manufacturer" | "distributor" | "wholesaler" | "service" | string;
  paymentTerms: "net-30" | "net-60" | "net-90" | "cod" | "prepaid" | string;
  totalOrders: number;
  rating: number;
}

interface SuppliersTableProps {
  refreshTrigger?: number;
}

const getSupplierTypeColor = (type: string) => {
  switch (type.toLowerCase()) {
    case "manufacturer":
      return "bg-blue-100 text-blue-800";
    case "distributor":
      return "bg-green-100 text-green-800";
    case "wholesaler":
      return "bg-purple-100 text-purple-800";
    case "service":
      return "bg-orange-100 text-orange-800";
    default:
      return "bg-gray-100 text-gray-800";
  }
};

export function SuppliersTable({ refreshTrigger }: SuppliersTableProps) {
  const [suppliers, setSuppliers] = useState<Supplier[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchSuppliers = async () => {
      try {
        setIsLoading(true);
        setError(null);
        // መረጃውን ከNext.js API route ለማምጣት
        const response = await fetch("/api/suppliers", {
          headers: AuthService.getAuthHeaders(),
        });

        const result = await response.json();

        if (response.ok && result.success) {
          setSuppliers(result.data || []);
        } else {
          throw new Error(result.message || "Failed to fetch suppliers.");
        }
      } catch (err) {
        console.error("Error fetching suppliers:", err);
        setError("Failed to load supplier data. Please try again.");
      } finally {
        setIsLoading(false);
      }
    };

    fetchSuppliers();
  }, [refreshTrigger]); // የ`refreshTrigger` ዋጋ ሲቀየር ዳግም እንዲጫን ያደርጋል

  const renderContent = () => {
    if (isLoading) {
      return (
        <CardContent className="p-6">
          <div className="text-center py-10">
            <Loader2 className="h-8 w-8 animate-spin text-accent mx-auto mb-4" />
            <p className="text-muted-foreground">Loading suppliers...</p>
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
    
    if (suppliers.length === 0) {
      return (
        <CardContent className="p-6">
          <div className="text-center py-8 text-muted-foreground">
            <p>No suppliers found. Add a new supplier to get started.</p>
          </div>
        </CardContent>
      );
    }

    return (
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Supplier ID</TableHead>
              <TableHead>Company</TableHead>
              <TableHead>Contact</TableHead>
              <TableHead>Type</TableHead>
              <TableHead>Payment Terms</TableHead>
              <TableHead>Orders</TableHead>
              <TableHead>Rating</TableHead>
              <TableHead>Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {suppliers.map((supplier) => (
              <TableRow key={supplier.id}>
                <TableCell className="font-medium">{supplier.id}</TableCell>
                <TableCell>
                  <div>
                    <div className="font-medium">{supplier.companyName}</div>
                    <div className="text-sm text-muted-foreground">{supplier.address}</div>
                  </div>
                </TableCell>
                <TableCell>
                  <div>
                    <div className="font-medium">{supplier.contactPerson}</div>
                    <div className="text-sm text-muted-foreground flex items-center space-x-2">
                      <Mail className="h-3 w-3" />
                      <span>{supplier.email}</span>
                    </div>
                    <div className="text-sm text-muted-foreground flex items-center space-x-2">
                      <Phone className="h-3 w-3" />
                      <span>{supplier.phone}</span>
                    </div>
                  </div>
                </TableCell>
                <TableCell>
                  <Badge className={getSupplierTypeColor(supplier.supplierType)} variant="secondary">
                    {supplier.supplierType.toUpperCase()}
                  </Badge>
                </TableCell>
                <TableCell>{supplier.paymentTerms.toUpperCase()}</TableCell>
                <TableCell>{supplier.totalOrders}</TableCell>
                <TableCell>
                  <div className="flex items-center space-x-1">
                    <span className="font-medium">{supplier.rating}</span>
                    <span className="text-yellow-500">★</span>
                  </div>
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
            ))}
          </TableBody>
        </Table>
      </CardContent>
    );
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="font-[family-name:var(--font-space-grotesk)]">All Suppliers</CardTitle>
        <CardDescription>Complete supplier database</CardDescription>
      </CardHeader>
      {renderContent()}
    </Card>
  );
}