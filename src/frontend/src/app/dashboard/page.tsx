"use client";

import { useState, useCallback } from "react";
import { useRouter } from "next/navigation";
import { motion } from "framer-motion";
import useSWR from "swr";
import { api } from "@/lib/api";
import Modal from "@/components/ui/Modal";
import Button from "@/components/ui/Button";
import Badge from "@/components/ui/Badge";
import GiftIcon from "@/components/ui/GiftIcon";
import { CardSkeleton } from "@/components/ui/Skeleton";
import { logger } from "@/lib/logger";
import type { Raffle } from "@/types/api";

export default function DashboardPage() {
  const router = useRouter();
  const { data: raffles, isLoading, mutate } = useSWR("my-raffles", api.raffles.listMy);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [newTitle, setNewTitle] = useState("");
  const [creating, setCreating] = useState(false);

  const handleCreate = useCallback(async () => {
    if (!newTitle.trim()) return;
    setCreating(true);
    try {
      await api.raffles.create({ title: newTitle.trim() });
      setShowCreateModal(false);
      setNewTitle("");
      mutate();
    } catch (err) {
      logger.error("Failed to create raffle", err);
    } finally {
      setCreating(false);
    }
  }, [newTitle, mutate]);

  const handleAction = useCallback(async (action: () => Promise<unknown>) => {
    try {
      await action();
      mutate();
    } catch (err) {
      logger.error("Raffle action failed", err);
    }
  }, [mutate]);

  return (
    <div className="max-w-7xl mx-auto">
      <div className="flex items-center justify-between mb-8">
        <div>
          <h1 className="text-4xl font-bold tracking-tight mb-2">
            My <span className="text-gradient">Raffles</span>
          </h1>
          <p className="text-foreground/60">Manage your giveaways and track participants</p>
        </div>
        <Button onClick={() => setShowCreateModal(true)}>
          <svg className="w-5 h-5 mr-2 inline" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
            <path strokeLinecap="round" strokeLinejoin="round" d="M12 4v16m8-8H4" />
          </svg>
          Create Raffle
        </Button>
      </div>

      {isLoading ? (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {[1, 2, 3].map((i) => <CardSkeleton key={i} />)}
        </div>
      ) : raffles && raffles.length > 0 ? (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {raffles.map((raffle: Raffle, index: number) => (
            <motion.div
              key={raffle.id}
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ delay: index * 0.1 }}
              className="glass rounded-2xl p-6 hover:glow-border transition-all duration-300 group"
            >
              <div className="flex items-start justify-between mb-4">
                <div className="flex-1">
                  <h3 className="text-lg font-semibold mb-2 group-hover:text-accent transition-colors">
                    {raffle.title}
                  </h3>
                  <Badge status={raffle.status} />
                </div>
              </div>

              <div className="space-y-3 mb-6">
                <div className="flex items-center gap-2 text-sm text-foreground/60">
                  <svg className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
                    <path strokeLinecap="round" strokeLinejoin="round" d="M15 19.128a9.38 9.38 0 002.625.372 9.337 9.337 0 004.121-.952 4.125 4.125 0 00-7.533-2.493M15 19.128v-.003c0-1.113-.285-2.16-.786-3.07M15 19.128v.106A12.318 12.318 0 018.624 21c-2.331 0-4.512-.645-6.374-1.766l-.001-.109a6.375 6.375 0 0111.964-3.07M12 6.375a3.375 3.375 0 11-6.75 0 3.375 3.375 0 016.75 0zm8.25 2.25a2.625 2.625 0 11-5.25 0 2.625 2.625 0 015.25 0z" />
                  </svg>
                  <span>{raffle.participantCount} participants</span>
                </div>
                {raffle.winner && (
                  <div className="flex items-center gap-2 text-sm text-accent-cyan">
                    <svg className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
                      <path strokeLinecap="round" strokeLinejoin="round" d="M11.48 3.499a.562.562 0 011.04 0l2.125 5.111a.563.563 0 00.475.345l5.518.442c.499.04.701.663.321.988l-4.204 3.602a.563.563 0 00-.182.557l1.285 5.385a.562.562 0 01-.84.61l-4.725-2.885a.563.563 0 00-.586 0L6.982 20.54a.562.562 0 01-.84-.61l1.285-5.386a.562.562 0 00-.182-.557l-4.204-3.602a.563.563 0 01.321-.988l5.518-.442a.563.563 0 00.475-.345L11.48 3.5z" />
                    </svg>
                    <span>Winner: {raffle.winner.displayName}</span>
                  </div>
                )}
              </div>

              <div className="flex gap-2">
                <Button
                  variant="secondary"
                  size="sm"
                  className="flex-1"
                  onClick={() => router.push(`/dashboard/raffles/${raffle.id}`)}
                >
                  Manage
                </Button>
                {raffle.status === "Pending" && (
                  <Button variant="primary" size="sm" onClick={() => handleAction(() => api.raffles.start(raffle.id))}>
                    Start
                  </Button>
                )}
                {raffle.status === "Active" && (
                  <Button variant="secondary" size="sm" onClick={() => handleAction(() => api.raffles.finish(raffle.id))}>
                    Finish
                  </Button>
                )}
                {raffle.status === "Finished" && (
                  <Button variant="primary" size="sm" onClick={() => handleAction(() => api.raffles.draw(raffle.id))}>
                    Draw
                  </Button>
                )}
              </div>
            </motion.div>
          ))}
        </div>
      ) : (
        <div className="glass rounded-2xl p-12 text-center">
          <div className="w-20 h-20 mx-auto mb-6 rounded-full bg-accent/10 flex items-center justify-center">
            <GiftIcon className="w-10 h-10 text-accent" />
          </div>
          <h3 className="text-xl font-semibold mb-2">No raffles yet</h3>
          <p className="text-foreground/60 mb-6">Create your first raffle to get started</p>
          <Button onClick={() => setShowCreateModal(true)}>Create Your First Raffle</Button>
        </div>
      )}

      <Modal isOpen={showCreateModal} onClose={() => setShowCreateModal(false)}>
        <div>
          <h2 className="text-2xl font-bold mb-6">Create New Raffle</h2>
          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium mb-2 text-foreground/80">
                Raffle Title
              </label>
              <input
                type="text"
                value={newTitle}
                onChange={(e) => setNewTitle(e.target.value)}
                placeholder="Enter raffle title..."
                className="w-full px-4 py-3 rounded-xl glass bg-transparent text-foreground placeholder:text-foreground/30 focus:outline-none focus:ring-2 focus:ring-accent/50 transition-all"
                maxLength={100}
              />
            </div>
            <div className="flex gap-3 pt-4">
              <Button variant="ghost" onClick={() => setShowCreateModal(false)} className="flex-1">
                Cancel
              </Button>
              <Button onClick={handleCreate} disabled={!newTitle.trim() || creating} className="flex-1">
                {creating ? "Creating..." : "Create"}
              </Button>
            </div>
          </div>
        </div>
      </Modal>
    </div>
  );
}
