import { FOOTER_LINKS, FOOTER_SOCIALS } from "@/constants";
import { Logo } from "@/components/ui/Logo";

export default function Footer() {
  return (
    <footer id="about" className="relative border-t border-foreground/5">
      <div className="max-w-7xl mx-auto px-8 py-20">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-12 mb-16">
          <div className="md:col-span-1">
            <div className="flex items-center gap-2.5 mb-5">
              <Logo size={32} />
              <span className="text-lg font-semibold tracking-tight">
                Lub<span className="text-gradient">Hub</span>
              </span>
            </div>
            <p className="text-sm text-foreground/30 leading-relaxed">
              The ultimate giveaway platform for Twitch streamers.
            </p>
          </div>

          <div>
            <h4 className="text-sm font-semibold mb-5 text-foreground/60">Product</h4>
            <ul className="space-y-4">
              {FOOTER_LINKS.product.map((item) => (
                <li key={item}>
                  <a href="#" className="text-sm text-foreground/30 hover:text-foreground/60 transition-colors duration-300">
                    {item}
                  </a>
                </li>
              ))}
            </ul>
          </div>

          <div>
            <h4 className="text-sm font-semibold mb-5 text-foreground/60">Resources</h4>
            <ul className="space-y-4">
              {FOOTER_LINKS.resources.map((item) => (
                <li key={item}>
                  <a href="#" className="text-sm text-foreground/30 hover:text-foreground/60 transition-colors duration-300">
                    {item}
                  </a>
                </li>
              ))}
            </ul>
          </div>

          <div>
            <h4 className="text-sm font-semibold mb-5 text-foreground/60">Legal</h4>
            <ul className="space-y-4">
              {FOOTER_LINKS.legal.map((item) => (
                <li key={item}>
                  <a href="#" className="text-sm text-foreground/30 hover:text-foreground/60 transition-colors duration-300">
                    {item}
                  </a>
                </li>
              ))}
            </ul>
          </div>
        </div>

        <div className="pt-10 border-t border-foreground/5 flex flex-col sm:flex-row items-center justify-between gap-4">
          <p className="text-xs text-foreground/20">
            &copy; 2026 LubHub. All rights reserved.
          </p>
          <div className="flex items-center gap-8">
            {FOOTER_SOCIALS.map((link) => (
              <a
                key={link.label}
                href={link.href}
                className="text-xs text-foreground/20 hover:text-foreground/50 transition-colors duration-300"
              >
                {link.label}
              </a>
            ))}
          </div>
        </div>
      </div>
    </footer>
  );
}
