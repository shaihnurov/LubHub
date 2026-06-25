"use client";

import { Suspense, useEffect, useState } from "react";
import { useRouter, useSearchParams } from "next/navigation";
import { api } from "@/lib/api";
import { useAuth } from "@/lib/auth";

function AuthCallbackContent() {
  const router = useRouter();
  const searchParams = useSearchParams();
  const code = searchParams.get("code");
  const [error, setError] = useState<string | null>(null);
  const { login } = useAuth();

  useEffect(() => {
    if (!code) return;

    let active = true;

    api.auth
      .exchangeTwitchCode(code)
      .then((response) => {
        if (!active) return;
        login(response.token, response.displayName, response.twitchId);
        router.replace("/");
      })
      .catch((err) => {
        if (!active) return;
        setError(err instanceof Error ? err.message : "Authentication failed");
      });

    return () => {
      active = false;
    };
  }, [code, router, login]);

  if (!code) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <p className="text-red-400 text-lg mb-4">Missing authorization code</p>
          <button
            onClick={() => router.replace("/")}
            className="px-6 py-3 rounded-xl bg-accent text-white font-medium hover:bg-accent-light transition-colors"
          >
            Back to home
          </button>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <p className="text-red-400 text-lg mb-4">{error}</p>
          <button
            onClick={() => router.replace("/")}
            className="px-6 py-3 rounded-xl bg-accent text-white font-medium hover:bg-accent-light transition-colors"
          >
            Back to home
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen flex items-center justify-center">
      <div className="flex flex-col items-center gap-4">
        <div className="w-8 h-8 border-2 border-accent border-t-transparent rounded-full animate-spin" />
        <p className="text-foreground/60 text-sm">Completing authentication...</p>
      </div>
    </div>
  );
}

export default function AuthCallbackPage() {
  return (
    <Suspense
      fallback={
        <div className="min-h-screen flex items-center justify-center">
          <div className="w-8 h-8 border-2 border-accent border-t-transparent rounded-full animate-spin" />
        </div>
      }
    >
      <AuthCallbackContent />
    </Suspense>
  );
}
