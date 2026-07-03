"use client"

import { useMemo, useState } from "react"
import useSWR from "swr"
import { Search, BookOpen } from "lucide-react"

import { getLibros } from "@/lib/services"
import { Header } from "@/components/dashboard/header"
import { AddLibroDialog } from "@/components/libros/add-libro-dialog"
import { Input } from "@/components/ui/input"
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
import { Empty, EmptyDescription, EmptyHeader, EmptyMedia, EmptyTitle } from "@/components/ui/empty"

export default function LibrosPage() {
  const { data, error, isLoading, mutate } = useSWR("libros", getLibros)
  const [query, setQuery] = useState("")

  const filtered = useMemo(() => {
    if (!data) return []
    const q = query.trim().toLowerCase()
    if (!q) return data
    return data.filter(
      (l) =>
        l.titulo.toLowerCase().includes(q) ||
        l.autor.toLowerCase().includes(q) ||
        l.isbn.toLowerCase().includes(q),
    )
  }, [data, query])

  return (
    <>
      <Header title="Libros" />
      <main className="flex-1 p-4 md:p-6">
        <div className="mb-4 flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
          <div className="relative w-full sm:max-w-xs">
            <Search className="pointer-events-none absolute top-1/2 left-3 size-4 -translate-y-1/2 text-muted-foreground" />
            <Input
              placeholder="Buscar por título, autor o ISBN"
              className="pl-9"
              value={query}
              onChange={(e) => setQuery(e.target.value)}
            />
          </div>
          <AddLibroDialog onCreated={() => mutate()} />
        </div>

        {error && (
          <Alert variant="destructive" className="mb-4">
            <AlertTitle>No se pudieron cargar los libros</AlertTitle>
            <AlertDescription>{error.message}</AlertDescription>
          </Alert>
        )}

        <Card className="overflow-hidden py-0">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>ISBN</TableHead>
                <TableHead>Título</TableHead>
                <TableHead>Autor</TableHead>
                <TableHead>Año</TableHead>
                <TableHead className="text-right">Disponible</TableHead>
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
                filtered.map((libro) => (
                  <TableRow key={libro.isbn}>
                    <TableCell className="font-mono text-xs">{libro.isbn}</TableCell>
                    <TableCell className="font-medium">{libro.titulo}</TableCell>
                    <TableCell className="text-muted-foreground">{libro.autor}</TableCell>
                    <TableCell className="text-muted-foreground">{libro.anioPublicacion}</TableCell>
                    <TableCell className="text-right">
                      {libro.estaDisponible ? (
                        <Badge className="bg-primary text-primary-foreground">
                          Disponible
                        </Badge>
                      ) : (
                        <Badge variant="destructive">Prestado</Badge>
                      )}
                    </TableCell>
                  </TableRow>
                ))}
            </TableBody>
          </Table>

          {!isLoading && !error && filtered.length === 0 && (
            <Empty className="py-12">
              <EmptyHeader>
                <EmptyMedia variant="icon">
                  <BookOpen />
                </EmptyMedia>
                <EmptyTitle>No hay libros</EmptyTitle>
                <EmptyDescription>
                  {query
                    ? "No se encontraron libros que coincidan con la búsqueda."
                    : "Agrega tu primer libro para comenzar."}
                </EmptyDescription>
              </EmptyHeader>
            </Empty>
          )}
        </Card>
      </main>
    </>
  )
}
