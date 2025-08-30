"use client";

import type React from "react";
import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Textarea } from "@/components/ui/textarea";
import { Plus, Truck, Loader2, AlertCircle, CheckCircle } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { AuthService } from "@/lib/auth";

interface SuppliersFormProps {
  onSupplierAdded?: () => void;
}

export function SuppliersForm({ onSupplierAdded }: SuppliersFormProps) {
  const [formData, setFormData] = useState({
    companyName: "",
    contactPerson: "",
    email: "",
    phone: "",
    address: "",
    supplierType: "",
    paymentTerms: "",
    taxId: "",
  });

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [submitStatus, setSubmitStatus] = useState<"success" | "error" | null>(null);
  const [submitMessage, setSubmitMessage] = useState<string>("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    setSubmitStatus(null);
    setSubmitMessage("");

    try {
      const response = await fetch("/api/suppliers", {
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
        setSubmitMessage("Supplier added successfully!");
        // ቅጹን ያጸዳል
        setFormData({
          companyName: "",
          contactPerson: "",
          email: "",
          phone: "",
          address: "",
          supplierType: "",
          paymentTerms: "",
          taxId: "",
        });
        onSupplierAdded?.();
      } else {
        setSubmitStatus("error");
        setSubmitMessage(result.message || "Failed to add supplier. Please check the data.");
      }
    } catch (err) {
      console.error("Error adding supplier:", err);
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
          <Truck className="h-5 w-5 text-accent" />
          <CardTitle className="font-[family-name:var(--font-space-grotesk)]">Add New Supplier</CardTitle>
        </div>
        <CardDescription>Register a new supplier</CardDescription>
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

          <div className="space-y-2">
            <Label htmlFor="companyName">Company Name</Label>
            <Input
              id="companyName"
              placeholder="Enter company name"
              value={formData.companyName}
              onChange={(e) => handleInputChange("companyName", e.target.value)}
              required
              disabled={isSubmitting}
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="contactPerson">Contact Person</Label>
            <Input
              id="contactPerson"
              placeholder="Enter contact person name"
              value={formData.contactPerson}
              onChange={(e) => handleInputChange("contactPerson", e.target.value)}
              required
              disabled={isSubmitting}
            />
          </div>

          <div className="grid gap-4 md:grid-cols-2">
            <div className="space-y-2">
              <Label htmlFor="email">Email</Label>
              <Input
                id="email"
                type="email"
                placeholder="Enter email address"
                value={formData.email}
                onChange={(e) => handleInputChange("email", e.target.value)}
                required
                disabled={isSubmitting}
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="phone">Phone</Label>
              <Input
                id="phone"
                type="tel"
                placeholder="Enter phone number"
                value={formData.phone}
                onChange={(e) => handleInputChange("phone", e.target.value)}
                required
                disabled={isSubmitting}
              />
            </div>
          </div>

          <div className="space-y-2">
            <Label htmlFor="address">Address</Label>
            <Textarea
              id="address"
              placeholder="Enter complete address"
              value={formData.address}
              onChange={(e) => handleInputChange("address", e.target.value)}
              rows={3}
              required
              disabled={isSubmitting}
            />
          </div>

          <div className="grid gap-4 md:grid-cols-2">
            <div className="space-y-2">
              <Label htmlFor="supplierType">Supplier Type</Label>
              <Select value={formData.supplierType} onValueChange={(value) => handleInputChange("supplierType", value)} disabled={isSubmitting}>
                <SelectTrigger>
                  <SelectValue placeholder="Select type" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="manufacturer">Manufacturer</SelectItem>
                  <SelectItem value="distributor">Distributor</SelectItem>
                  <SelectItem value="wholesaler">Wholesaler</SelectItem>
                  <SelectItem value="service">Service Provider</SelectItem>
                </SelectContent>
              </Select>
            </div>
            <div className="space-y-2">
              <Label htmlFor="paymentTerms">Payment Terms</Label>
              <Select value={formData.paymentTerms} onValueChange={(value) => handleInputChange("paymentTerms", value)} disabled={isSubmitting}>
                <SelectTrigger>
                  <SelectValue placeholder="Select terms" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="net-30">Net 30</SelectItem>
                  <SelectItem value="net-60">Net 60</SelectItem>
                  <SelectItem value="net-90">Net 90</SelectItem>
                  <SelectItem value="cod">Cash on Delivery</SelectItem>
                  <SelectItem value="prepaid">Prepaid</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>

          <div className="space-y-2">
            <Label htmlFor="taxId">Tax ID (Optional)</Label>
            <Input
              id="taxId"
              placeholder="Enter tax identification number"
              value={formData.taxId}
              onChange={(e) => handleInputChange("taxId", e.target.value)}
              disabled={isSubmitting}
            />
          </div>

          <Button type="submit" className="w-full bg-accent hover:bg-accent/90" disabled={isSubmitting}>
            {isSubmitting ? (
              <>
                <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                Adding Supplier...
              </>
            ) : (
              <>
                <Plus className="mr-2 h-4 w-4" />
                Add Supplier
              </>
            )}
          </Button>
        </form>
      </CardContent>
    </Card>
  );
}