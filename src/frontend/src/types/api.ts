export type RaffleStatus = "Pending" | "Active" | "Finished" | "Drawn";

export interface AuthResponse {
  token: string;
  displayName: string;
  twitchId: string;
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
  participantCount: number;
  streamerName: string;
  winner?: WinnerResponse | null;
}

export interface Participant {
  id: number;
  raffleId: number;
  twitchUserId: string;
  displayName: string;
  botScore: number;
  isSuspected: boolean;
}

export interface UserProfile {
  twitchId: string;
  displayName: string;
  email: string;
  rafflesCreated: number;
  rafflesParticipated: number;
  wins: number;
  createdAt: string;
}

export interface ApiError {
  type: string;
  title: string;
  status: number;
  detail?: string;
}

export interface RaffleHubHandlers {
  onParticipantCountUpdated?: (count: number) => void;
  onWinnerDrawn?: (twitchUserId: string, displayName: string) => void;
  onJoinConfirmed?: (raffleId: number) => void;
}
