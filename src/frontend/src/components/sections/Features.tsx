"use client";

import { motion } from "framer-motion";
import { FEATURES } from "@/constants";
import { fadeUpVariants, staggerContainerVariants } from "@/lib/animations";

export default function Features() {
  return (
    <section id="features" className="relative py-32 px-8">
      <div className="max-w-7xl mx-auto">
        <motion.div
          initial={{ opacity: 0, y: 30 }}
          whileInView={{ opacity: 1, y: 0 }}
          viewport={{ once: true, margin: "-100px" }}
          transition={{ duration: 0.8 }}
          className="text-center mb-24"
        >
          <span className="text-xs uppercase tracking-[0.2em] text-accent/80 font-medium mb-5 block">
            Features
          </span>
          <h2 className="text-4xl md:text-5xl font-bold tracking-tight mb-7">
            Everything you need to
            <br />
            <span className="text-gradient">run legendary giveaways</span>
          </h2>
          <p className="text-foreground/40 max-w-xl mx-auto text-lg leading-relaxed">
            Every feature designed to maximize hype and minimize hassle.
          </p>
        </motion.div>

        <motion.div
          variants={staggerContainerVariants(0.1)}
          initial="hidden"
          whileInView="visible"
          viewport={{ once: true, margin: "-50px" }}
          className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8"
        >
          {FEATURES.map((feature, i) => (
            <motion.div
              key={i}
              variants={fadeUpVariants}
              className="group relative p-10 rounded-2xl glass transition-all duration-500 hover:glow-border hover:bg-surface-elevated/50"
            >
              <div className="w-14 h-14 rounded-xl bg-accent/10 flex items-center justify-center text-accent mb-6 group-hover:bg-accent/20 transition-colors duration-500">
                {feature.icon}
              </div>
              <h3 className="text-lg font-semibold mb-4 tracking-tight">
                {feature.title}
              </h3>
              <p className="text-sm text-foreground/40 leading-relaxed">
                {feature.description}
              </p>
            </motion.div>
          ))}
        </motion.div>
      </div>
    </section>
  );
}
