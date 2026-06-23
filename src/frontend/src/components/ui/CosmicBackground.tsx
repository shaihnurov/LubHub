interface CosmicBackgroundProps {
  showGrid?: boolean;
}

export default function CosmicBackground({ showGrid = true }: CosmicBackgroundProps) {
  return (
    <>
      {showGrid && <div className="absolute inset-0 bg-grid opacity-30" />}
      <div className="absolute inset-0">
        <div className="absolute top-0 left-1/4 w-96 h-96 bg-accent/5 rounded-full blur-3xl" />
        <div className="absolute bottom-0 right-1/4 w-96 h-96 bg-accent-cyan/5 rounded-full blur-3xl" />
      </div>
    </>
  );
}
