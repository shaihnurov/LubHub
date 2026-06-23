const TOKEN_KEY = "lubhub_token";
const DISPLAY_NAME_KEY = "lubhub_display_name";

export function getToken(): string | null {
  if (typeof window === "undefined") return null;
  return localStorage.getItem(TOKEN_KEY);
}

export function setToken(token: string, displayName: string): void {
  localStorage.setItem(TOKEN_KEY, token);
  localStorage.setItem(DISPLAY_NAME_KEY, displayName);
}

export function getDisplayName(): string | null {
  if (typeof window === "undefined") return null;
  return localStorage.getItem(DISPLAY_NAME_KEY);
}

export function clearToken(): void {
  localStorage.removeItem(TOKEN_KEY);
  localStorage.removeItem(DISPLAY_NAME_KEY);
}
