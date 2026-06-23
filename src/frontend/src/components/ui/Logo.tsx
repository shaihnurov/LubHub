interface LogoProps {
  size?: number;
}

export function Logo({ size = 36 }: LogoProps) {
  return (
    <div className="relative" style={{ width: size, height: size }}>
      <div className="absolute inset-0 bg-gradient-to-br from-accent to-accent-cyan rounded-lg rotate-12 opacity-80 blur-sm" />
      <div className="absolute inset-0 bg-gradient-to-br from-accent to-accent-cyan rounded-lg -rotate-12 opacity-60" />
      <div className="relative flex items-center justify-center w-full h-full">
        <span className="text-white font-bold" style={{ fontSize: size * 0.4 }}>L</span>
      </div>
    </div>
  );
}
