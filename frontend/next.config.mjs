/** @type {import('next').NextConfig} */
const nextConfig = {
  // Build estático (HTML/CSS/JS) para que ASP.NET Core lo sirva como wwwroot/.
  output: "export",
  trailingSlash: true,
  typescript: {
    ignoreBuildErrors: true,
  },
  images: {
    unoptimized: true,
  },
}

export default nextConfig
