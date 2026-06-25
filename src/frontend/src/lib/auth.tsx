"use client";

import {
  createContext,
  useCallback,
  useContext,
  useSyncExternalStore,
  type ReactNode,
} from "react";
import { api } from "@/lib/api";
import { clearToken, getDisplayName, getToken, getTwitchId, setToken } from "@/lib/token";

const authListeners = new Set<() => void>();

function subscribeAuth(callback: () => void) {
  authListeners.add(callback);
  return () => {
    authListeners.delete(callback);
  };
}

interface AuthSnapshot {
  displayName: string | null;
  twitchId: string | null;
}

const EMPTY_SNAPSHOT: AuthSnapshot = { displayName: null, twitchId: null };

let cachedSnapshot: AuthSnapshot | null = null;

function getAuthSnapshot(): AuthSnapshot {
  const token = getToken();
  if (!token) return EMPTY_SNAPSHOT;

  const next: AuthSnapshot = { displayName: getDisplayName(), twitchId: getTwitchId() };

  if (
    cachedSnapshot &&
    cachedSnapshot.displayName === next.displayName &&
    cachedSnapshot.twitchId === next.twitchId
  ) {
    return cachedSnapshot;
  }

  cachedSnapshot = next;
  return next;
}

function getServerSnapshot(): AuthSnapshot {
  return EMPTY_SNAPSHOT;
}

function notifyAuthListeners() {
  authListeners.forEach((cb) => cb());
}

interface AuthContextValue {
  isAuthenticated: boolean;
  displayName: string | null;
  twitchId: string | null;
  loginWithTwitch: () => Promise<void>;
  login: (token: string, displayName: string, twitchId: string) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const snapshot = useSyncExternalStore(
    subscribeAuth,
    getAuthSnapshot,
    getServerSnapshot,
  );

  const loginWithTwitch = useCallback(async () => {
    const url = await api.auth.getTwitchLoginUrl();
    window.location.href = url;
  }, []);

  const login = useCallback((token: string, name: string, twitchId: string) => {
    setToken(token, name, twitchId);
    notifyAuthListeners();
  }, []);

  const logout = useCallback(() => {
    clearToken();
    notifyAuthListeners();
  }, []);

  return (
    <AuthContext.Provider
      value={{
        isAuthenticated: !!snapshot.displayName,
        displayName: snapshot.displayName,
        twitchId: snapshot.twitchId,
        loginWithTwitch,
        login,
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
