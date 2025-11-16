import { useEffect, useState } from "react";
import "./App.css";
import LoginPage from "./components/LoginPage";
import { useAuth } from "./components/hooks/useAuth";
import TasksPage from "./components/TasksPage";

function App() {
  const { userId, login, logout } = useAuth();
  const [loggedIn, setLoggedIn] = useState(false);

  useEffect(() => {
    if (userId) setLoggedIn(true);
  }, [userId]);

  const handleLogin = (userId: string, username: string) => {
    login(userId, username);
    setLoggedIn(true);
  };

  const handleLogout = () => {
    logout();
    setLoggedIn(false);
  };

  return (
    <>
      {!loggedIn ? (
        <LoginPage onLogin={handleLogin} />
      ) : (
        <TasksPage onLogout={handleLogout} />
      )}
    </>
  );
}

export default App;
