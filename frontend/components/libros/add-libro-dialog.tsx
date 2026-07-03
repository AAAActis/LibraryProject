"use client"

import { useState } from "react"
import { toast } from "sonner"
import { Plus } from "lucide-react"

import { createLibro } from "@/lib/services"
import { ApiError } from "@/lib/api"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import { Field, FieldGroup, FieldLabel } from "@/components/ui/field"
import { Spinner } from "@/components/ui/spinner"

export function AddLibroDialog({ onCreated }: { onCreated?: () => void }) {
  const [open, setOpen] = useState(false)
  const [loading, setLoading] = useState(false)
  const [form, setForm] = useState({
    isbn: "",
    titulo: "",
    autor: "",
    anioPublicacion: "",
    cantPaginas: "",
  })

  function update(key: keyof typeof form, value: string) {
    setForm((f) => ({ ...f, [key]: value }))
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    setLoading(true)
    try {
      await createLibro({
        isbn: form.isbn,
        titulo: form.titulo,
        autor: form.autor,
        anioPublicacion: Number(form.anioPublicacion),
        cantPaginas: Number(form.cantPaginas),
      })
      toast.success("Libro agregado correctamente")
      setForm({ isbn: "", titulo: "", autor: "", anioPublicacion: "", cantPaginas: "" })
      setOpen(false)
      onCreated?.()
    } catch (err) {
      const message =
        err instanceof ApiError ? err.message : "No se pudo agregar el libro"
      toast.error(message)
    } finally {
      setLoading(false)
    }
  }

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger render={<Button />}>
        <Plus data-icon="inline-start" />
        Agregar libro
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Agregar libro</DialogTitle>
          <DialogDescription>
            Completa la información del nuevo libro.
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit} id="add-libro-form">
          <FieldGroup>
            <Field>
              <FieldLabel htmlFor="isbn">ISBN</FieldLabel>
              <Input
                id="isbn"
                required
                value={form.isbn}
                onChange={(e) => update("isbn", e.target.value)}
              />
            </Field>
            <Field>
              <FieldLabel htmlFor="titulo">Título</FieldLabel>
              <Input
                id="titulo"
                required
                value={form.titulo}
                onChange={(e) => update("titulo", e.target.value)}
              />
            </Field>
            <Field>
              <FieldLabel htmlFor="autor">Autor</FieldLabel>
              <Input
                id="autor"
                required
                value={form.autor}
                onChange={(e) => update("autor", e.target.value)}
              />
            </Field>
            <Field>
              <FieldLabel htmlFor="anio">Año</FieldLabel>
              <Input
                id="anio"
                type="number"
                required
                value={form.anioPublicacion}
                onChange={(e) => update("anioPublicacion", e.target.value)}
              />
            </Field>
            <Field>
              <FieldLabel htmlFor="cantPaginas">Cantidad de páginas</FieldLabel>
              <Input
                id="cantPaginas"
                type="number"
                required
                value={form.cantPaginas}
                onChange={(e) => update("cantPaginas", e.target.value)}
              />
            </Field>
          </FieldGroup>
        </form>
        <DialogFooter>
          <DialogClose render={<Button variant="outline" />}>Cancelar</DialogClose>
          <Button type="submit" form="add-libro-form" disabled={loading}>
            {loading && <Spinner data-icon="inline-start" />}
            Guardar
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
