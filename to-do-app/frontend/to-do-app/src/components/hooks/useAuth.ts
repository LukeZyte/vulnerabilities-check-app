import { useEffect, useState } from "react";

export function useAuth() {
  const [userId, setUserId] = useState<string | null>(null);

  useEffect(() => {
    const stored = localStorage.getItem("userid");
    if (stored) setUserId(stored);
  }, []);

  const login = (userId: string, username: string) => {
    localStorage.setItem("username", username.trim());
    localStorage.setItem("userid", userId);
    console.log("setUserId", userId);
    setUserId(userId);
  };

  const logout = () => {
    localStorage.removeItem("username");
    localStorage.removeItem("userid");
    setUserId(null);
  };

  return { userId, login, logout };
}
