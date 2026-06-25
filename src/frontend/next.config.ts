import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  async rewrites() {
    const apiBaseUrl = process.env.API_BASE_URL ?? "http://localhost:5217";

    return [
      {
        source: "/api/:path*",
        destination: `${apiBaseUrl}/api/:path*`,
      },
      {
        source: "/hubs/:path*",
        destination: `${apiBaseUrl}/hubs/:path*`,
      },
    ];
  },
};

export default nextConfig;
