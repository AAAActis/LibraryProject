"use client"

import { useEffect } from "react"
import { useRouter } from "next/navigation"
import { Spinner } from "@/components/ui/spinner"

export default function Page() {
  const router = useRouter()

  useEffect(() => {
    // Siempre arranca en login: un token en localStorage no garantiza que
    // siga siendo válido (expira a la hora), así que no lo usamos para
    // decidir a dónde mandar al usuario al entrar a "/".
    router.replace("/login")
  }, [router])

  return (
    <main className="flex min-h-screen items-center justify-center bg-muted/40">
      <Spinner className="size-6 text-muted-foreground" />
    </main>
  )
}
