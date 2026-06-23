"use client";

import { useEffect } from "react";
import { useRouter } from "next/navigation";
import { useAuth } from "@/lib/auth";

export function useAuthGuard() {
  const { isAuthenticated } = useAuth();
  const router = useRouter();

  useEffect(() => {
    if (!isAuthenticated) {
      router.replace("/");
    }
  }, [isAuthenticated, router]);

  return { isAuthenticated };
}

export function AuthGuard({ children }: { children: React.ReactNode }) {
  useAuthGuard();
  return <>{children}</>;
}
