"use client";

import React, { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Textarea } from "@/components/ui/textarea";
import { Package, Loader2, CheckCircle, AlertCircle } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { AuthService } from "@/lib/auth";

// ለዕቃ፣ አቅራቢ እና ማከማቻ መረጃ የTypeScript አይነት
interface Item {
  id: string;
  name: string;
}
interface Supplier {
  id: string;
  name: string;
}
interface Godown {
  id: string;
  name: string;
}

export function InwardsForm() {
  const [formData, setFormData] = useState({
    transactionDate: "",
    itemId: "",
    supplierId: "",
    godownId: "",
    quantity: "",
    unitPrice: "",
    notes: "",
  });

  const [items, setItems] = useState<Item[]>([]);
  const [suppliers, setSuppliers] = useState<Supplier[]>([]);
  const [godowns, setGodowns] = useState<Godown[]>([]);

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [submitStatus, setSubmitStatus] = useState<"success" | "error" | null>(null);
  const [submitMessage, setSubmitMessage] = useState<string>("");

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [itemsRes, suppliersRes, godownsRes] = await Promise.all([
          fetch("/api/items", { headers: AuthService.getAuthHeaders() }),
          fetch("/api/suppliers", { headers: AuthService.getAuthHeaders() }),
          fetch("/api/godowns", { headers: AuthService.getAuthHeaders() }),
        ]);

        const [itemsData, suppliersData, godownsData] = await Promise.all([
          itemsRes.json(),
          suppliersRes.json(),
          godownsRes.json(),
        ]);

        if (itemsData.success) setItems(itemsData.data);
        if (suppliersData.success) setSuppliers(suppliersData.data);
        if (godownsData.success) setGodowns(godownsData.data);
      } catch (err) {
        console.error("Failed to fetch form options:", err);
      }
    };

    fetchData();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    setSubmitStatus(null);
    setSubmitMessage("");

    try {
      const response = await fetch("/api/transactions/inwards", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          ...AuthService.getAuthHeaders(),
        },
        body: JSON.stringify(formData),
      });

      const result = await response.json();

      if (response.ok && result.success) {
        setSubmitStatus("success");
        setSubmitMessage("Inward transaction recorded successfully!");
        setFormData({
          transactionDate: "",
          itemId: "",
          supplierId: "",
          godownId: "",
          quantity: "",
          unitPrice: "",
          notes: "",
        });
      } else {
        setSubmitStatus("error");
        setSubmitMessage(result.message || "Failed to record transaction.");
      }
    } catch (err) {
      setSubmitStatus("error");
      setSubmitMessage("Network error occurred. Please try again.");
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleInputChange = (field: string, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  return (
    <Card>
      <CardHeader>
        <div className="flex items-center space-x-2">
          <Package className="h-5 w-5 text-accent" />
          <CardTitle className="font-[family-name:var(--font-space-grotesk)]">New Inward Transaction</CardTitle>
        </div>
        <CardDescription>Record items received from suppliers</CardDescription>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit} className="space-y-4">
          {submitStatus === "success" && (
            <Alert variant="default" className="bg-green-100 border-green-400 text-green-700">
              <CheckCircle className="h-4 w-4" />
              <AlertTitle>Success</AlertTitle>
              <AlertDescription>{submitMessage}</AlertDescription>
            </Alert>
          )}
          {submitStatus === "error" && (
            <Alert variant="destructive">
              <AlertCircle className="h-4 w-4" />
              <AlertTitle>Error</AlertTitle>
              <AlertDescription>{submitMessage}</AlertDescription>
            </Alert>
          )}

          <div className="grid gap-4 md:grid-cols-2">
            <div className="space-y-2">
              <Label htmlFor="transactionDate">Transaction Date</Label>
              <Input
                id="transactionDate"
                type="date"
                value={formData.transactionDate}
                onChange={(e) => handleInputChange("transactionDate", e.target.value)}
                required
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="quantity">Quantity</Label>
              <Input
                id="quantity"
                type="number"
                placeholder="Enter quantity"
                value={formData.quantity}
                onChange={(e) => handleInputChange("quantity", e.target.value)}
                required
              />
            </div>
          </div>

          <div className="space-y-2">
            <Label htmlFor="itemId">Item</Label>
            <Select value={formData.itemId} onValueChange={(value) => handleInputChange("itemId", value)} required>
              <SelectTrigger>
                <SelectValue placeholder="Select an item" />
              </SelectTrigger>
              <SelectContent>
                {items.map((item) => (
                  <SelectItem key={item.id} value={item.id}>{item.name}</SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          <div className="space-y-2">
            <Label htmlFor="supplierId">Supplier</Label>
            <Select value={formData.supplierId} onValueChange={(value) => handleInputChange("supplierId", value)} required>
              <SelectTrigger>
                <SelectValue placeholder="Select a supplier" />
              </SelectTrigger>
              <SelectContent>
                {suppliers.map((supplier) => (
                  <SelectItem key={supplier.id} value={supplier.id}>{supplier.name}</SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          <div className="grid gap-4 md:grid-cols-2">
            <div className="space-y-2">
              <Label htmlFor="godownId">Godown</Label>
              <Select value={formData.godownId} onValueChange={(value) => handleInputChange("godownId", value)} required>
                <SelectTrigger>
                  <SelectValue placeholder="Select godown" />
                </SelectTrigger>
                <SelectContent>
                  {godowns.map((godown) => (
                    <SelectItem key={godown.id} value={godown.id}>{godown.name}</SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
            <div className="space-y-2">
              <Label htmlFor="unitPrice">Unit Price</Label>
              <Input
                id="unitPrice"
                type="number"
                step="0.01"
                placeholder="0.00"
                value={formData.unitPrice}
                onChange={(e) => handleInputChange("unitPrice", e.target.value)}
                required
              />
            </div>
          </div>

          <div className="space-y-2">
            <Label htmlFor="notes">Notes (Optional)</Label>
            <Textarea
              id="notes"
              placeholder="Additional notes about this transaction"
              value={formData.notes}
              onChange={(e) => handleInputChange("notes", e.target.value)}
              rows={3}
            />
          </div>

          <Button type="submit" className="w-full bg-accent hover:bg-accent/90" disabled={isSubmitting}>
            {isSubmitting ? (
              <Loader2 className="h-4 w-4 animate-spin mr-2" />
            ) : (
              <Package className="h-4 w-4 mr-2" />
            )}
            {isSubmitting ? "Recording..." : "Record Inward Transaction"}
          </Button>
        </form>
      </CardContent>
    </Card>
  );
}