import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { FileText, DollarSign, Clock, CheckCircle } from "lucide-react"

const stats = [
  {
    title: "Total Invoices",
    value: "1,247",
    change: "+12%",
    changeType: "positive" as const,
    icon: FileText,
    description: "All time invoices",
  },
  {
    title: "Total Revenue",
    value: "$284,750",
    change: "+18%",
    changeType: "positive" as const,
    icon: DollarSign,
    description: "This month",
  },
  {
    title: "Pending Invoices",
    value: "23",
    change: "-5%",
    changeType: "negative" as const,
    icon: Clock,
    description: "Awaiting payment",
  },
  {
    title: "Paid Invoices",
    value: "1,224",
    change: "+15%",
    changeType: "positive" as const,
    icon: CheckCircle,
    description: "Successfully paid",
  },
]

export function InvoiceStats() {
  return (
    <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
      {stats.map((stat) => (
        <Card key={stat.title}>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">{stat.title}</CardTitle>
            <stat.icon className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold font-[family-name:var(--font-space-grotesk)]">{stat.value}</div>
            <div className="flex items-center space-x-2 text-xs text-muted-foreground">
              <span className={`font-medium ${stat.changeType === "positive" ? "text-green-600" : "text-red-600"}`}>
                {stat.change}
              </span>
              <span>from last month</span>
            </div>
            <p className="text-xs text-muted-foreground mt-1">{stat.description}</p>
          </CardContent>
        </Card>
      ))}
    </div>
  )
}
