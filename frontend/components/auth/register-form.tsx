"use client"

import { useState } from "react"
import { useRouter } from "next/navigation"
import Link from "next/link"
import { toast } from "sonner"
import { BookMarked, User, Mail, Phone, IdCard, Lock } from "lucide-react"

import { register } from "@/lib/services"
import { ApiError } from "@/lib/api"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Field, FieldGroup, FieldLabel } from "@/components/ui/field"
import { Spinner } from "@/components/ui/spinner"

export function RegisterForm() {
  const router = useRouter()
  const [nombre, setNombre] = useState("")
  const [apellido, setApellido] = useState("")
  const [email, setEmail] = useState("")
  const [telefono, setTelefono] = useState("")
  const [documento, setDocumento] = useState("")
  const [password, setPassword] = useState("")
  const [loading, setLoading] = useState(false)

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    setLoading(true)
    try {
      await register({ nombre, apellido, email, telefono, documento, password })
      toast.success("Cuenta creada. Ahora puedes iniciar sesión.")
      router.push("/login")
    } catch (err) {
      const message =
        err instanceof ApiError ? err.message : "No se pudo crear la cuenta"
      toast.error(message)
    } finally {
      setLoading(false)
    }
  }

  return (
    <Card className="w-full max-w-sm">
      <CardHeader className="items-center text-center">
        <div className="mb-2 flex size-12 items-center justify-center rounded-xl bg-primary text-primary-foreground">
          <BookMarked className="size-6" />
        </div>
        <CardTitle className="text-xl">Crear cuenta</CardTitle>
        <CardDescription>Regístrate en LibraryProject</CardDescription>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit}>
          <FieldGroup>
            <Field>
              <FieldLabel htmlFor="nombre">Nombre</FieldLabel>
              <div className="relative">
                <User className="pointer-events-none absolute top-1/2 left-3 size-4 -translate-y-1/2 text-muted-foreground" />
                <Input
                  id="nombre"
                  required
                  placeholder="Nombre"
                  className="pl-9"
                  value={nombre}
                  onChange={(e) => setNombre(e.target.value)}
                />
              </div>
            </Field>
            <Field>
              <FieldLabel htmlFor="apellido">Apellido</FieldLabel>
              <div className="relative">
                <User className="pointer-events-none absolute top-1/2 left-3 size-4 -translate-y-1/2 text-muted-foreground" />
                <Input
                  id="apellido"
                  required
                  placeholder="Apellido"
                  className="pl-9"
                  value={apellido}
                  onChange={(e) => setApellido(e.target.value)}
                />
              </div>
            </Field>
            <Field>
              <FieldLabel htmlFor="email">Email</FieldLabel>
              <div className="relative">
                <Mail className="pointer-events-none absolute top-1/2 left-3 size-4 -translate-y-1/2 text-muted-foreground" />
                <Input
                  id="email"
                  type="email"
                  autoComplete="email"
                  required
                  placeholder="tu@correo.com"
                  className="pl-9"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                />
              </div>
            </Field>
            <Field>
              <FieldLabel htmlFor="telefono">Teléfono</FieldLabel>
              <div className="relative">
                <Phone className="pointer-events-none absolute top-1/2 left-3 size-4 -translate-y-1/2 text-muted-foreground" />
                <Input
                  id="telefono"
                  required
                  placeholder="351-123456"
                  className="pl-9"
                  value={telefono}
                  onChange={(e) => setTelefono(e.target.value)}
                />
              </div>
            </Field>
            <Field>
              <FieldLabel htmlFor="documento">Documento</FieldLabel>
              <div className="relative">
                <IdCard className="pointer-events-none absolute top-1/2 left-3 size-4 -translate-y-1/2 text-muted-foreground" />
                <Input
                  id="documento"
                  required
                  placeholder="99123456"
                  className="pl-9"
                  value={documento}
                  onChange={(e) => setDocumento(e.target.value)}
                />
              </div>
            </Field>
            <Field>
              <FieldLabel htmlFor="password">Contraseña</FieldLabel>
              <div className="relative">
                <Lock className="pointer-events-none absolute top-1/2 left-3 size-4 -translate-y-1/2 text-muted-foreground" />
                <Input
                  id="password"
                  type="password"
                  autoComplete="new-password"
                  required
                  placeholder="••••••••"
                  className="pl-9"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
              </div>
            </Field>
            <Button type="submit" className="w-full" disabled={loading}>
              {loading && <Spinner data-icon="inline-start" />}
              Registrarse
            </Button>
            <p className="text-center text-sm text-muted-foreground">
              ¿Ya tienes cuenta?{" "}
              <Link
                href="/login"
                className="font-medium text-primary underline-offset-4 hover:underline"
              >
                Inicia sesión
              </Link>
            </p>
          </FieldGroup>
        </form>
      </CardContent>
    </Card>
  )
}
