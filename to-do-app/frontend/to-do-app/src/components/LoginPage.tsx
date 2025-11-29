import React, { useState } from "react";
import {
  Avatar,
  Button,
  TextField,
  Box,
  Typography,
  Container,
  Paper,
  InputAdornment,
  IconButton,
} from "@mui/material";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import axios from "axios";
import { API_URL } from "../consts/local.consts";
import { Visibility, VisibilityOff } from "@mui/icons-material";

type Props = {
  onLogin: (userId: string, username: string, token: string) => void;
};

export default function LoginPage({ onLogin }: Props) {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [errors, setErrors] = useState({ username: false, password: false });
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  const validate = () => {
    const errs = {
      username: username.trim().length === 0,
      password: password.trim().length < 6,
    };
    setErrors(errs);
    return !(errs.username || errs.password);
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!validate()) {
      return;
    }

    setLoading(true);

    axios
      .post(`${API_URL}/api/Auth/login`, {
        username,
        password,
      })
      .then((response) => {
        const token = response.data.token;
        
        const userId = response.data.userId; 

        if (!userId) {
            console.error("Backend nie zwrócił userId!");
        }

        localStorage.setItem("token", token);
        
        onLogin(userId, username.trim(), token);

        console.log("Login successful. Token:", token, "UserID:", userId);
      })
      .catch((error) => {
        console.error("Login failed:", error);
        alert(
          "Logowanie nie powiodło się. Sprawdź swoje dane i spróbuj ponownie."
        );
      })
      .finally(() => {
        setLoading(false);
      });
  };

  return (
    <Container component="main" maxWidth="xs">
      <Paper
        elevation={3}
        sx={{
          marginTop: 8,
          padding: 4,
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <Typography component="h1" variant="h5">
          ToDoApp
        </Typography>
        <Avatar sx={{ m: 1, bgcolor: "primary.main" }}>
          <LockOutlinedIcon />
        </Avatar>
        <Typography component="h1" variant="h6">
          Zaloguj się
        </Typography>
        <Box
          component="form"
          onSubmit={handleSubmit}
          noValidate
          sx={{ mt: 1, width: "100%" }}
        >
          <TextField
            margin="normal"
            fullWidth
            id="username"
            label="Nazwa użytkownika"
            name="username"
            autoComplete="username"
            autoFocus
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            error={errors.username}
            helperText={errors.username ? "Wprowadź nazwę użytkownika" : " "}
          />
          <TextField
            margin="normal"
            fullWidth
            name="password"
            label="Hasło"
            type={showPassword ? "text" : "password"}
            id="password"
            autoComplete="current-password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            error={errors.password}
            helperText={
              errors.password ? "Hasło musi mieć przynajmniej 6 znaków" : " "
            }
            InputProps={{
              endAdornment: (
                <InputAdornment position="end">
                  <IconButton
                    aria-label="toggle password visibility"
                    onClick={() => setShowPassword((prev) => !prev)}
                    edge="end"
                  >
                    {showPassword ? <VisibilityOff /> : <Visibility />}
                  </IconButton>
                </InputAdornment>
              ),
            }}
          />
          <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
            <Button
              disabled={loading}
              type="submit"
              variant="contained"
              sx={{ mt: 2 }}
            >
              {loading ? "Logowanie..." : "Zaloguj"}
            </Button>
          </Box>
        </Box>
      </Paper>
    </Container>
  );
}
