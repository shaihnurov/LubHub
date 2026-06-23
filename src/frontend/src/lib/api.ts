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

const STUB_RAFFLES: Raffle[] = [
  {
    id: 1,
    streamerId: 1,
    title: "Steam Key Giveaway",
    status: "Active",
    startedAt: "2026-06-20T10:00:00Z",
    endedAt: null,
    participantCount: 8742,
    streamerName: "xQC",
  },
  {
    id: 2,
    streamerId: 1,
    title: "Merch Drop",
    status: "Active",
    startedAt: "2026-06-19T14:00:00Z",
    endedAt: null,
    participantCount: 3291,
    streamerName: "Pokimane",
  },
  {
    id: 3,
    streamerId: 1,
    title: "Gift Card Frenzy",
    status: "Finished",
    startedAt: "2026-06-18T12:00:00Z",
    endedAt: "2026-06-22T18:00:00Z",
    participantCount: 9650,
    streamerName: "Shroud",
    winner: { winnerId: 42, twitchUserId: "123456", displayName: "LuckyViewer" },
  },
  {
    id: 4,
    streamerId: 1,
    title: "Console Giveaway",
    status: "Pending",
    startedAt: null,
    endedAt: null,
    participantCount: 0,
    streamerName: "TimTheTatman",
  },
  {
    id: 5,
    streamerId: 1,
    title: "Keyboard Drop",
    status: "Drawn",
    startedAt: "2026-06-10T09:00:00Z",
    endedAt: "2026-06-15T17:00:00Z",
    participantCount: 7200,
    streamerName: "Lirik",
    winner: { winnerId: 99, twitchUserId: "789012", displayName: "KeyboardWinner" },
  },
];

const STUB_PROFILE: UserProfile = {
  twitchId: "12345678",
  displayName: "TestStreamer",
  email: "test@example.com",
  rafflesCreated: 5,
  rafflesParticipated: 12,
  wins: 2,
  createdAt: "2026-01-15T10:00:00Z",
};

const STUB_PARTICIPANTS: Participant[] = [
  { id: 1, raffleId: 1, twitchUserId: "user1", displayName: "ViewerOne", botScore: 0, isSuspected: false },
  { id: 2, raffleId: 1, twitchUserId: "user2", displayName: "ViewerTwo", botScore: 0.1, isSuspected: false },
  { id: 3, raffleId: 1, twitchUserId: "user3", displayName: "ViewerThree", botScore: 0.8, isSuspected: true },
  { id: 4, raffleId: 1, twitchUserId: "user4", displayName: "ViewerFour", botScore: 0, isSuspected: false },
  { id: 5, raffleId: 1, twitchUserId: "user5", displayName: "ViewerFive", botScore: 0.05, isSuspected: false },
];

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
    listMy: async (): Promise<Raffle[]> => {
      await new Promise((r) => setTimeout(r, 400));
      return STUB_RAFFLES;
    },
    listAll: async (): Promise<Raffle[]> => {
      await new Promise((r) => setTimeout(r, 400));
      return STUB_RAFFLES;
    },
    getById: async (id: number): Promise<Raffle> => {
      await new Promise((r) => setTimeout(r, 300));
      const raffle = STUB_RAFFLES.find((r) => r.id === id) ?? STUB_RAFFLES[0];
      return { ...raffle };
    },
    getParticipants: async (raffleId: number): Promise<Participant[]> => {
      await new Promise((r) => setTimeout(r, 300));
      void raffleId;
      return STUB_PARTICIPANTS;
    },
  },

  profile: {
    get: async (): Promise<UserProfile> => {
      await new Promise((r) => setTimeout(r, 300));
      return STUB_PROFILE;
    },
  },
};
