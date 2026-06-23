import type { Variants } from "framer-motion";

export const EASE_OUT: [number, number, number, number] = [0.25, 0.46, 0.45, 0.94];

export const fadeUpVariants: Variants = {
  hidden: { opacity: 0, y: 30 },
  visible: {
    opacity: 1,
    y: 0,
    transition: { duration: 0.6, ease: EASE_OUT },
  },
};

export const staggerContainerVariants = (stagger: number = 0.1): Variants => ({
  hidden: { opacity: 0 },
  visible: {
    opacity: 1,
    transition: { staggerChildren: stagger },
  },
});
