"use client"

import { useEffect } from "react"
import { useRouter } from "next/navigation"
import { isAuthenticated } from "@/lib/api"
import { Spinner } from "@/components/ui/spinner"

export default function Page() {
  const router = useRouter()

  useEffect(() => {
    router.replace(isAuthenticated() ? "/dashboard" : "/login")
  }, [router])

  return (
    <main className="flex min-h-screen items-center justify-center bg-muted/40">
      <Spinner className="size-6 text-muted-foreground" />
    </main>
  )
}
