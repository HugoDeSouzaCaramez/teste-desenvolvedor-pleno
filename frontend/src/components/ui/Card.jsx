export function Card({ children, className }) {
    return (
      <div className={`rounded-2xl shadow-md p-4 ${className}`}>
        {children}
      </div>
    );
  }
  