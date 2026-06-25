"use client";

import { motion } from "framer-motion";
import useSWR from "swr";
import { api } from "@/lib/api";
import { useAuth } from "@/lib/auth";
import { Skeleton } from "@/components/ui/Skeleton";

export default function ProfilePage() {
  const { displayName } = useAuth();
  const { data: profile, isLoading } = useSWR("profile", api.profile.get, {
    revalidateOnFocus: false,
    revalidateOnReconnect: false,
  });

  if (isLoading) {
    return (
      <div className="max-w-4xl mx-auto">
        <div className="glass rounded-2xl p-8 mb-6">
          <div className="flex items-center gap-6">
            <Skeleton className="w-24 h-24 rounded-full" />
            <div className="flex-1 space-y-3">
              <Skeleton className="h-8 w-48" />
              <Skeleton className="h-4 w-32" />
            </div>
          </div>
        </div>
        <div className="glass rounded-2xl p-8">
          <Skeleton className="h-6 w-32 mb-6" />
          <div className="grid grid-cols-3 gap-4">
            <Skeleton className="h-24" />
            <Skeleton className="h-24" />
            <Skeleton className="h-24" />
          </div>
        </div>
      </div>
    );
  }

  if (!profile) {
    return (
      <div className="glass rounded-2xl p-12 text-center">
        <p className="text-foreground/60">Failed to load profile</p>
      </div>
    );
  }

  return (
    <div className="max-w-4xl mx-auto">
      <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} className="glass rounded-2xl p-8 mb-6">
        <div className="flex items-center gap-6">
          <div className="w-24 h-24 rounded-full bg-gradient-to-br from-accent to-accent-cyan flex items-center justify-center glow-border">
            <span className="text-3xl font-bold text-white">{displayName?.[0]?.toUpperCase() ?? "U"}</span>
          </div>
          <div>
            <h1 className="text-3xl font-bold mb-2">{profile.displayName}</h1>
            <p className="text-foreground/60">{profile.email}</p>
            <p className="text-sm text-foreground/40 mt-2">Member since {new Date(profile.createdAt).toLocaleDateString()}</p>
          </div>
        </div>
      </motion.div>

      <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.1 }} className="glass rounded-2xl p-8 mb-6">
        <h2 className="text-2xl font-bold mb-6">Statistics</h2>
        <div className="grid grid-cols-3 gap-4">
          <div className="glass-strong rounded-xl p-6 text-center">
            <div className="text-3xl font-bold text-gradient mb-2">{profile.rafflesCreated}</div>
            <div className="text-foreground/60 text-sm">Raffles Created</div>
          </div>
          <div className="glass-strong rounded-xl p-6 text-center">
            <div className="text-3xl font-bold text-gradient mb-2">{profile.rafflesParticipated}</div>
            <div className="text-foreground/60 text-sm">Participated</div>
          </div>
          <div className="glass-strong rounded-xl p-6 text-center">
            <div className="text-3xl font-bold text-gradient mb-2">{profile.wins}</div>
            <div className="text-foreground/60 text-sm">Wins</div>
          </div>
        </div>
      </motion.div>

      <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.2 }} className="glass rounded-2xl p-8">
        <h2 className="text-2xl font-bold mb-6">Account Information</h2>
        <div className="space-y-4">
          <div className="glass-strong rounded-xl p-4">
            <div className="text-foreground/60 text-sm mb-1">Twitch ID</div>
            <div className="font-medium">{profile.twitchId}</div>
          </div>
          <div className="glass-strong rounded-xl p-4">
            <div className="text-foreground/60 text-sm mb-1">Display Name</div>
            <div className="font-medium">{profile.displayName}</div>
          </div>
          <div className="glass-strong rounded-xl p-4">
            <div className="text-foreground/60 text-sm mb-1">Email</div>
            <div className="font-medium">{profile.email}</div>
          </div>
        </div>
      </motion.div>
    </div>
  );
}
