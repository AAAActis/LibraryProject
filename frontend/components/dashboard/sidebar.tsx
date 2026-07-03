"use client"

import Link from "next/link"
import { usePathname, useRouter } from "next/navigation"
import { BookMarked, LogOut } from "lucide-react"

import { cn } from "@/lib/utils"
import { logout } from "@/lib/services"
import { navItems } from "@/lib/nav"
import { Button } from "@/components/ui/button"

export function SidebarNav({ onNavigate }: { onNavigate?: () => void }) {
  const pathname = usePathname()

  return (
    <nav className="flex flex-1 flex-col gap-1 px-3 py-2">
      {navItems.map((item) => {
        const active =
          pathname === item.href || pathname.startsWith(item.href + "/")
        const Icon = item.icon
        return (
          <Link
            key={item.href}
            href={item.href}
            onClick={onNavigate}
            className={cn(
              "flex items-center gap-3 rounded-lg px-3 py-2 text-sm font-medium transition-colors",
              active
                ? "bg-sidebar-primary text-sidebar-primary-foreground"
                : "text-sidebar-foreground/70 hover:bg-sidebar-accent hover:text-sidebar-accent-foreground",
            )}
          >
            <Icon className="size-4.5" />
            {item.label}
          </Link>
        )
      })}
    </nav>
  )
}

export function SidebarBrand() {
  return (
    <div className="flex items-center gap-2.5 px-6 py-5">
      <div className="flex size-9 items-center justify-center rounded-lg bg-primary text-primary-foreground">
        <BookMarked className="size-5" />
      </div>
      <span className="text-base font-semibold tracking-tight text-sidebar-foreground">
        LibraryProject
      </span>
    </div>
  )
}

export function SidebarLogout() {
  const router = useRouter()

  function handleLogout() {
    logout()
    router.replace("/login")
  }

  return (
    <div className="border-t border-sidebar-border p-3">
      <Button
        variant="ghost"
        className="w-full justify-start gap-3 text-sidebar-foreground/70 hover:bg-sidebar-accent hover:text-sidebar-accent-foreground"
        onClick={handleLogout}
      >
        <LogOut className="size-4.5" />
        Cerrar sesión
      </Button>
    </div>
  )
}

const sidebarGradient = {
  backgroundImage:
    "linear-gradient(to bottom, var(--sidebar-gradient-from), var(--sidebar-gradient-to))",
}

export function Sidebar() {
  return (
    <aside
      className="hidden w-64 shrink-0 flex-col bg-sidebar text-sidebar-foreground md:flex"
      style={sidebarGradient}
    >
      <SidebarBrand />
      <SidebarNav />
      <SidebarLogout />
    </aside>
  )
}

export { sidebarGradient }
