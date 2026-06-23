"use client";

import { useMemo } from "react";
import { useRouter } from "next/navigation";
import { motion } from "framer-motion";
import { HERO_STATS } from "@/constants";
import { EASE_OUT } from "@/lib/animations";
import { useAuth } from "@/lib/auth";
import { TwitchIcon } from "@/components/ui/TwitchIcon";

function seededRandom(seed: number) {
  let s = seed;
  return () => {
    s = (s * 16807) % 2147483647;
    return (s - 1) / 2147483646;
  };
}

function generateStars(count: number, seed: number) {
  const rand = seededRandom(seed);
  const stars = [];
  for (let i = 0; i < count; i++) {
    const isLarge = rand() > 0.8;
    stars.push({
      id: i,
      left: rand() * 100,
      top: rand() * 100,
      size: isLarge ? "lg" : "sm",
      duration: rand() * 4 + 2,
      delay: rand() * 6,
    });
  }
  return stars;
}

const comets = [
  { id: 0, top: 12, angle: -4, delay: 0 },
  { id: 1, top: 38, angle: -7, delay: 5 },
  { id: 2, top: 62, angle: -3, delay: 10 },
];

export default function Hero() {
  const router = useRouter();
  const stars = useMemo(() => generateStars(20, 42), []);
  const { isAuthenticated, loginWithTwitch } = useAuth();

  const handlePrimaryAction = () => {
    if (isAuthenticated) {
      router.push("/dashboard");
    } else {
      loginWithTwitch();
    }
  };

  return (
    <section className="relative min-h-screen flex items-center justify-center overflow-hidden">
      <div className="absolute inset-0 bg-grid" />

      <div className="absolute inset-0" style={{ contain: "layout style paint" }}>
        {stars.map((s) => (
          <div
            key={`s-${s.id}`}
            className={`star ${s.size === "lg" ? "star-lg" : ""}`}
            style={{
              left: `${s.left}%`,
              top: `${s.top}%`,
              animation: `twinkle ${s.duration}s ease-in-out ${s.delay}s infinite`,
            }}
          />
        ))}

        {comets.map((c) => (
          <div
            key={`c-${c.id}`}
            className="comet"
            style={{
              top: `${c.top}%`,
              rotate: `${c.angle}deg`,
              animation: `comet-fly 15s linear ${c.delay}s infinite`,
            }}
          />
        ))}
      </div>

      <div className="absolute inset-0 bg-gradient-to-b from-transparent via-transparent to-[#050507]" />

      <div className="relative z-10 max-w-5xl mx-auto px-8 text-center">
        <motion.div
          initial={{ opacity: 0, y: 40 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 1, ease: EASE_OUT }}
        >
          <div className="inline-flex items-center gap-2.5 px-5 py-2.5 rounded-full glass mb-8">
            <span className="w-2 h-2 rounded-full bg-green-400 animate-pulse" />
            <span className="text-xs text-foreground/70 font-medium tracking-wide uppercase">
              Now in Public Beta
            </span>
          </div>
        </motion.div>

        <motion.h1
          initial={{ opacity: 0, y: 40 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 1, delay: 0.15, ease: EASE_OUT }}
          className="text-5xl sm:text-6xl md:text-7xl lg:text-8xl font-bold tracking-tight leading-[1.05] mb-8"
        >
          An
          <span className="text-gradient"> open-source</span> platform
          for running giveaways
        </motion.h1>

        <motion.p
          initial={{ opacity: 0, y: 30 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 1, delay: 0.3, ease: EASE_OUT }}
          className="text-lg md:text-xl text-foreground/50 max-w-2xl mx-auto mb-12 leading-relaxed"
        >
          LubHub empowers streamers to create thrilling, fair giveaways
          that keep your chat engaged and your community coming back for more.
        </motion.p>

        <motion.div
          initial={{ opacity: 0, y: 30 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 1, delay: 0.45, ease: EASE_OUT }}
          className="flex flex-col sm:flex-row items-center justify-center gap-5"
        >
          <button
            onClick={handlePrimaryAction}
            className="group relative px-10 py-5 rounded-2xl bg-[#9146ff] text-white font-semibold text-base transition-all duration-500 hover:shadow-2xl hover:shadow-[#9146ff]/30 active:scale-95 overflow-hidden"
          >
            <span className="relative z-10 flex items-center gap-3">
              <TwitchIcon size={20} />
              {isAuthenticated ? "Go to Dashboard" : "Start with Twitch"}
            </span>
            <div className="absolute inset-0 bg-gradient-to-r from-[#9146ff] to-[#7c3aed] opacity-0 group-hover:opacity-100 transition-opacity duration-500" />
          </button>

          <a
            href="#features"
            className="group px-10 py-5 rounded-2xl glass text-foreground/80 font-medium text-base transition-all duration-300 hover:text-foreground hover:border-foreground/20 flex items-center gap-3"
          >
            Learn more
            <svg
              className="w-4 h-4 group-hover:translate-y-0.5 transition-transform duration-300"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
              strokeWidth={2}
            >
              <path strokeLinecap="round" strokeLinejoin="round" d="M19 9l-7 7-7-7" />
            </svg>
          </a>
        </motion.div>

        <motion.div
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ duration: 1.5, delay: 1 }}
          className="mt-24 flex items-center justify-center gap-14 text-foreground/30"
        >
          {HERO_STATS.map((stat, i) => (
            <div key={stat.label} className="flex items-center gap-14">
              {i > 0 && <div className="w-px h-8 bg-foreground/10" />}
              <div className="flex flex-col items-center gap-2">
                <span className="text-2xl font-bold text-foreground/60">{stat.value}</span>
                <span className="text-xs uppercase tracking-wider">{stat.label}</span>
              </div>
            </div>
          ))}
        </motion.div>
      </div>
    </section>
  );
}
