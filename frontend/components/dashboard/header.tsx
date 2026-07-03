"use client"

import { useState } from "react"
import { Library, Menu } from "lucide-react"

import { Button } from "@/components/ui/button"
import {
  Sheet,
  SheetContent,
  SheetTitle,
  SheetTrigger,
} from "@/components/ui/sheet"
import {
  SidebarBrand,
  SidebarNav,
  SidebarLogout,
  sidebarGradient,
} from "@/components/dashboard/sidebar"

export function Header({ title }: { title: string }) {
  const [open, setOpen] = useState(false)

  return (
    <header className="sticky top-0 z-30 flex h-16 items-center gap-3 border-b bg-background/95 px-4 backdrop-blur md:px-6">
      <Sheet open={open} onOpenChange={setOpen}>
        <SheetTrigger
          render={
            <Button variant="outline" size="icon" className="md:hidden" />
          }
        >
          <Menu />
          <span className="sr-only">Abrir menú</span>
        </SheetTrigger>
        <SheetContent
          side="left"
          className="w-64 border-none p-0 text-sidebar-foreground"
          style={sidebarGradient}
        >
          <SheetTitle className="sr-only">Navegación</SheetTitle>
          <div className="flex h-full flex-col">
            <SidebarBrand />
            <SidebarNav onNavigate={() => setOpen(false)} />
            <SidebarLogout />
          </div>
        </SheetContent>
      </Sheet>

      <div className="flex items-center gap-2.5">
        <div className="flex size-9 items-center justify-center rounded-lg bg-primary/10 text-primary">
          <Library className="size-5" />
        </div>
        <h1 className="text-lg font-semibold tracking-tight text-balance">
          {title}
        </h1>
      </div>
    </header>
  )
}
