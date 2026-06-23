"use client";

import { useCallback } from "react";
import { useParams, useRouter } from "next/navigation";
import { motion } from "framer-motion";
import useSWR from "swr";
import { api } from "@/lib/api";
import Button from "@/components/ui/Button";
import Badge from "@/components/ui/Badge";
import BackButton from "@/components/ui/BackButton";
import { ListSkeleton } from "@/components/ui/Skeleton";
import { logger } from "@/lib/logger";
import type { Participant } from "@/types/api";

export default function RaffleManagePage() {
  const params = useParams();
  const router = useRouter();
  const raffleId = Number(params.id);

  const { data: raffle, mutate } = useSWR(
    raffleId ? `raffle-${raffleId}` : null,
    () => api.raffles.getById(raffleId)
  );
  const { data: participants } = useSWR(
    raffleId ? `raffle-${raffleId}-participants` : null,
    () => api.raffles.getParticipants(raffleId)
  );

  const handleAction = useCallback(async (action: () => Promise<unknown>) => {
    try {
      await action();
      mutate();
    } catch (err) {
      logger.error("Raffle action failed", err);
    }
  }, [mutate]);

  if (!raffle) {
    return (
      <div className="max-w-5xl mx-auto">
        <div className="mb-8">
          <div className="h-8 w-64 bg-foreground/5 rounded animate-pulse mb-2" />
          <div className="h-4 w-48 bg-foreground/5 rounded animate-pulse" />
        </div>
        <ListSkeleton count={5} />
      </div>
    );
  }

  return (
    <div className="max-w-5xl mx-auto">
      <BackButton onClick={() => router.push("/dashboard")}>
        Back to Dashboard
      </BackButton>

      <div className="glass rounded-2xl p-8 mb-6">
        <div className="flex items-start justify-between mb-6">
          <div>
            <h1 className="text-3xl font-bold mb-3">{raffle.title}</h1>
            <Badge status={raffle.status} />
          </div>
          <div className="flex gap-3">
            {raffle.status === "Pending" && (
              <Button onClick={() => handleAction(() => api.raffles.start(raffleId))}>
                Start Raffle
              </Button>
            )}
            {raffle.status === "Active" && (
              <Button onClick={() => handleAction(() => api.raffles.finish(raffleId))}>
                Finish Raffle
              </Button>
            )}
            {raffle.status === "Finished" && (
              <Button onClick={() => handleAction(() => api.raffles.draw(raffleId))}>
                Draw Winner
              </Button>
            )}
          </div>
        </div>

        <div className="grid grid-cols-3 gap-4">
          <div className="glass-strong rounded-xl p-4">
            <div className="text-foreground/60 text-sm mb-1">Participants</div>
            <div className="text-2xl font-bold text-gradient">{raffle.participantCount}</div>
          </div>
          <div className="glass-strong rounded-xl p-4">
            <div className="text-foreground/60 text-sm mb-1">Status</div>
            <div className="text-lg font-semibold capitalize">{raffle.status.toLowerCase()}</div>
          </div>
          <div className="glass-strong rounded-xl p-4">
            <div className="text-foreground/60 text-sm mb-1">Created</div>
            <div className="text-sm">
              {new Date(raffle.startedAt ?? raffle.endedAt ?? new Date().toISOString()).toLocaleDateString()}
            </div>
          </div>
        </div>

        {raffle.winner && (
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            className="mt-6 p-6 rounded-xl bg-gradient-to-r from-accent/10 to-accent-cyan/10 border border-accent/20"
          >
            <div className="flex items-center gap-4">
              <div className="w-12 h-12 rounded-full bg-accent/20 flex items-center justify-center">
                <svg className="w-6 h-6 text-accent" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
                  <path strokeLinecap="round" strokeLinejoin="round" d="M11.48 3.499a.562.562 0 011.04 0l2.125 5.111a.563.563 0 00.475.345l5.518.442c.499.04.701.663.321.988l-4.204 3.602a.563.563 0 00-.182.557l1.285 5.385a.562.562 0 01-.84.61l-4.725-2.885a.563.563 0 00-.586 0L6.982 20.54a.562.562 0 01-.84-.61l1.285-5.386a.562.562 0 00-.182-.557l-4.204-3.602a.563.563 0 01.321-.988l5.518-.442a.563.563 0 00.475-.345L11.48 3.5z" />
                </svg>
              </div>
              <div>
                <div className="text-sm text-accent-cyan mb-1">Winner</div>
                <div className="text-xl font-bold">{raffle.winner.displayName}</div>
              </div>
            </div>
          </motion.div>
        )}
      </div>

      <div className="glass rounded-2xl p-8">
        <h2 className="text-2xl font-bold mb-6">Participants</h2>
        {!participants || participants.length === 0 ? (
          <div className="text-center py-12">
            <div className="w-16 h-16 mx-auto mb-4 rounded-full bg-foreground/5 flex items-center justify-center">
              <svg className="w-8 h-8 text-foreground/30" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
                <path strokeLinecap="round" strokeLinejoin="round" d="M15 19.128a9.38 9.38 0 002.625.372 9.337 9.337 0 004.121-.952 4.125 4.125 0 00-7.533-2.493M15 19.128v-.003c0-1.113-.285-2.16-.786-3.07M15 19.128v.106A12.318 12.318 0 018.624 21c-2.331 0-4.512-.645-6.374-1.766l-.001-.109a6.375 6.375 0 0111.964-3.07M12 6.375a3.375 3.375 0 11-6.75 0 3.375 3.375 0 016.75 0zm8.25 2.25a2.625 2.625 0 11-5.25 0 2.625 2.625 0 015.25 0z" />
              </svg>
            </div>
            <p className="text-foreground/60">No participants yet</p>
          </div>
        ) : (
          <div className="space-y-3">
            {participants.map((p: Participant, i: number) => (
              <motion.div
                key={p.id}
                initial={{ opacity: 0, x: -20 }}
                animate={{ opacity: 1, x: 0 }}
                transition={{ delay: i * 0.05 }}
                className="glass-strong rounded-xl p-4 flex items-center justify-between hover:glow-border transition-all duration-300"
              >
                <div className="flex items-center gap-4">
                  <div className="w-10 h-10 rounded-full bg-gradient-to-br from-accent to-accent-cyan flex items-center justify-center">
                    <span className="text-sm font-bold text-white">
                      {p.displayName[0]?.toUpperCase() ?? "?"}
                    </span>
                  </div>
                  <div>
                    <div className="font-medium">{p.displayName}</div>
                    <div className="text-sm text-foreground/60">ID: {p.twitchUserId}</div>
                  </div>
                </div>
                {p.isSuspected && (
                  <span className="px-3 py-1 rounded-full text-xs font-medium bg-red-500/10 text-red-400 border border-red-500/20">
                    Suspected Bot
                  </span>
                )}
              </motion.div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
