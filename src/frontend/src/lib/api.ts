import type {
  ApiError,
  AuthResponse,
  CreateRaffleRequest,
  WinnerResponse,
  Raffle,
  Participant,
  UserProfile,
} from "@/types/api";
import { getToken } from "@/lib/token";
import { logger } from "@/lib/logger";

const API_PREFIX = "/api/v1";

export class ApiClientError extends Error {
  constructor(
    public status: number,
    public error: ApiError | null,
    message: string
  ) {
    super(message);
    this.name = "ApiClientError";
  }
}

async function request<T>(path: string, options: RequestInit = {}): Promise<T> {
  const token = getToken();

  const headers: Record<string, string> = {
    "Content-Type": "application/json",
    ...((options.headers as Record<string, string>) ?? {}),
  };

  if (token) {
    headers["Authorization"] = `Bearer ${token}`;
  }

  const response = await fetch(`${API_PREFIX}${path}`, {
    ...options,
    headers,
  });

  if (response.status === 204) {
    return undefined as T;
  }

  if (!response.ok) {
    let errorBody: ApiError | null = null;
    try {
      errorBody = await response.json();
    } catch {
      logger.warn("Failed to parse error response");
    }
    throw new ApiClientError(
      response.status,
      errorBody,
      errorBody?.detail ?? "Request failed"
    );
  }

  const contentType = response.headers.get("content-type") ?? "";

  if (contentType.includes("application/json")) {
    return response.json() as Promise<T>;
  }

  const text = await response.text();
  return text as unknown as T;
}

export const api = {
  auth: {
    getTwitchLoginUrl: () => request<string>("/auth/twitch/login"),
    exchangeTwitchCode: (code: string) =>
      request<AuthResponse>(`/auth/twitch/callback?code=${encodeURIComponent(code)}`),
  },

  raffles: {
    create: (body: CreateRaffleRequest) =>
      request<number>("/raffles", { method: "POST", body: JSON.stringify(body) }),
    start: (id: number) => request<void>(`/raffles/${id}/start`, { method: "POST" }),
    finish: (id: number) => request<void>(`/raffles/${id}/finish`, { method: "POST" }),
    draw: (id: number) => request<WinnerResponse>(`/raffles/${id}/draw`, { method: "POST" }),
    join: (id: number) => request<void>(`/raffles/${id}/join`, { method: "POST" }),
    listMy: () => request<Raffle[]>("/raffles/my"),
    listAll: () => request<Raffle[]>("/raffles"),
    getById: (id: number) => request<Raffle>(`/raffles/${id}`),
    getParticipants: (raffleId: number) => request<Participant[]>(`/raffles/${raffleId}/participants`),
  },

  profile: {
    get: () => request<UserProfile>("/profile"),
  },
};
