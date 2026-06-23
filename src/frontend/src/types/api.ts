export type RaffleStatus = "Pending" | "Active" | "Finished" | "Drawn";

export interface AuthResponse {
  token: string;
  displayName: string;
}

export interface WinnerResponse {
  winnerId: number;
  twitchUserId: string;
  displayName: string;
}

export interface CreateRaffleRequest {
  title: string;
}

export interface Raffle {
  id: number;
  streamerId: number;
  title: string;
  status: RaffleStatus;
  startedAt: string | null;
  endedAt: string | null;
}

export interface Participant {
  id: number;
  raffleId: number;
  twitchUserId: string;
  displayName: string;
  botScore: number;
  isSuspected: boolean;
}

export interface ApiError {
  type: string;
  title: string;
  status: number;
  detail?: string;
}

export type RaffleHubEvent =
  | "ParticipantCountUpdated"
  | "WinnerDrawn"
  | "JoinConfirmed";

export interface RaffleHubHandlers {
  onParticipantCountUpdated?: (count: number) => void;
  onWinnerDrawn?: (twitchUserId: string, displayName: string) => void;
  onJoinConfirmed?: (raffleId: number) => void;
}
