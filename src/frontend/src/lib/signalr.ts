import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from "@microsoft/signalr";
import type { RaffleHubHandlers } from "@/types/api";
import { getToken } from "@/lib/token";

const HUB_PATH = "/hubs/raffle";

let connection: HubConnection | null = null;

function getHubUrl(): string {
  const baseUrl = process.env.NEXT_PUBLIC_SIGNALR_URL ?? "http://localhost:5217";
  return `${baseUrl}${HUB_PATH}`;
}

export function getRaffleHub(): HubConnection {
  if (connection) return connection;

  connection = new HubConnectionBuilder()
    .withUrl(getHubUrl(), {
      accessTokenFactory: () => getToken() ?? "",
    })
    .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
    .configureLogging(LogLevel.Information)
    .build();

  return connection;
}

export async function startRaffleHub(): Promise<void> {
  const hub = getRaffleHub();

  if (hub.state === HubConnectionState.Connected) return;

  try {
    await hub.start();
  } catch (err) {
    console.error("[SignalR] Failed to connect:", err);
    throw err;
  }
}

export async function stopRaffleHub(): Promise<void> {
  const hub = getRaffleHub();

  if (hub.state === HubConnectionState.Disconnected) return;

  await hub.stop();
}

export async function joinRaffleGroup(raffleId: number): Promise<void> {
  const hub = getRaffleHub();
  await hub.invoke("JoinRaffleGroup", raffleId);
}

export async function leaveRaffleGroup(raffleId: number): Promise<void> {
  const hub = getRaffleHub();
  await hub.invoke("LeaveRaffleGroup", raffleId);
}

export function registerRaffleHandlers(handlers: RaffleHubHandlers): () => void {
  const hub = getRaffleHub();

  if (handlers.onParticipantCountUpdated) {
    hub.on("ParticipantCountUpdated", handlers.onParticipantCountUpdated);
  }
  if (handlers.onWinnerDrawn) {
    hub.on("WinnerDrawn", handlers.onWinnerDrawn);
  }
  if (handlers.onJoinConfirmed) {
    hub.on("JoinConfirmed", handlers.onJoinConfirmed);
  }

  return () => {
    hub.off("ParticipantCountUpdated");
    hub.off("WinnerDrawn");
    hub.off("JoinConfirmed");
  };
}
