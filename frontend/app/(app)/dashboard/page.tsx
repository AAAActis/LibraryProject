"use client"

import useSWR from "swr"
import { BookOpen, BookCheck, ArrowLeftRight, CircleDollarSign } from "lucide-react"

import { getStats } from "@/lib/services"
import { Header } from "@/components/dashboard/header"
import { StatCard } from "@/components/dashboard/stat-card"
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert"

export default function DashboardPage() {
  const { data, error, isLoading } = useSWR("dashboard-stats", getStats)

  return (
    <>
      <Header title="Dashboard" />
      <main className="flex-1 p-4 md:p-6">
        {error && (
          <Alert variant="destructive" className="mb-6">
            <AlertTitle>No se pudieron cargar las estadísticas</AlertTitle>
            <AlertDescription>{error.message}</AlertDescription>
          </Alert>
        )}
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-4">
          <StatCard
            label="Total de libros"
            value={data?.totalLibros}
            icon={BookOpen}
            loading={isLoading}
          />
          <StatCard
            label="Libros disponibles"
            value={data?.librosDisponibles}
            icon={BookCheck}
            loading={isLoading}
            accent
          />
          <StatCard
            label="Préstamos activos"
            value={data?.prestamosActivos}
            icon={ArrowLeftRight}
            loading={isLoading}
          />
          <StatCard
            label="Multas pendientes"
            value={data?.multasPendientes}
            icon={CircleDollarSign}
            loading={isLoading}
          />
        </div>
      </main>
    </>
  )
}
