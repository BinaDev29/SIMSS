"use client";

import { useState, useEffect, type ReactNode } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Edit, Trash2, Mail, Phone, Loader2, AlertCircle } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { toast } from "react-hot-toast";
import { AuthService } from "@/lib/auth";

interface Employee {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  position: string;
  department: string;
  hireDate: string;
  salary: number;
  status: string;
}

interface EmployeesTableProps {
  refreshTrigger?: number;
  onEditEmployee?: (employee: Employee) => void;
}

const statusColors: { [key: string]: string } = {
  active: "bg-green-100 text-green-800",
  "on-leave": "bg-yellow-100 text-yellow-800",
  inactive: "bg-red-100 text-red-800",
};

const departmentColors: { [key: string]: string } = {
  Warehouse: "bg-blue-100 text-blue-800",
  Logistics: "bg-purple-100 text-purple-800",
  Finance: "bg-green-100 text-green-800",
  Administration: "bg-orange-100 text-orange-800",
  Sales: "bg-pink-100 text-pink-800",
  HR: "bg-indigo-100 text-indigo-800",
};

export function EmployeesTable({ refreshTrigger, onEditEmployee }: EmployeesTableProps) {
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchEmployees = async () => {
    try {
      setIsLoading(true);
      setError(null);
      const response = await fetch("/api/employees", {
        headers: AuthService.getAuthHeaders(),
      });
      const result = await response.json();

      if (response.ok && result.success) {
        setEmployees(result.data || []);
      } else {
        throw new Error(result.message || "Failed to fetch employees.");
      }
    } catch (err) {
      console.error("Error fetching employees:", err);
      setError("Failed to load employee data. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  const handleDeleteEmployee = async (employeeId: string) => {
    const confirmation = window.confirm("Are you sure you want to delete this employee?");
    if (!confirmation) {
      return;
    }

    try {
      const response = await fetch("/api/employees", {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
          ...AuthService.getAuthHeaders(),
        },
        body: JSON.stringify({ id: employeeId }),
      });

      const result = await response.json();

      if (response.ok && result.success) {
        toast.success("Employee deleted successfully.");
        setEmployees(employees.filter((emp) => emp.id !== employeeId));
      } else {
        throw new Error(result.message || "Failed to delete employee.");
      }
    } catch (err) {
      console.error("Error deleting employee:", err);
      toast.error(err instanceof Error ? err.message : "An unknown error occurred.");
    }
  };

  useEffect(() => {
    fetchEmployees();
  }, [refreshTrigger]);

  const renderContent = (): ReactNode => {
    if (isLoading) {
      return (
        <div className="text-center py-8">
          <Loader2 className="h-8 w-8 animate-spin text-accent mx-auto mb-4" />
          <p className="text-muted-foreground">Loading employee data...</p>
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

    if (employees.length === 0) {
      return (
        <div className="text-center py-8">
          <p className="text-muted-foreground">No employees found. Add new employees to get started.</p>
        </div>
      );
    }

    return (
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Employee ID</TableHead>
            <TableHead>Name</TableHead>
            <TableHead>Contact</TableHead>
            <TableHead>Position</TableHead>
            <TableHead>Department</TableHead>
            <TableHead>Hire Date</TableHead>
            <TableHead>Status</TableHead>
            <TableHead>Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {employees.map((employee) => (
            <TableRow key={employee.id}>
              <TableCell className="font-medium">{employee.id}</TableCell>
              <TableCell>
                <div>
                  <div className="font-medium">
                    {employee.firstName} {employee.lastName}
                  </div>
                  <div className="text-sm text-muted-foreground">${employee.salary.toLocaleString()}/year</div>
                </div>
              </TableCell>
              <TableCell>
                <div>
                  <div className="text-sm text-muted-foreground flex items-center space-x-2">
                    <Mail className="h-3 w-3" />
                    <span>{employee.email}</span>
                  </div>
                  <div className="text-sm text-muted-foreground flex items-center space-x-2">
                    <Phone className="h-3 w-3" />
                    <span>{employee.phone}</span>
                  </div>
                </div>
              </TableCell>
              <TableCell>{employee.position}</TableCell>
              <TableCell>
                <Badge className={departmentColors[employee.department] || "bg-gray-100 text-gray-800"} variant="secondary">
                  {employee.department}
                </Badge>
              </TableCell>
              <TableCell>{new Date(employee.hireDate).toLocaleDateString()}</TableCell>
              <TableCell>
                <Badge className={statusColors[employee.status] || "bg-gray-100 text-gray-800"} variant="secondary">
                  {employee.status.replace("-", " ")}
                </Badge>
              </TableCell>
              <TableCell>
                <div className="flex items-center space-x-2">
                  <Button variant="ghost" size="sm" onClick={() => onEditEmployee?.(employee)}>
                    <Edit className="h-4 w-4" />
                  </Button>
                  <Button variant="ghost" size="sm" onClick={() => handleDeleteEmployee(employee.id)}>
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
        <CardTitle className="font-[family-name:var(--font-space-grotesk)]">All Employees</CardTitle>
        <CardDescription>Complete employee database</CardDescription>
      </CardHeader>
      <CardContent>{renderContent()}</CardContent>
    </Card>
  );
}