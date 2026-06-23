"use client";

import { useMemo } from "react";
import { useRouter } from "next/navigation";
import { motion } from "framer-motion";
import useSWR from "swr";
import { api } from "@/lib/api";
import Badge from "@/components/ui/Badge";
import { CardSkeleton } from "@/components/ui/Skeleton";
import GiftIcon from "@/components/ui/GiftIcon";

export default function Giveaways() {
  const router = useRouter();
  const { data: allRaffles, isLoading } = useSWR("giveaways-top", api.raffles.listAll);

  const raffles = useMemo(() => {
    if (!allRaffles) return [];
    return allRaffles
      .filter((r) => r.status === "Active")
      .sort((a, b) => b.participantCount - a.participantCount)
      .slice(0, 6);
  }, [allRaffles]);

  return (
    <section id="giveaways" className="relative py-32 px-8">
      <div className="absolute inset-0 bg-gradient-to-b from-transparent via-accent/[0.02] to-transparent" />

      <div className="relative max-w-7xl mx-auto">
        <motion.div
          initial={{ opacity: 0, y: 30 }}
          whileInView={{ opacity: 1, y: 0 }}
          viewport={{ once: true, margin: "-100px" }}
          transition={{ duration: 0.8 }}
          className="text-center mb-24"
        >
          <span className="text-xs uppercase tracking-[0.2em] text-accent-cyan/80 font-medium mb-5 block">
            Live Now
          </span>
          <h2 className="text-4xl md:text-5xl font-bold tracking-tight mb-7">
            Active giveaways
            <br />
            <span className="text-gradient">happening right now</span>
          </h2>
          <p className="text-foreground/40 max-w-xl mx-auto text-lg leading-relaxed">
            Jump into live giveaways from your favorite streamers.
          </p>
        </motion.div>

        {isLoading ? (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
            {[1, 2, 3, 4, 5, 6].map((i) => (
              <CardSkeleton key={i} />
            ))}
          </div>
        ) : raffles.length > 0 ? (
          <>
            <motion.div
              initial={{ opacity: 0 }}
              whileInView={{ opacity: 1 }}
              viewport={{ once: true }}
              transition={{ duration: 0.8 }}
              className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8 mb-12"
            >
              {raffles.map((raffle, i) => (
                <motion.div
                  key={raffle.id}
                  initial={{ opacity: 0, y: 30 }}
                  whileInView={{ opacity: 1, y: 0 }}
                  viewport={{ once: true, margin: "-50px" }}
                  transition={{ duration: 0.6, delay: i * 0.1 }}
                  className="group relative rounded-2xl glass overflow-hidden transition-all duration-500 hover:glow-border"
                >
                  <div className="relative h-52 bg-surface-elevated overflow-hidden">
                    <div className="absolute inset-0 bg-gradient-to-br from-accent/20 via-transparent to-accent-cyan/20 opacity-50" />
                    <div className="absolute inset-0 flex items-center justify-center">
                      <div className="text-center">
                        <div className="w-16 h-16 mx-auto rounded-2xl bg-accent/10 border border-accent/20 flex items-center justify-center mb-3">
                          <GiftIcon className="w-8 h-8 text-accent/60" />
                        </div>
                      </div>
                    </div>
                    <div className="absolute top-4 right-4">
                      <Badge status={raffle.status} />
                    </div>
                  </div>

                  <div className="p-8">
                    <h3 className="font-semibold tracking-tight text-base mb-4">{raffle.title}</h3>

                    <div className="flex items-center gap-3 mb-5">
                      <div className="w-9 h-9 rounded-full bg-accent/20 flex items-center justify-center">
                        <span className="text-xs font-bold text-accent">
                          {raffle.streamerName[0]?.toUpperCase()}
                        </span>
                      </div>
                      <div>
                        <p className="text-sm font-medium">{raffle.streamerName}</p>
                      </div>
                    </div>

                    <div className="mb-5">
                      <span className="text-xs text-foreground/30">
                        {raffle.participantCount.toLocaleString()} participants
                      </span>
                    </div>

                    <button
                      onClick={() => router.push(`/raffles/${raffle.id}`)}
                      className="w-full py-3.5 px-6 rounded-xl text-sm font-medium transition-all duration-300 bg-accent hover:bg-accent-light text-white hover:shadow-lg hover:shadow-accent/25 active:scale-95"
                    >
                      View Details
                    </button>
                  </div>
                </motion.div>
              ))}
            </motion.div>

            <motion.div
              initial={{ opacity: 0, y: 20 }}
              whileInView={{ opacity: 1, y: 0 }}
              viewport={{ once: true }}
              transition={{ duration: 0.6 }}
              className="text-center"
            >
              <button
                onClick={() => router.push("/raffles")}
                className="group px-8 py-4 rounded-xl glass text-foreground/80 font-medium transition-all duration-300 hover:text-foreground hover:glow-border inline-flex items-center gap-3"
              >
                View All Giveaways
                <svg
                  className="w-4 h-4 group-hover:translate-x-1 transition-transform duration-300"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                  strokeWidth={2}
                >
                  <path strokeLinecap="round" strokeLinejoin="round" d="M9 5l7 7-7 7" />
                </svg>
              </button>
            </motion.div>
          </>
        ) : (
          <div className="text-center py-16">
            <p className="text-foreground/40 text-lg">No active giveaways at the moment</p>
          </div>
        )}
      </div>
    </section>
  );
}
