import Navbar from "@/components/layout/Navbar";
import Footer from "@/components/layout/Footer";
import Hero from "@/components/sections/Hero";
import Features from "@/components/sections/Features";
import Giveaways from "@/components/sections/Giveaways";
import CTA from "@/components/sections/CTA";

export default function Home() {
  return (
    <main className="relative">
      <Navbar />
      <Hero />
      <Features />
      <Giveaways />
      <CTA />
      <Footer />
    </main>
  );
}
