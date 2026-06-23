"use client";

import { useState, useEffect } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { NAV_LINKS } from "@/constants";
import { EASE_OUT } from "@/lib/animations";
import { useAuth } from "@/lib/auth";
import { Logo } from "@/components/ui/Logo";
import { TwitchIcon } from "@/components/ui/TwitchIcon";

export default function Navbar() {
  const [scrolled, setScrolled] = useState(false);
  const [mobileOpen, setMobileOpen] = useState(false);
  const { isAuthenticated, displayName, loginWithTwitch, logout } = useAuth();

  useEffect(() => {
    const onScroll = () => setScrolled(window.scrollY > 20);
    window.addEventListener("scroll", onScroll, { passive: true });
    return () => window.removeEventListener("scroll", onScroll);
  }, []);

  return (
    <motion.nav
      initial={{ y: -100, opacity: 0 }}
      animate={{ y: 0, opacity: 1 }}
      transition={{ duration: 0.8, ease: EASE_OUT }}
      className={`fixed top-0 left-0 right-0 z-50 border-b border-transparent transition-[background-color,box-shadow,backdrop-filter] duration-500 ${
        scrolled ? "nav-scrolled border-foreground/5" : "bg-transparent"
      }`}
    >
      <div className="max-w-7xl mx-auto px-8 lg:px-12">
        <div className="flex items-center justify-between h-18 lg:h-22">
          <div className="flex items-center gap-3">
            <Logo size={36} />
            <span className="text-xl font-semibold tracking-tight">
              Lub<span className="text-gradient">Hub</span>
            </span>
          </div>

          <div className="hidden md:flex items-center gap-10">
            {NAV_LINKS.map((item) => (
              <a
                key={item.label}
                href={item.href}
                className="text-sm text-foreground/60 hover:text-foreground transition-colors duration-300 relative group py-2"
              >
                {item.label}
                <span className="absolute -bottom-1 left-0 w-0 h-px bg-accent group-hover:w-full transition-all duration-300" />
              </a>
            ))}
          </div>

          <div className="flex items-center gap-4">
            {isAuthenticated ? (
              <div className="hidden sm:flex items-center gap-3">
                <span className="text-sm text-foreground/60">
                  {displayName}
                </span>
                <button
                  onClick={logout}
                  className="px-4 py-2.5 rounded-xl glass text-foreground/70 hover:text-foreground text-sm font-medium transition-all duration-300 active:scale-95"
                >
                  Logout
                </button>
              </div>
            ) : (
              <button
                onClick={loginWithTwitch}
                className="hidden sm:flex items-center gap-2.5 px-6 py-3 rounded-xl bg-[#9146ff] hover:bg-[#7c3aed] text-white text-sm font-medium transition-all duration-300 hover:shadow-lg hover:shadow-[#9146ff]/25 active:scale-95"
              >
                <TwitchIcon size={18} />
                Login with Twitch
              </button>
            )}

            <button
              onClick={() => setMobileOpen(!mobileOpen)}
              className="md:hidden flex flex-col gap-1.5 p-3"
              aria-label="Menu"
            >
              <motion.span
                animate={mobileOpen ? { rotate: 45, y: 6 } : { rotate: 0, y: 0 }}
                className="block w-6 h-px bg-foreground"
              />
              <motion.span
                animate={mobileOpen ? { opacity: 0 } : { opacity: 1 }}
                className="block w-6 h-px bg-foreground"
              />
              <motion.span
                animate={mobileOpen ? { rotate: -45, y: -6 } : { rotate: 0, y: 0 }}
                className="block w-6 h-px bg-foreground"
              />
            </button>
          </div>
        </div>
      </div>

      <AnimatePresence>
        {mobileOpen && (
          <motion.div
            initial={{ height: 0, opacity: 0 }}
            animate={{ height: "auto", opacity: 1 }}
            exit={{ height: 0, opacity: 0 }}
            transition={{ duration: 0.3 }}
            className="md:hidden glass-strong overflow-hidden"
          >
            <div className="px-8 py-6 flex flex-col gap-5">
              {NAV_LINKS.map((item) => (
                <a
                  key={item.label}
                  href={item.href}
                  onClick={() => setMobileOpen(false)}
                  className="text-foreground/60 hover:text-foreground transition-colors py-1"
                >
                  {item.label}
                </a>
              ))}
              {isAuthenticated ? (
                <div className="flex items-center justify-between">
                  <span className="text-sm text-foreground/60">{displayName}</span>
                  <button
                    onClick={() => {
                      logout();
                      setMobileOpen(false);
                    }}
                    className="px-4 py-2.5 rounded-xl glass text-foreground/70 text-sm font-medium transition-all"
                  >
                    Logout
                  </button>
                </div>
              ) : (
                <button
                  onClick={() => {
                    loginWithTwitch();
                    setMobileOpen(false);
                  }}
                  className="flex items-center justify-center gap-2.5 px-6 py-3 rounded-xl bg-[#9146ff] hover:bg-[#7c3aed] text-white text-sm font-medium transition-all"
                >
                  <TwitchIcon size={18} />
                  Login with Twitch
                </button>
              )}
            </div>
          </motion.div>
        )}
      </AnimatePresence>
    </motion.nav>
  );
}
