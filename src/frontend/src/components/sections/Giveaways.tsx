"use client";

import { motion } from "framer-motion";
import { GIVEAWAYS, STATUS_CONFIG } from "@/constants";
import { fadeUpVariants, staggerContainerVariants } from "@/lib/animations";

export default function Giveaways() {
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
            Jump into live giveaways from your favorite streamers. Don&apos;t miss out — the clock is ticking.
          </p>
        </motion.div>

        <motion.div
          variants={staggerContainerVariants(0.08)}
          initial="hidden"
          whileInView="visible"
          viewport={{ once: true, margin: "-50px" }}
          className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8"
        >
          {GIVEAWAYS.map((g, i) => {
            const status = STATUS_CONFIG[g.status];
            const progress = (g.participants / g.maxParticipants) * 100;

            return (
              <motion.div
                key={i}
                variants={fadeUpVariants}
                className="group relative rounded-2xl glass overflow-hidden transition-all duration-500 hover:glow-border"
              >
                <div className="relative h-52 bg-surface-elevated overflow-hidden">
                  <div className="absolute inset-0 bg-gradient-to-br from-accent/20 via-transparent to-accent-cyan/20 opacity-50" />
                  <div className="absolute inset-0 flex items-center justify-center">
                    <div className="text-center">
                      <div className="w-16 h-16 mx-auto rounded-2xl bg-accent/10 border border-accent/20 flex items-center justify-center mb-3">
                        <svg className="w-8 h-8 text-accent/60" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
                          <path strokeLinecap="round" strokeLinejoin="round" d="M21 11.25v8.25a1.5 1.5 0 01-1.5 1.5H5.25a1.5 1.5 0 01-1.5-1.5v-8.25M12 4.875A2.625 2.625 0 109.375 7.5H12m0-2.625V7.5m0-2.625A2.625 2.625 0 1114.625 7.5H12m0 0V21m-8.625-9.75h18c.621 0 1.125-.504 1.125-1.125V9.75c0-.621-.504-1.125-1.125-1.125h-18c-.621 0-1.125.504-1.125 1.125v1.125c0 .621.504 1.125 1.125 1.125z" />
                        </svg>
                      </div>
                      <span className="text-xs text-foreground/25 font-mono uppercase tracking-wider">
                        [ Prize Image / GIF ]
                      </span>
                    </div>
                  </div>

                  <div className={`absolute top-4 right-4 px-3.5 py-1.5 rounded-full text-[10px] font-bold tracking-wider ${status.bg} ${status.color}`}>
                    {status.label}
                  </div>
                </div>

                <div className="p-8">
                  <div className="flex items-center justify-between mb-4">
                    <h3 className="font-semibold tracking-tight text-base">{g.title}</h3>
                  </div>

                  <div className="flex items-center gap-3 mb-5">
                    <div className="w-9 h-9 rounded-full bg-accent/20 flex items-center justify-center">
                      <span className="text-xs font-bold text-accent">
                        {g.streamer[0]}
                      </span>
                    </div>
                    <div>
                      <p className="text-sm font-medium">{g.streamer}</p>
                      <p className="text-xs text-foreground/30">{g.viewers} viewers</p>
                    </div>
                  </div>

                  <p className="text-sm text-foreground/40 mb-5">
                    Prize: <span className="text-foreground/70 font-medium">{g.prize}</span>
                  </p>

                  {g.status !== "upcoming" && (
                    <div className="mb-5">
                      <div className="flex justify-between text-xs mb-2.5">
                        <span className="text-foreground/30">
                          {g.participants.toLocaleString()} / {g.maxParticipants.toLocaleString()}
                        </span>
                        <span className="text-foreground/50">{Math.round(progress)}%</span>
                      </div>
                      <div className="h-2 rounded-full bg-foreground/5 overflow-hidden">
                        <motion.div
                          initial={{ width: 0 }}
                          whileInView={{ width: `${progress}%` }}
                          viewport={{ once: true }}
                          transition={{ duration: 1, delay: 0.3, ease: "easeOut" }}
                          className="h-full rounded-full bg-gradient-to-r from-accent to-accent-cyan"
                        />
                      </div>
                    </div>
                  )}

                  <button
                    disabled={g.status === "ended"}
                    className={`w-full py-3.5 px-6 rounded-xl text-sm font-medium transition-all duration-300 ${
                      g.status === "live" || g.status === "ending"
                        ? "bg-accent hover:bg-accent-light text-white hover:shadow-lg hover:shadow-accent/25 active:scale-95"
                        : g.status === "upcoming"
                        ? "glass text-foreground/60 hover:text-foreground hover:glow-border"
                        : "bg-foreground/5 text-foreground/20 cursor-not-allowed"
                    }`}
                  >
                    {g.status === "live" || g.status === "ending"
                      ? "Join Giveaway"
                      : g.status === "upcoming"
                      ? "Set Reminder"
                      : "Ended"}
                  </button>
                </div>
              </motion.div>
            );
          })}
        </motion.div>
      </div>
    </section>
  );
}
