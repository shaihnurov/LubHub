import Link from "next/link";

export default function NotFound() {
  return (
    <div className="min-h-screen flex items-center justify-center">
      <div className="text-center">
        <h1 className="text-6xl font-bold mb-4 text-gradient">404</h1>
        <p className="text-2xl mb-8">Page not found</p>
        <Link
          href="/"
          className="px-6 py-3 rounded-xl bg-accent text-white font-medium hover:bg-accent-light transition-colors inline-block"
        >
          Go home
        </Link>
      </div>
    </div>
  );
}
