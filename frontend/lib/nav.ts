import {
  LayoutDashboard,
  BookOpen,
  Users,
  ArrowLeftRight,
  CircleDollarSign,
  type LucideIcon,
} from "lucide-react"

export interface NavItem {
  href: string
  label: string
  icon: LucideIcon
}

export const navItems: NavItem[] = [
  { href: "/dashboard", label: "Dashboard", icon: LayoutDashboard },
  { href: "/libros", label: "Libros", icon: BookOpen },
  { href: "/usuarios", label: "Usuarios", icon: Users },
  { href: "/prestamos", label: "Préstamos", icon: ArrowLeftRight },
  { href: "/multas", label: "Multas", icon: CircleDollarSign },
]
