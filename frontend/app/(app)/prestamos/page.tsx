"use client"

import { useState } from "react"
import useSWR from "swr"
import { toast } from "sonner"
import { ArrowLeftRight, RotateCcw } from "lucide-react"

import { getPrestamos, devolverPrestamo } from "@/lib/services"
import { ApiError } from "@/lib/api"
import type { Prestamo } from "@/lib/types"
import { Header } from "@/components/dashboard/header"
import { NuevoPrestamoDialog } from "@/components/prestamos/nuevo-prestamo-dialog"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
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
import { Spinner } from "@/components/ui/spinner"

function EstadoBadge({ activo }: { activo: boolean }) {
  if (!activo) return <Badge variant="secondary">Devuelto</Badge>
  return <Badge className="bg-primary text-primary-foreground">Activo</Badge>
}

export default function PrestamosPage() {
  const { data, error, isLoading, mutate } = useSWR("prestamos", getPrestamos)
  const [returningId, setReturningId] = useState<string | null>(null)

  async function handleDevolver(prestamo: Prestamo) {
    setReturningId(prestamo.id)
    try {
      // PUT /api/prestamos/devolver  (JWT attached automatically)
      const res = await devolverPrestamo({
        libroIsbn: prestamo.libroIsbn,
        nroSocio: prestamo.nroSocio,
      })
      if (res.multa) {
        toast.success(
          `Libro "${prestamo.libroTitulo}" devuelto con retraso. Multa generada: $${res.multa.montoTotal}`,
        )
      } else {
        toast.success(`Libro "${prestamo.libroTitulo}" devuelto correctamente`)
      }
      mutate()
    } catch (err) {
      if (err instanceof ApiError) {
        if (err.status === 404) {
          toast.error(err.message || "El préstamo no existe (404)")
        } else if (err.status === 400) {
          toast.error(err.message || "No se puede devolver este préstamo (400)")
        } else {
          toast.error(err.message || "No se pudo devolver el libro")
        }
      } else {
        toast.error("No se pudo devolver el libro")
      }
    } finally {
      setReturningId(null)
    }
  }

  return (
    <>
      <Header title="Préstamos" />
      <main className="flex-1 p-4 md:p-6">
        <div className="mb-4 flex items-center justify-end">
          <NuevoPrestamoDialog onCreated={() => mutate()} />
        </div>

        {error && (
          <Alert variant="destructive" className="mb-4">
            <AlertTitle>No se pudieron cargar los préstamos</AlertTitle>
            <AlertDescription>{error.message}</AlertDescription>
          </Alert>
        )}

        <Card className="overflow-hidden py-0">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>ID</TableHead>
                <TableHead>Libro</TableHead>
                <TableHead>Usuario</TableHead>
                <TableHead>Fecha</TableHead>
                <TableHead>Estado</TableHead>
                <TableHead className="text-right">Acción</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {isLoading &&
                Array.from({ length: 5 }).map((_, i) => (
                  <TableRow key={i}>
                    {Array.from({ length: 6 }).map((_, j) => (
                      <TableCell key={j}>
                        <Skeleton className="h-4 w-full" />
                      </TableCell>
                    ))}
                  </TableRow>
                ))}

              {!isLoading &&
                data?.map((prestamo) => {
                  const isReturned = !prestamo.activo
                  const isReturning = returningId === prestamo.id
                  return (
                    <TableRow key={prestamo.id}>
                      <TableCell className="font-mono text-xs">{prestamo.id}</TableCell>
                      <TableCell className="font-medium">{prestamo.libroTitulo}</TableCell>
                      <TableCell className="text-muted-foreground">
                        {prestamo.usuarioNombre}
                      </TableCell>
                      <TableCell className="text-muted-foreground">
                        {new Date(prestamo.fechaPrestamo).toLocaleDateString()}
                      </TableCell>
                      <TableCell>
                        <EstadoBadge activo={prestamo.activo} />
                      </TableCell>
                      <TableCell className="text-right">
                        <Button
                          variant="outline"
                          size="sm"
                          disabled={isReturned || isReturning}
                          onClick={() => handleDevolver(prestamo)}
                        >
                          {isReturning ? (
                            <Spinner data-icon="inline-start" />
                          ) : (
                            <RotateCcw data-icon="inline-start" />
                          )}
                          Devolver
                        </Button>
                      </TableCell>
                    </TableRow>
                  )
                })}
            </TableBody>
          </Table>

          {!isLoading && !error && (!data || data.length === 0) && (
            <Empty className="py-12">
              <EmptyHeader>
                <EmptyMedia variant="icon">
                  <ArrowLeftRight />
                </EmptyMedia>
                <EmptyTitle>No hay préstamos</EmptyTitle>
                <EmptyDescription>
                  Registra un nuevo préstamo para comenzar.
                </EmptyDescription>
              </EmptyHeader>
            </Empty>
          )}
        </Card>
      </main>
    </>
  )
}
