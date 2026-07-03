"use client"

import { useEffect, useState } from "react"
import { useRouter } from "next/navigation"

import { isAuthenticated } from "@/lib/api"
import { Sidebar } from "@/components/dashboard/sidebar"
import { Spinner } from "@/components/ui/spinner"

export default function AppLayout({ children }: { children: React.ReactNode }) {
  const router = useRouter()
  const [ready, setReady] = useState(false)

  useEffect(() => {
    if (!isAuthenticated()) {
      router.replace("/login")
    } else {
      setReady(true)
    }
  }, [router])

  if (!ready) {
    return (
      <div className="flex min-h-screen items-center justify-center bg-muted/40">
        <Spinner className="size-6 text-muted-foreground" />
      </div>
    )
  }

  return (
    <div className="flex min-h-screen bg-muted/40">
      <Sidebar />
      <div className="flex min-w-0 flex-1 flex-col">{children}</div>
    </div>
  )
}
