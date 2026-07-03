# Compila el frontend (Next.js, build estático) y lo copia a Libreria1/wwwroot/
# para que el backend lo sirva como archivos estáticos (mismo origen, sin CORS).
#
# Uso: .\build.ps1
# Luego: dotnet run --project Libreria1   (o dotnet publish para producción)

$ErrorActionPreference = "Stop"

Push-Location "$PSScriptRoot/frontend"
try {
    npm install

    # Vacío => el frontend hace requests relativas ("/api/...") al mismo origen
    # donde se sirve, en vez de apuntar a http://localhost:5000.
    $env:NEXT_PUBLIC_API_URL = ""
    npm run build
}
finally {
    Pop-Location
}

$wwwroot = "$PSScriptRoot/Libreria1/wwwroot"
if (Test-Path $wwwroot) {
    Remove-Item -Recurse -Force $wwwroot
}
Copy-Item -Recurse "$PSScriptRoot/frontend/out" $wwwroot

Write-Host "Build listo en $wwwroot"
