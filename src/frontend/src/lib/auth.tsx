"use client";

import {
  createContext,
  useCallback,
  useContext,
  useSyncExternalStore,
  type ReactNode,
} from "react";
import { api } from "@/lib/api";
import { clearToken, getDisplayName, getToken } from "@/lib/token";

const authListeners = new Set<() => void>();

function subscribeAuth(callback: () => void) {
  authListeners.add(callback);
  return () => {
    authListeners.delete(callback);
  };
}

function getAuthSnapshot(): string | null {
  const token = getToken();
  if (!token) return null;
  return getDisplayName();
}

function getServerSnapshot(): string | null {
  return null;
}

interface AuthContextValue {
  isAuthenticated: boolean;
  displayName: string | null;
  loginWithTwitch: () => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const displayName = useSyncExternalStore(
    subscribeAuth,
    getAuthSnapshot,
    getServerSnapshot,
  );

  const loginWithTwitch = useCallback(async () => {
    const url = await api.auth.getTwitchLoginUrl();
    window.location.href = url;
  }, []);

  const logout = useCallback(() => {
    clearToken();
    authListeners.forEach((cb) => cb());
  }, []);

  return (
    <AuthContext.Provider
      value={{
        isAuthenticated: !!displayName,
        displayName,
        loginWithTwitch,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within AuthProvider");
  }
  return context;
}
