"use client";

import type React from "react";
import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Textarea } from "@/components/ui/textarea";
import { Plus, Package, Loader2, CheckCircle, AlertCircle } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { AuthService } from "@/lib/auth";

// ለምድብ የTypeScript አይነት
interface Category {
  id: string;
  name: string;
}

interface ItemsFormProps {
  onItemAdded?: () => void;
  // ለውሂብ ማስተካከያ (editing)
  editingItem?: {
    id: string;
    itemName: string;
    description: string;
    category: string;
    unitPrice: string;
    minStockLevel: string;
    unit: string;
  } | null;
}

export function ItemsForm({ onItemAdded, editingItem }: ItemsFormProps) {
  const [formData, setFormData] = useState({
    itemName: "",
    description: "",
    category: "",
    unitPrice: "",
    minStockLevel: "",
    unit: "",
  });

  const [categories, setCategories] = useState<Category[]>([]);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [submitStatus, setSubmitStatus] = useState<"success" | "error" | null>(null);
  const [submitMessage, setSubmitMessage] = useState<string>("");

  useEffect(() => {
    // የመረጃ ማስተካከያ ካለ የቅጹን መረጃ ይሞላል
    if (editingItem) {
      setFormData({
        itemName: editingItem.itemName,
        description: editingItem.description,
        category: editingItem.category,
        unitPrice: editingItem.unitPrice.toString(),
        minStockLevel: editingItem.minStockLevel.toString(),
        unit: editingItem.unit,
      });
    }
    // ምድቦችን ከኤፒአይ ለማምጣት
    const fetchCategories = async () => {
      try {
        const response = await fetch("/api/categories", {
          headers: AuthService.getAuthHeaders(),
        });
        const result = await response.json();
        if (result.success) {
          setCategories(result.data);
        }
      } catch (err) {
        console.error("Failed to fetch categories:", err);
      }
    };
    fetchCategories();
  }, [editingItem]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    setSubmitStatus(null);
    setSubmitMessage("");

    const apiPath = editingItem ? `/api/items/${editingItem.id}` : "/api/items";
    const method = editingItem ? "PUT" : "POST";

    try {
      const response = await fetch(apiPath, {
        method,
        headers: {
          "Content-Type": "application/json",
          ...AuthService.getAuthHeaders(),
        },
        body: JSON.stringify(formData),
      });

      const result = await response.json();

      if (response.ok && result.success) {
        setSubmitStatus("success");
        setSubmitMessage(`Item ${editingItem ? "updated" : "added"} successfully!`);
        // ቅጹን ያጸዳል
        setFormData({
          itemName: "",
          description: "",
          category: "",
          unitPrice: "",
          minStockLevel: "",
          unit: "",
        });
        // የወላጅ ኮምፖነንቱን ያዘምናል
        onItemAdded?.();
      } else {
        setSubmitStatus("error");
        setSubmitMessage(result.message || `Failed to ${editingItem ? "update" : "add"} item.`);
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
          <CardTitle className="font-[family-name:var(--font-space-grotesk)]">{editingItem ? "Edit Item" : "Add New Item"}</CardTitle>
        </div>
        <CardDescription>{editingItem ? "Update item details" : "Create a new inventory item"}</CardDescription>
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
            <Label htmlFor="itemName">Item Name</Label>
            <Input
              id="itemName"
              placeholder="Enter item name"
              value={formData.itemName}
              onChange={(e) => handleInputChange("itemName", e.target.value)}
              required
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="description">Description</Label>
            <Textarea
              id="description"
              placeholder="Enter item description"
              value={formData.description}
              onChange={(e) => handleInputChange("description", e.target.value)}
              rows={3}
              required
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="category">Category</Label>
            <Select value={formData.category} onValueChange={(value) => handleInputChange("category", value)}>
              <SelectTrigger>
                <SelectValue placeholder="Select category" />
              </SelectTrigger>
              <SelectContent>
                {categories.map((category) => (
                  <SelectItem key={category.id} value={category.id}>{category.name}</SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          <div className="grid gap-4 md:grid-cols-2">
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
            <div className="space-y-2">
              <Label htmlFor="unit">Unit</Label>
              <Select value={formData.unit} onValueChange={(value) => handleInputChange("unit", value)}>
                <SelectTrigger>
                  <SelectValue placeholder="Select unit" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="pieces">Pieces</SelectItem>
                  <SelectItem value="kg">Kilograms</SelectItem>
                  <SelectItem value="liters">Liters</SelectItem>
                  <SelectItem value="meters">Meters</SelectItem>
                  <SelectItem value="boxes">Boxes</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>

          <div className="space-y-2">
            <Label htmlFor="minStockLevel">Minimum Stock Level</Label>
            <Input
              id="minStockLevel"
              type="number"
              placeholder="Enter minimum stock level"
              value={formData.minStockLevel}
              onChange={(e) => handleInputChange("minStockLevel", e.target.value)}
              required
            />
          </div>

          <Button type="submit" className="w-full bg-accent hover:bg-accent/90" disabled={isSubmitting}>
            {isSubmitting ? (
              <Loader2 className="h-4 w-4 animate-spin mr-2" />
            ) : (
              <Plus className="mr-2 h-4 w-4" />
            )}
            {isSubmitting ? "Saving..." : editingItem ? "Update Item" : "Add Item"}
          </Button>
        </form>
      </CardContent>
    </Card>
  );
}