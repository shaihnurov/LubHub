"use client";

import { motion } from "framer-motion";
import { useAuth } from "@/lib/auth";
import { TwitchIcon } from "@/components/ui/TwitchIcon";

export default function CTA() {
  const { isAuthenticated, loginWithTwitch } = useAuth();

  return (
    <section className="relative py-32 px-8 overflow-hidden">
      <div className="absolute inset-0">
        <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[800px] h-[800px] rounded-full opacity-20"
          style={{ background: "radial-gradient(circle, #9333ea 0%, transparent 60%)" }}
        />
        <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[600px] h-[600px] rounded-full opacity-10"
          style={{ background: "radial-gradient(circle, #06b6d4 0%, transparent 60%)" }}
        />
      </div>

      <div className="relative max-w-3xl mx-auto text-center">
        <motion.div
          initial={{ opacity: 0, y: 30 }}
          whileInView={{ opacity: 1, y: 0 }}
          viewport={{ once: true, margin: "-100px" }}
          transition={{ duration: 0.8 }}
        >
          <h2 className="text-4xl md:text-6xl font-bold tracking-tight mb-8">
            Ready to give back
            <br />
            <span className="text-gradient">to your community?</span>
          </h2>
          <p className="text-foreground/40 text-lg mb-12 max-w-xl mx-auto leading-relaxed">
            Join other streamers who are already using LubHub to create unforgettable moments.
          </p>

          <button
            onClick={loginWithTwitch}
            className="group relative inline-flex items-center gap-3 px-12 py-6 rounded-2xl bg-[#9146ff] text-white font-semibold text-lg transition-all duration-500 hover:shadow-2xl hover:shadow-[#9146ff]/30 active:scale-95 overflow-hidden"
          >
            <span className="relative z-10 flex items-center gap-3">
              <TwitchIcon size={24} />
              {isAuthenticated ? "Go to Dashboard" : "Connect with Twitch"}
            </span>
            <div className="absolute inset-0 bg-gradient-to-r from-[#9146ff] to-[#7c3aed] opacity-0 group-hover:opacity-100 transition-opacity duration-500" />
          </button>

          <p className="mt-8 text-xs text-foreground/20">
            Free to start. No credit card required.
          </p>
        </motion.div>
      </div>
    </section>
  );
}
