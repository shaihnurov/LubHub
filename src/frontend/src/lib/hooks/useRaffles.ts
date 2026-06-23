"use client";

import { useCallback } from "react";
import useSWR from "swr";
import { api, ApiClientError } from "@/lib/api";
import type { WinnerResponse } from "@/types/api";

export function useCreateRaffle() {
  return useCallback(async (title: string) => {
    return api.raffles.create({ title });
  }, []);
}

export function useStartRaffle() {
  return useCallback(async (id: number) => {
    return api.raffles.start(id);
  }, []);
}

export function useFinishRaffle() {
  return useCallback(async (id: number) => {
    return api.raffles.finish(id);
  }, []);
}

export function useDrawRaffle(id: number | null) {
  const { data, error, isLoading, mutate } = useSWR<WinnerResponse>(
    id ? `raffles/${id}/draw` : null,
    () => api.raffles.draw(id!),
    { revalidateOnFocus: false, revalidateIfStale: false },
  );

  return { winner: data, error, isLoading, draw: mutate };
}

export function useJoinRaffle() {
  return useCallback(async (id: number) => {
    return api.raffles.join(id);
  }, []);
}

export function useApiError(): (err: unknown) => string {
  return useCallback((err: unknown) => {
    if (err instanceof ApiClientError) {
      return err.message;
    }
    if (err instanceof Error) {
      return err.message;
    }
    return "An unexpected error occurred";
  }, []);
}
