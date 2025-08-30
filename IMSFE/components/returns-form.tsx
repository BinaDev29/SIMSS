"use client";

import type React from "react";
import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Textarea } from "@/components/ui/textarea";
import { RotateCcw, Loader2, AlertCircle, CheckCircle } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { AuthService } from "@/lib/auth";

// ለዕቃዎች እና ለደንበኞች የTypeScript አይነቶች
interface Item {
  id: string;
  name: string;
}

interface Customer {
  id: string;
  name: string;
}

interface ReturnsFormProps {
  onReturnProcessed?: () => void;
}

export function ReturnsForm({ onReturnProcessed }: ReturnsFormProps) {
  const [formData, setFormData] = useState({
    returnDate: "",
    itemId: "",
    customerId: "",
    quantity: "",
    reason: "",
    condition: "",
    notes: "",
  });

  const [items, setItems] = useState<Item[]>([]);
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isLoadingData, setIsLoadingData] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [submitStatus, setSubmitStatus] = useState<"success" | "error" | null>(null);
  const [submitMessage, setSubmitMessage] = useState<string>("");

  useEffect(() => {
    const fetchData = async () => {
      try {
        setIsLoadingData(true);
        const [itemsResponse, customersResponse] = await Promise.all([
          fetch("/api/items", { headers: AuthService.getAuthHeaders() }),
          fetch("/api/customers", { headers: AuthService.getAuthHeaders() }),
        ]);

        const itemsResult = await itemsResponse.json();
        const customersResult = await customersResponse.json();

        if (itemsResponse.ok && itemsResult.success) {
          setItems(itemsResult.data);
        } else {
          throw new Error(itemsResult.message || "Failed to fetch items.");
        }

        if (customersResponse.ok && customersResult.success) {
          setCustomers(customersResult.data);
        } else {
          throw new Error(customersResult.message || "Failed to fetch customers.");
        }
      } catch (err) {
        console.error("Error fetching form data:", err);
        setError("Failed to load form data. Please try again.");
      } finally {
        setIsLoadingData(false);
      }
    };
    fetchData();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    setSubmitStatus(null);
    setSubmitMessage("");
    setError(null);

    try {
      const response = await fetch("/api/returns", {
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
        setSubmitMessage("Return transaction processed successfully!");
        // ቅጹን ያጸዳል
        setFormData({
          returnDate: "",
          itemId: "",
          customerId: "",
          quantity: "",
          reason: "",
          condition: "",
          notes: "",
        });
        // የወላጅ ኮምፖነንቱን ያዘምናል (ለምሳሌ ጠረጴዛውን)
        onReturnProcessed?.();
      } else {
        setSubmitStatus("error");
        setSubmitMessage(result.message || "Failed to process return transaction.");
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

  const renderContent = () => {
    if (isLoadingData) {
      return (
        <CardContent className="p-6">
          <div className="text-center py-10">
            <Loader2 className="h-8 w-8 animate-spin text-accent mx-auto mb-4" />
            <p className="text-muted-foreground">Loading form data...</p>
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
    
    return (
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
              <Label htmlFor="returnDate">Return Date</Label>
              <Input
                id="returnDate"
                type="date"
                value={formData.returnDate}
                onChange={(e) => handleInputChange("returnDate", e.target.value)}
                required
                disabled={isSubmitting}
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
                disabled={isSubmitting}
              />
            </div>
          </div>

          <div className="space-y-2">
            <Label htmlFor="itemId">Item</Label>
            <Select value={formData.itemId} onValueChange={(value) => handleInputChange("itemId", value)} disabled={isSubmitting}>
              <SelectTrigger>
                <SelectValue placeholder="Select returned item" />
              </SelectTrigger>
              <SelectContent>
                {items.map((item) => (
                  <SelectItem key={item.id} value={item.id}>{item.name}</SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          <div className="space-y-2">
            <Label htmlFor="customerId">Customer</Label>
            <Select value={formData.customerId} onValueChange={(value) => handleInputChange("customerId", value)} disabled={isSubmitting}>
              <SelectTrigger>
                <SelectValue placeholder="Select customer" />
              </SelectTrigger>
              <SelectContent>
                {customers.map((customer) => (
                  <SelectItem key={customer.id} value={customer.id}>{customer.name}</SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          <div className="grid gap-4 md:grid-cols-2">
            <div className="space-y-2">
              <Label htmlFor="reason">Return Reason</Label>
              <Select value={formData.reason} onValueChange={(value) => handleInputChange("reason", value)} disabled={isSubmitting}>
                <SelectTrigger>
                  <SelectValue placeholder="Select reason" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="defective">Defective Product</SelectItem>
                  <SelectItem value="wrong-item">Wrong Item Delivered</SelectItem>
                  <SelectItem value="damaged">Damaged in Transit</SelectItem>
                  <SelectItem value="not-needed">No Longer Needed</SelectItem>
                  <SelectItem value="other">Other</SelectItem>
                </SelectContent>
              </Select>
            </div>
            <div className="space-y-2">
              <Label htmlFor="condition">Item Condition</Label>
              <Select value={formData.condition} onValueChange={(value) => handleInputChange("condition", value)} disabled={isSubmitting}>
                <SelectTrigger>
                  <SelectValue placeholder="Select condition" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="new">Like New</SelectItem>
                  <SelectItem value="good">Good Condition</SelectItem>
                  <SelectItem value="fair">Fair Condition</SelectItem>
                  <SelectItem value="poor">Poor Condition</SelectItem>
                  <SelectItem value="damaged">Damaged</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>

          <div className="space-y-2">
            <Label htmlFor="notes">Notes (Optional)</Label>
            <Textarea
              id="notes"
              placeholder="Additional details about the return"
              value={formData.notes}
              onChange={(e) => handleInputChange("notes", e.target.value)}
              rows={3}
              disabled={isSubmitting}
            />
          </div>

          <Button type="submit" className="w-full bg-accent hover:bg-accent/90" disabled={isSubmitting}>
            {isSubmitting ? (
              <>
                <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                Processing...
              </>
            ) : (
              "Process Return"
            )}
          </Button>
        </form>
      </CardContent>
    );
  };
  
  return (
    <Card>
      <CardHeader>
        <div className="flex items-center space-x-2">
          <RotateCcw className="h-5 w-5 text-accent" />
          <CardTitle className="font-[family-name:var(--font-space-grotesk)]">New Return Transaction</CardTitle>
        </div>
        <CardDescription>Process returned items from customers</CardDescription>
      </CardHeader>
      {renderContent()}
    </Card>
  );
}