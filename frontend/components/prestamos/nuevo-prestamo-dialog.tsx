"use client"

import { useState } from "react"
import useSWR from "swr"
import { toast } from "sonner"
import { Plus } from "lucide-react"

import { createPrestamo, getUsuarios } from "@/lib/services"
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
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import { Spinner } from "@/components/ui/spinner"

export function NuevoPrestamoDialog({ onCreated }: { onCreated?: () => void }) {
  const [open, setOpen] = useState(false)
  const [loading, setLoading] = useState(false)
  const [isbn, setIsbn] = useState("")
  const [nroSocio, setNroSocio] = useState("")

  const { data: usuarios } = useSWR(open ? "usuarios" : null, getUsuarios)

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!nroSocio) {
      toast.error("Selecciona un usuario")
      return
    }
    setLoading(true)
    try {
      await createPrestamo({ isbn, nroSocio: Number(nroSocio) })
      toast.success("Préstamo registrado correctamente")
      setIsbn("")
      setNroSocio("")
      setOpen(false)
      onCreated?.()
    } catch (err) {
      const message =
        err instanceof ApiError ? err.message : "No se pudo registrar el préstamo"
      toast.error(message)
    } finally {
      setLoading(false)
    }
  }

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger render={<Button />}>
        <Plus data-icon="inline-start" />
        Nuevo préstamo
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Nuevo préstamo</DialogTitle>
          <DialogDescription>
            Registra el préstamo de un libro a un usuario.
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit} id="nuevo-prestamo-form">
          <FieldGroup>
            <Field>
              <FieldLabel htmlFor="prestamo-isbn">ISBN del libro</FieldLabel>
              <Input
                id="prestamo-isbn"
                required
                placeholder="978-..."
                value={isbn}
                onChange={(e) => setIsbn(e.target.value)}
              />
            </Field>
            <Field>
              <FieldLabel htmlFor="prestamo-usuario">Usuario</FieldLabel>
              <Select
                value={nroSocio}
                onValueChange={(v) => setNroSocio(v ?? "")}
              >
                <SelectTrigger id="prestamo-usuario" className="w-full">
                  <SelectValue placeholder="Selecciona un usuario" />
                </SelectTrigger>
                <SelectContent>
                  <SelectGroup>
                    {usuarios?.map((u) => (
                      <SelectItem key={u.id} value={String(u.nroSocio)}>
                        {u.nombre} {u.apellido}
                      </SelectItem>
                    ))}
                  </SelectGroup>
                </SelectContent>
              </Select>
            </Field>
          </FieldGroup>
        </form>
        <DialogFooter>
          <DialogClose render={<Button variant="outline" />}>Cancelar</DialogClose>
          <Button type="submit" form="nuevo-prestamo-form" disabled={loading}>
            {loading && <Spinner data-icon="inline-start" />}
            Registrar
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
