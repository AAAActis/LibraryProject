"use client"

import useSWR from "swr"
import { Users } from "lucide-react"

import { getUsuarios } from "@/lib/services"
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

export default function UsuariosPage() {
  const { data, error, isLoading } = useSWR("usuarios", getUsuarios)

  return (
    <>
      <Header title="Usuarios" />
      <main className="flex-1 p-4 md:p-6">
        {error && (
          <Alert variant="destructive" className="mb-4">
            <AlertTitle>No se pudieron cargar los usuarios</AlertTitle>
            <AlertDescription>{error.message}</AlertDescription>
          </Alert>
        )}
        <Card className="overflow-hidden py-0">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Nombre</TableHead>
                <TableHead>Apellido</TableHead>
                <TableHead>Email</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {isLoading &&
                Array.from({ length: 5 }).map((_, i) => (
                  <TableRow key={i}>
                    {Array.from({ length: 3 }).map((_, j) => (
                      <TableCell key={j}>
                        <Skeleton className="h-4 w-full" />
                      </TableCell>
                    ))}
                  </TableRow>
                ))}
              {!isLoading &&
                data?.map((u) => (
                  <TableRow key={u.id}>
                    <TableCell className="font-medium">{u.nombre}</TableCell>
                    <TableCell>{u.apellido}</TableCell>
                    <TableCell className="text-muted-foreground">{u.email}</TableCell>
                  </TableRow>
                ))}
            </TableBody>
          </Table>
          {!isLoading && !error && (!data || data.length === 0) && (
            <Empty className="py-12">
              <EmptyHeader>
                <EmptyMedia variant="icon">
                  <Users />
                </EmptyMedia>
                <EmptyTitle>No hay usuarios</EmptyTitle>
                <EmptyDescription>
                  Aún no hay usuarios registrados.
                </EmptyDescription>
              </EmptyHeader>
            </Empty>
          )}
        </Card>
      </main>
    </>
  )
}
