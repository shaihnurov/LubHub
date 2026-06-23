interface BadgeProps {
  status: "Pending" | "Active" | "Finished" | "Drawn";
  className?: string;
}

const statusConfig = {
  Pending: {
    label: "Pending",
    className: "bg-foreground/5 text-foreground/60 border-foreground/10",
  },
  Active: {
    label: "Live",
    className: "bg-green-500/10 text-green-400 border-green-500/20",
  },
  Finished: {
    label: "Finished",
    className: "bg-amber-500/10 text-amber-400 border-amber-500/20",
  },
  Drawn: {
    label: "Drawn",
    className: "bg-accent/10 text-accent border-accent/20",
  },
};

export default function Badge({ status, className = "" }: BadgeProps) {
  const config = statusConfig[status];

  return (
    <span
      className={`inline-flex items-center px-3 py-1 rounded-full text-xs font-medium border ${config.className} ${className}`}
    >
      <span className="w-1.5 h-1.5 rounded-full bg-current mr-2 animate-pulse" />
      {config.label}
    </span>
  );
}
