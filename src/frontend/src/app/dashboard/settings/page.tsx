"use client";

import { useState } from "react";
import { motion } from "framer-motion";
import useSWR from "swr";
import { api } from "@/lib/api";
import { useAuth } from "@/lib/auth";
import Toggle from "@/components/ui/Toggle";
import Button from "@/components/ui/Button";
import { Skeleton } from "@/components/ui/Skeleton";

export default function SettingsPage() {
  useAuth();
  const { data: profile, isLoading } = useSWR("profile", api.profile.get);
  
  const [notifications, setNotifications] = useState({
    emailNotifications: true,
    raffleUpdates: true,
    winnerAnnouncements: true,
  });
  const [saved, setSaved] = useState(false);

  const handleSave = () => {
    setSaved(true);
    setTimeout(() => setSaved(false), 3000);
  };

  if (isLoading) {
    return (
      <div className="max-w-4xl mx-auto">
        <div className="glass rounded-2xl p-8 mb-6">
          <Skeleton className="h-8 w-48 mb-6" />
          <div className="space-y-4">
            <Skeleton className="h-16" />
            <Skeleton className="h-16" />
            <Skeleton className="h-16" />
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
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
      >
        <h1 className="text-4xl font-bold mb-2">
          <span className="text-gradient">Settings</span>
        </h1>
        <p className="text-foreground/60 mb-8">Manage your account preferences</p>
      </motion.div>

      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay: 0.1 }}
        className="glass rounded-2xl p-8 mb-6"
      >
        <h2 className="text-2xl font-bold mb-6">Notifications</h2>
        <div className="space-y-4">
          <Toggle
            label="Email Notifications"
            description="Receive email updates about your raffles"
            checked={notifications.emailNotifications}
            onChange={(checked) => setNotifications({ ...notifications, emailNotifications: checked })}
          />
          <Toggle
            label="Raffle Updates"
            description="Get notified when participants join"
            checked={notifications.raffleUpdates}
            onChange={(checked) => setNotifications({ ...notifications, raffleUpdates: checked })}
          />
          <Toggle
            label="Winner Announcements"
            description="Receive notifications when winners are drawn"
            checked={notifications.winnerAnnouncements}
            onChange={(checked) => setNotifications({ ...notifications, winnerAnnouncements: checked })}
          />
        </div>
      </motion.div>

      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay: 0.2 }}
        className="glass rounded-2xl p-8 mb-6"
      >
        <h2 className="text-2xl font-bold mb-6">Account</h2>
        <div className="glass-strong rounded-xl p-4">
          <div className="text-foreground/60 text-sm mb-1">Connected Account</div>
          <div className="flex items-center gap-3">
            <svg className="w-6 h-6 text-[#9146ff]" viewBox="0 0 24 24" fill="currentColor">
              <path d="M11.571 4.714h1.715v5.143H11.57zm4.715 0H18v5.143h-1.714zM6 0L1.714 4.286v15.428h5.143V24l4.286-4.286h3.428L22.286 12V0zm14.571 11.143l-3.428 3.428h-3.429l-3 3v-3H6.857V1.714h13.714z" />
            </svg>
            <span className="font-medium">Twitch</span>
          </div>
        </div>
      </motion.div>

      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay: 0.3 }}
        className="flex items-center justify-between"
      >
        {saved && (
          <motion.div
            initial={{ opacity: 0, x: -20 }}
            animate={{ opacity: 1, x: 0 }}
            className="text-green-400 text-sm"
          >
            Settings saved successfully!
          </motion.div>
        )}
        <Button onClick={handleSave} className="ml-auto">
          Save Changes
        </Button>
      </motion.div>
    </div>
  );
}
