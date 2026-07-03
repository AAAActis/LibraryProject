"use client"

import useSWR from "swr"
import { CircleDollarSign } from "lucide-react"

import { getMultas } from "@/lib/services"
import { Header } from "@/components/dashboard/header"
import { Card } from "@/components/ui/card"
import { Skeleton } from "@/components/ui/skeleton"
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert"
import {
  Empty,
  EmptyDescription,
  EmptyHeader,
  EmptyMedia,
  EmptyTitle,
} from "@/components/ui/empty"

export default function MultasPage() {
  const { data, error, isLoading } = useSWR("multas", getMultas)

  return (
    <>
      <Header title="Multas" />
      <main className="flex-1 p-4 md:p-6">
        {error && (
          <Alert variant="destructive" className="mb-4">
            <AlertTitle>No se pudieron cargar las multas</AlertTitle>
            <AlertDescription>{error.message}</AlertDescription>
          </Alert>
        )}

        <Card className="overflow-hidden py-0">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Libro</TableHead>
                <TableHead>Usuario</TableHead>
                <TableHead>Días de retraso</TableHead>
                <TableHead>Monto</TableHead>
                <TableHead className="text-right">Fecha</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {isLoading &&
                Array.from({ length: 5 }).map((_, i) => (
                  <TableRow key={i}>
                    {Array.from({ length: 5 }).map((_, j) => (
                      <TableCell key={j}>
                        <Skeleton className="h-4 w-full" />
                      </TableCell>
                    ))}
                  </TableRow>
                ))}

              {!isLoading &&
                data?.map((multa) => (
                  <TableRow key={multa.id}>
                    <TableCell className="font-medium">{multa.libroTitulo}</TableCell>
                    <TableCell className="text-muted-foreground">
                      {multa.usuarioNombre}
                    </TableCell>
                    <TableCell className="text-muted-foreground">
                      {multa.diasRetraso}
                    </TableCell>
                    <TableCell className="text-muted-foreground">
                      ${multa.montoTotal}
                    </TableCell>
                    <TableCell className="text-right text-muted-foreground">
                      {new Date(multa.fechaGenerada).toLocaleDateString()}
                    </TableCell>
                  </TableRow>
                ))}
            </TableBody>
          </Table>

          {!isLoading && !error && (!data || data.length === 0) && (
            <Empty className="py-16">
              <EmptyHeader>
                <EmptyMedia variant="icon">
                  <CircleDollarSign />
                </EmptyMedia>
                <EmptyTitle>Sin multas pendientes</EmptyTitle>
                <EmptyDescription>
                  Las multas por préstamos vencidos aparecerán aquí.
                </EmptyDescription>
              </EmptyHeader>
            </Empty>
          )}
        </Card>
      </main>
    </>
  )
}
