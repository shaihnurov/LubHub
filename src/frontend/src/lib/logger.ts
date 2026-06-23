const isDev = process.env.NODE_ENV === "development";

export const logger = {
  error: (message: string, error?: unknown) => {
    if (isDev) {
      console.error(`[LubHub] ${message}`, error ?? "");
    }
  },
  warn: (message: string, data?: unknown) => {
    if (isDev) {
      console.warn(`[LubHub] ${message}`, data ?? "");
    }
  },
  info: (message: string, data?: unknown) => {
    if (isDev) {
      console.info(`[LubHub] ${message}`, data ?? "");
    }
  },
};
