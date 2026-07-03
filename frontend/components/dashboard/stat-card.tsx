import type { LucideIcon } from "lucide-react"

import { cn } from "@/lib/utils"
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Skeleton } from "@/components/ui/skeleton"

export function StatCard({
  label,
  value,
  icon: Icon,
  loading,
  accent,
}: {
  label: string
  value: number | undefined
  icon: LucideIcon
  loading?: boolean
  accent?: boolean
}) {
  return (
    <Card>
      <CardHeader className="flex-row items-center justify-between gap-2 pb-2">
        <CardTitle className="text-sm font-medium text-muted-foreground">
          {label}
        </CardTitle>
        <div
          className={cn(
            "flex size-9 items-center justify-center rounded-lg",
            accent
              ? "bg-primary text-primary-foreground"
              : "bg-primary/10 text-primary",
          )}
        >
          <Icon className="size-5" />
        </div>
      </CardHeader>
      <CardContent>
        {loading ? (
          <Skeleton className="h-8 w-16" />
        ) : (
          <p className="text-3xl font-semibold tracking-tight">{value ?? 0}</p>
        )}
      </CardContent>
    </Card>
  )
}
