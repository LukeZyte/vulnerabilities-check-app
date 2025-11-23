import { useEffect, useState } from "react";

export function useAuth() {
  const [userId, setUserId] = useState<string | null>(null);
  const [token, setToken] = useState<string | null>(null);

  useEffect(() => {
    setUserId(localStorage.getItem("userid"));
    setToken(localStorage.getItem("token"));
  }, []);

  const login = (userId: string, token: string) => {
    localStorage.setItem("userid", userId);
    localStorage.setItem("token", token);
    setUserId(userId);
    setToken(token);
  };

  const logout = () => {
    localStorage.removeItem("userid");
    localStorage.removeItem("token");
    setUserId(null);
    setToken(null);
  };

  return { userId, token, login, logout };
}
