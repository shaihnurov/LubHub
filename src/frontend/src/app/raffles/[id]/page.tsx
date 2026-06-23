"use client";

import { useEffect, useState, useCallback } from "react";
import { useParams, useRouter } from "next/navigation";
import { motion } from "framer-motion";
import useSWR from "swr";
import { api } from "@/lib/api";
import { useAuth } from "@/lib/auth";
import Button from "@/components/ui/Button";
import Badge from "@/components/ui/Badge";
import { Skeleton } from "@/components/ui/Skeleton";
import GiftIcon from "@/components/ui/GiftIcon";
import BackButton from "@/components/ui/BackButton";
import CosmicBackground from "@/components/ui/CosmicBackground";
import {
  startRaffleHub,
  stopRaffleHub,
  joinRaffleGroup,
  leaveRaffleGroup,
  registerRaffleHandlers,
} from "@/lib/signalr";
import { logger } from "@/lib/logger";

export default function RaffleDetailPage() {
  const params = useParams();
  const router = useRouter();
  const { isAuthenticated } = useAuth();
  const raffleId = Number(params.id);

  const { data: raffle, mutate } = useSWR(
    raffleId ? `raffle-${raffleId}` : null,
    () => api.raffles.getById(raffleId)
  );

  const [isJoining, setIsJoining] = useState(false);
  const [hasJoined, setHasJoined] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [liveCount, setLiveCount] = useState<number | null>(null);
  const [winner, setWinner] = useState<{ twitchUserId: string; displayName: string } | null>(null);

  const participantCount = liveCount ?? raffle?.participantCount ?? 0;

  const handleParticipantCountUpdated = useCallback((count: number) => {
    setLiveCount(count);
  }, []);

  const handleWinnerDrawn = useCallback((twitchUserId: string, displayName: string) => {
    setWinner({ twitchUserId, displayName });
    mutate();
  }, [mutate]);

  const handleJoinConfirmed = useCallback((id: number) => {
    if (id === raffleId) {
      setHasJoined(true);
      setIsJoining(false);
    }
  }, [raffleId]);

  useEffect(() => {
    if (!raffleId || raffle?.status !== "Active") return;
    
    let cleanup: (() => void) | undefined;
    let cancelled = false;

    const connect = async () => {
      try {
        await startRaffleHub();
        if (cancelled) return;
        
        await joinRaffleGroup(raffleId);
        if (cancelled) return;
        
        cleanup = registerRaffleHandlers({
          onParticipantCountUpdated: handleParticipantCountUpdated,
          onWinnerDrawn: handleWinnerDrawn,
          onJoinConfirmed: handleJoinConfirmed,
        });
      } catch (err) {
        logger.error("SignalR connection error", err);
      }
    };

    connect();

    return () => {
      cancelled = true;
      if (cleanup) cleanup();
      leaveRaffleGroup(raffleId).catch(() => {});
      stopRaffleHub().catch(() => {});
    };
  }, [raffleId, raffle?.status, handleParticipantCountUpdated, handleWinnerDrawn, handleJoinConfirmed]);

  const handleJoin = async () => {
    if (!isAuthenticated) {
      setError("Please login to join the raffle");
      return;
    }

    setIsJoining(true);
    setError(null);

    try {
      await api.raffles.join(raffleId);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to join");
      setIsJoining(false);
    }
  };

  if (!raffle) {
    return (
      <div className="min-h-screen bg-background relative overflow-hidden">
        <CosmicBackground />
        <div className="relative z-10 max-w-5xl mx-auto px-8 py-12">
          <Skeleton className="h-8 w-48 mb-8" />
          <div className="glass rounded-2xl p-12">
            <Skeleton className="h-64 mb-8" />
            <Skeleton className="h-12 w-3/4 mb-4" />
          </div>
        </div>
      </div>
    );
  }

  const displayWinner = winner ?? raffle.winner;

  return (
    <div className="min-h-screen bg-background relative overflow-hidden">
      <CosmicBackground showGrid={true} />

      <div className="relative z-10 max-w-5xl mx-auto px-8 py-12">
        <BackButton onClick={() => router.push("/raffles")}>Back to Raffles</BackButton>

        <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.5 }}>
          <div className="glass rounded-3xl overflow-hidden mb-6">
            <div className="relative h-80 bg-gradient-to-br from-accent/30 via-transparent to-accent-cyan/30 overflow-hidden">
              <div className="absolute inset-0 bg-grid opacity-20" />
              <div className="absolute inset-0 flex items-center justify-center">
                <motion.div
                  initial={{ scale: 0.8, opacity: 0 }}
                  animate={{ scale: 1, opacity: 1 }}
                  transition={{ duration: 0.6, delay: 0.2 }}
                  className="text-center"
                >
                  <div className="w-24 h-24 mx-auto rounded-3xl bg-gradient-to-br from-accent/20 to-accent-cyan/20 border border-accent/30 flex items-center justify-center mb-4 backdrop-blur-sm">
                    <GiftIcon className="w-12 h-12 text-accent" />
                  </div>
                </motion.div>
              </div>
              <div className="absolute top-6 right-6">
                <Badge status={raffle.status} />
              </div>
            </div>

            <div className="p-8">
              <div className="mb-8">
                <motion.h1
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: 0.1 }}
                  className="text-5xl font-bold mb-4 tracking-tight"
                >
                  {raffle.title}
                </motion.h1>
                <motion.p
                  initial={{ opacity: 0 }}
                  animate={{ opacity: 1 }}
                  transition={{ delay: 0.2 }}
                  className="text-foreground/60 text-lg"
                >
                  by <span className="text-foreground/80 font-medium">{raffle.streamerName}</span>
                </motion.p>
              </div>

              <div className="grid grid-cols-2 gap-6 mb-8">
                <motion.div
                  initial={{ opacity: 0, scale: 0.9 }}
                  animate={{ opacity: 1, scale: 1 }}
                  transition={{ delay: 0.3 }}
                  className="glass-strong rounded-2xl p-8 text-center"
                >
                  <div className="text-foreground/60 text-sm mb-3">Participants</div>
                  <div className="text-5xl font-bold text-gradient">{participantCount.toLocaleString()}</div>
                </motion.div>
                <motion.div
                  initial={{ opacity: 0, scale: 0.9 }}
                  animate={{ opacity: 1, scale: 1 }}
                  transition={{ delay: 0.4 }}
                  className="glass-strong rounded-2xl p-8 text-center"
                >
                  <div className="text-foreground/60 text-sm mb-3">Status</div>
                  <div className="text-3xl font-semibold capitalize">{raffle.status.toLowerCase()}</div>
                </motion.div>
              </div>

              {displayWinner && (
                <motion.div
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: 0.5 }}
                  className="mb-8 p-8 rounded-2xl bg-gradient-to-r from-accent/20 to-accent-cyan/20 border border-accent/30 backdrop-blur-sm"
                >
                  <div className="flex items-center gap-6">
                    <div className="w-16 h-16 rounded-2xl bg-accent/30 flex items-center justify-center">
                      <svg className="w-8 h-8 text-accent" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
                        <path strokeLinecap="round" strokeLinejoin="round" d="M11.48 3.499a.562.562 0 011.04 0l2.125 5.111a.563.563 0 00.475.345l5.518.442c.499.04.701.663.321.988l-4.204 3.602a.563.563 0 00-.182.557l1.285 5.385a.562.562 0 01-.84.61l-4.725-2.885a.563.563 0 00-.586 0L6.982 20.54a.562.562 0 01-.84-.61l1.285-5.386a.562.562 0 00-.182-.557l-4.204-3.602a.563.563 0 01.321-.988l5.518-.442a.563.563 0 00.475-.345L11.48 3.5z" />
                      </svg>
                    </div>
                    <div>
                      <div className="text-sm text-accent-cyan mb-2 font-medium">Winner</div>
                      <div className="text-2xl font-bold">{displayWinner.displayName}</div>
                    </div>
                  </div>
                </motion.div>
              )}

              {error && (
                <motion.div
                  initial={{ opacity: 0, y: -10 }}
                  animate={{ opacity: 1, y: 0 }}
                  className="mb-6 p-4 rounded-xl bg-red-500/10 border border-red-500/20 text-red-400"
                >
                  {error}
                </motion.div>
              )}

              {raffle.status === "Active" && !hasJoined && (
                <motion.div
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: 0.5 }}
                >
                  <Button onClick={handleJoin} disabled={isJoining} size="lg" className="w-full text-lg py-5">
                    {isJoining ? "Joining..." : "Join Raffle"}
                  </Button>
                </motion.div>
              )}

              {hasJoined && (
                <motion.div
                  initial={{ opacity: 0, scale: 0.9 }}
                  animate={{ opacity: 1, scale: 1 }}
                  className="p-6 rounded-2xl bg-green-500/10 border border-green-500/20 text-green-400 text-center text-lg font-medium"
                >
                  You have successfully joined this raffle!
                </motion.div>
              )}

              {raffle.status === "Pending" && (
                <div className="p-6 rounded-2xl glass text-center text-foreground/60">
                  This raffle hasn&apos;t started yet
                </div>
              )}

              {raffle.status === "Finished" && !displayWinner && (
                <div className="p-6 rounded-2xl glass text-center text-foreground/60">
                  Raffle finished, waiting for winner announcement...
                </div>
              )}

              {raffle.status === "Drawn" && !displayWinner && (
                <div className="p-6 rounded-2xl glass text-center text-foreground/60">
                  Winner has been drawn
                </div>
              )}
            </div>
          </div>
        </motion.div>
      </div>
    </div>
  );
}
