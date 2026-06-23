import type { Feature, Giveaway, NavLink, StatusConfig } from "@/types";
import type { RaffleStatus } from "@/types";

export const NAV_LINKS: NavLink[] = [
  { label: "Features", href: "#features" },
  { label: "Giveaways", href: "#giveaways" },
  { label: "About", href: "#about" },
];

export const HERO_STATS = [
  { value: "value", label: "Streamers" },
  { value: "value", label: "Giveaways" },
  { value: "value", label: "Winners" },
] as const;

export const FEATURES: Feature[] = [
  {
    icon: (
      <svg className="w-7 h-7" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
        <path strokeLinecap="round" strokeLinejoin="round" d="M9.813 15.904L9 18.75l-.813-2.846a4.5 4.5 0 00-3.09-3.09L2.25 12l2.846-.813a4.5 4.5 0 003.09-3.09L9 5.25l.813 2.846a4.5 4.5 0 003.09 3.09L15.75 12l-2.846.813a4.5 4.5 0 00-3.09 3.09zM18.259 8.715L18 9.75l-.259-1.035a3.375 3.375 0 00-2.455-2.456L14.25 6l1.036-.259a3.375 3.375 0 002.455-2.456L18 2.25l.259 1.035a3.375 3.375 0 002.455 2.456L21.75 6l-1.036.259a3.375 3.375 0 00-2.455 2.456z" />
      </svg>
    ),
    title: "Instant Setup",
    description: "Launch a giveaway in under 30 seconds. No complex forms, no friction. Just pure excitement for your viewers.",
  },
  {
    icon: (
      <svg className="w-7 h-7" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
        <path strokeLinecap="round" strokeLinejoin="round" d="M16.5 18.75h-9m9 0a3 3 0 013 3h-15a3 3 0 013-3m9 0v-4.5A3.375 3.375 0 0013.125 10.875h-2.25A3.375 3.375 0 007.5 14.25v4.5m6-15a3 3 0 11-6 0 3 3 0 016 0z" />
      </svg>
    ),
    title: "Fair & Transparent",
    description: "Provably fair random selection. Every participant has an equal chance, and results are verifiable by anyone.",
  },
  {
    icon: (
      <svg className="w-7 h-7" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
        <path strokeLinecap="round" strokeLinejoin="round" d="M3 13.125C3 12.504 3.504 12 4.125 12h2.25c.621 0 1.125.504 1.125 1.125v6.75C7.5 20.496 6.996 21 6.375 21h-2.25A1.125 1.125 0 013 19.875v-6.75zM9.75 8.625c0-.621.504-1.125 1.125-1.125h2.25c.621 0 1.125.504 1.125 1.125v11.25c0 .621-.504 1.125-1.125 1.125h-2.25a1.125 1.125 0 01-1.125-1.125V8.625zM16.5 4.125c0-.621.504-1.125 1.125-1.125h2.25C20.496 3 21 3.504 21 4.125v15.75c0 .621-.504 1.125-1.125 1.125h-2.25a1.125 1.125 0 01-1.125-1.125V4.125z" />
      </svg>
    ),
    title: "Real-Time Engagement",
    description: "Live participant counter, animated roll sequences, and chat integration that keeps your audience on the edge of their seats.",
  },
  {
    icon: (
      <svg className="w-7 h-7" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
        <path strokeLinecap="round" strokeLinejoin="round" d="M21 12a2.25 2.25 0 00-2.25-2.25H15a3 3 0 11-6 0H5.25A2.25 2.25 0 003 12m18 0v6a2.25 2.25 0 01-2.25 2.25H5.25A2.25 2.25 0 013 18v-6m18 0V9M3 12V9m18 0a2.25 2.25 0 00-2.25-2.25H5.25A2.25 2.25 0 013 9m18 0V6a2.25 2.25 0 00-2.25-2.25H5.25A2.25 2.25 0 013 6v3" />
      </svg>
    ),
    title: "Prize Templates",
    description: "Save your favorite giveaway configurations. Game keys, merch, gift cards — set it up once, reuse forever.",
  },
  {
    icon: (
      <svg className="w-7 h-7" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
        <path strokeLinecap="round" strokeLinejoin="round" d="M7.5 21L3 16.5m0 0L7.5 12M3 16.5H13.5m0-13.5L18 7.5m0 0L13.5 12M18 7.5H7.5" />
      </svg>
    ),
    title: "Subscriber Bonus",
    description: "Give subs extra entries as a thank-you. Configurable multipliers let you reward your most loyal supporters.",
  },
  {
    icon: (
      <svg className="w-7 h-7" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.5}>
        <path strokeLinecap="round" strokeLinejoin="round" d="M9 12.75L11.25 15 15 9.75m-3-7.036A11.959 11.959 0 013.598 6 11.99 11.99 0 003 9.749c0 5.592 3.824 10.29 9 11.623 5.176-1.332 9-6.03 9-11.622 0-1.31-.21-2.571-.598-3.751h-.152c-3.196 0-6.1-1.248-8.25-3.285z" />
      </svg>
    ),
    title: "Anti-Fraud",
    description: "Smart detection filters bots and alt accounts. Only real viewers participate, keeping giveaways legit.",
  },
];

export const GIVEAWAYS: Giveaway[] = [
  {
    title: "Steam Key Giveaway",
    streamer: "xQC",
    viewers: "42.3K",
    prize: "Elden Ring DLC Key",
    status: "live",
    participants: 8742,
    maxParticipants: 10000,
  },
  {
    title: "Merch Drop",
    streamer: "Pokimane",
    viewers: "18.7K",
    prize: "Exclusive Hoodie Bundle",
    status: "live",
    participants: 3291,
    maxParticipants: 5000,
  },
  {
    title: "Gift Card Frenzy",
    streamer: "Shroud",
    viewers: "28.1K",
    prize: "$100 Amazon Gift Card x5",
    status: "ending",
    participants: 9650,
    maxParticipants: 10000,
  },
  {
    title: "Console Giveaway",
    streamer: "TimTheTatman",
    viewers: "35.6K",
    prize: "PS5 Pro Bundle",
    status: "upcoming",
    participants: 0,
    maxParticipants: 15000,
  },
  {
    title: "Keyboard Drop",
    streamer: "Summit1g",
    viewers: "12.4K",
    prize: "Custom Mechanical Keyboard",
    status: "upcoming",
    participants: 0,
    maxParticipants: 8000,
  },
  {
    title: "Game Pass Month",
    streamer: "Lirik",
    viewers: "9.8K",
    prize: "Xbox Game Pass Ultimate 3-Month",
    status: "ended",
    participants: 7200,
    maxParticipants: 7200,
  },
];

export const STATUS_CONFIG: Record<RaffleStatus, StatusConfig> = {
  live: { label: "LIVE", color: "text-red-400", bg: "bg-red-400/10" },
  ending: { label: "ENDING SOON", color: "text-amber-400", bg: "bg-amber-400/10" },
  upcoming: { label: "UPCOMING", color: "text-accent-cyan", bg: "bg-accent-cyan/10" },
  ended: { label: "ENDED", color: "text-foreground/30", bg: "bg-foreground/5" },
};

export const FOOTER_LINKS = {
  product: ["Features", "Pricing", "API", "Changelog"],
  resources: ["Documentation", "Blog", "Community", "Support"],
  legal: ["Privacy", "Terms", "Cookies", "DMCA"],
} as const;

export const FOOTER_SOCIALS: NavLink[] = [
  { label: "Telegram", href: "https://t.me/shaihnurov" },
  { label: "GitHub", href: "https://github.com/shaihnurov/LubHub" },
];
