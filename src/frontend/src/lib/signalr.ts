import { HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr";
import type { RaffleHubHandlers } from "@/types/api";
import { getToken } from "@/lib/token";
import { logger } from "@/lib/logger";

const HUB_URL = "http://localhost:5217/hubs/raffle";

let connection: HubConnection | null = null;
let isConnecting = false;

export function getRaffleHub(): HubConnection {
  if (connection) return connection;

  connection = new HubConnectionBuilder()
    .withUrl(HUB_URL, { accessTokenFactory: () => getToken() ?? "" })
    .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
    .configureLogging(LogLevel.Warning)
    .build();

  return connection;
}

export async function startRaffleHub(): Promise<void> {
  const hub = getRaffleHub();

  if (hub.state === HubConnectionState.Connected) return;
  if (isConnecting) return;

  isConnecting = true;
  try {
    await hub.start();
  } catch (err) {
    logger.error("SignalR connection failed", err);
    throw err;
  } finally {
    isConnecting = false;
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
