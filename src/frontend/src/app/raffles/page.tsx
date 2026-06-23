"use client";

import { useRouter } from "next/navigation";
import { motion } from "framer-motion";
import useSWR from "swr";
import { api } from "@/lib/api";
import Badge from "@/components/ui/Badge";
import { CardSkeleton } from "@/components/ui/Skeleton";
import GiftIcon from "@/components/ui/GiftIcon";
import BackButton from "@/components/ui/BackButton";
import CosmicBackground from "@/components/ui/CosmicBackground";
import type { Raffle } from "@/types/api";

export default function RafflesCatalogPage() {
  const router = useRouter();
  const { data: raffles, isLoading } = useSWR("all-raffles", api.raffles.listAll);

  const filteredRaffles = raffles?.filter((r) => r.status === "Active");

  return (
    <div className="min-h-screen bg-background relative overflow-hidden">
      <CosmicBackground />

      <div className="relative z-10 max-w-7xl mx-auto px-8 py-12">
        <BackButton onClick={() => router.push("/")}>Back to Home</BackButton>

        <div className="mb-12">
          <h1 className="text-4xl font-bold tracking-tight mb-4">
            Browse <span className="text-gradient">Raffles</span>
          </h1>
          <p className="text-foreground/60 text-lg">Discover active giveaways and join the fun</p>
        </div>

        {isLoading ? (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {[1, 2, 3, 4, 5, 6].map((i) => (
              <CardSkeleton key={i} />
            ))}
          </div>
        ) : filteredRaffles && filteredRaffles.length > 0 ? (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {filteredRaffles.map((raffle: Raffle, index: number) => (
              <motion.div
                key={raffle.id}
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ delay: index * 0.05 }}
                className="group rounded-2xl glass overflow-hidden hover:glow-border transition-all duration-300"
              >
                <div className="relative h-48 bg-surface-elevated overflow-hidden">
                  <div className="absolute inset-0 bg-gradient-to-br from-accent/20 via-transparent to-accent-cyan/20 opacity-50" />
                  <div className="absolute inset-0 flex items-center justify-center">
                    <div className="w-16 h-16 mx-auto rounded-2xl bg-accent/10 border border-accent/20 flex items-center justify-center">
                      <GiftIcon className="w-8 h-8 text-accent/60" />
                    </div>
                  </div>
                  <div className="absolute top-4 right-4">
                    <Badge status={raffle.status} />
                  </div>
                </div>

                <div className="p-6">
                  <h3 className="text-lg font-semibold mb-2">{raffle.title}</h3>
                  <p className="text-sm text-foreground/40 mb-4">by {raffle.streamerName}</p>
                  <div className="flex items-center justify-between mb-4">
                    <span className="text-sm text-foreground/50">
                      {raffle.participantCount.toLocaleString()} participants
                    </span>
                  </div>

                  {raffle.winner && (
                    <div className="mb-4 p-3 rounded-xl bg-accent-cyan/10 border border-accent-cyan/20">
                      <p className="text-xs text-accent-cyan">
                        Winner: <span className="font-semibold">{raffle.winner.displayName}</span>
                      </p>
                    </div>
                  )}

                  <button
                    onClick={() => router.push(`/raffles/${raffle.id}`)}
                    className="w-full px-4 py-2.5 rounded-xl glass text-sm font-medium hover:bg-foreground/5 transition-all"
                  >
                    View Details
                  </button>
                </div>
              </motion.div>
            ))}
          </div>
        ) : (
          <div className="p-12 rounded-2xl glass text-center">
            <p className="text-foreground/60">No raffles found</p>
          </div>
        )}
      </div>
    </div>
  );
}
