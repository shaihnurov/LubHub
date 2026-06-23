export type RaffleStatus = "live" | "ending" | "upcoming" | "ended";

export interface Feature {
  icon: React.ReactNode;
  title: string;
  description: string;
}

export interface Giveaway {
  title: string;
  streamer: string;
  viewers: string;
  prize: string;
  status: RaffleStatus;
  participants: number;
  maxParticipants: number;
}

export interface StatusConfig {
  label: string;
  color: string;
  bg: string;
}

export interface NavLink {
  label: string;
  href: string;
}
