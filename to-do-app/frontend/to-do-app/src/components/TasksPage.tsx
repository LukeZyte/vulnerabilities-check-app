import React, { useEffect, useState } from "react";
import {
  Box,
  Button,
  Container,
  List,
  TextField,
  Typography,
  Paper,
  Divider,
} from "@mui/material";
import DOMPurify from "dompurify";
import axios from "axios";
import { API_URL } from "../consts/local.consts";
import { useAuth } from "./hooks/useAuth";
import TaskItem from "./TaskItem";

export interface Task {
  id: string;
  text: string;
  isCompleted: boolean;
}

type Props = {
  onLogout: () => void;
};

export default function TasksPage({ onLogout }: Props) {
  const [username, setUsername] = useState(
    localStorage.getItem("username") || ""
  );
  const [task, setTask] = useState("");
  const [tasks, setTasks] = useState<Task[]>([]);
  const [pageLoading, setPageLoading] = useState(true);
  const [addLoading, setAddLoading] = useState(false);
  const [deleteLoading, setDeleteLoading] = useState(false);
  const { userId } = useAuth();

  useEffect(() => {
    if (!userId) return;

    axios
      .get(`${API_URL}/api/Tasks/owner/${userId}`)
      .then((response) => {
        setTasks(response.data);
      })
      .catch((error) => {
        console.error("Failed to fetch tasks:", error);
        alert("Nie udało się pobrać zadań. Spróbuj ponownie.");
      })
      .finally(() => {
        setPageLoading(false);
      });
  }, [userId]);

  const addTask = () => {
    if (!task.trim()) return;

    setAddLoading(true);

    console.log({
      text: task.trim(),
      ownerId: userId,
    });

    axios
      .post(`${API_URL}/api/Tasks`, {
        text: task.trim(),
        ownerId: userId,
      })
      .then((response) => {
        const createdTaskId: string = response.data.taskId;

        const newTask: Task = {
          id: createdTaskId,
          text: task.trim(),
          isCompleted: false,
        };

        setTasks((prev) => [...prev, newTask]);
      })
      .catch((error) => {
        console.error("Failed to add task:", error);
        alert("Nie udało się dodać zadania. Spróbuj ponownie.");
      })
      .finally(() => {
        setTask("");
        setAddLoading(false);
      });
  };

  const deleteTask = (id: string) => {
    setDeleteLoading(true);
    axios
      .delete(`${API_URL}/api/Tasks/${id}`)
      .then((response) => {
        console.log(response.data);
        setTasks((prev) => prev.filter((t) => t.id !== id));
      })
      .catch((error) => {
        console.error("Failed to delete task:", error);
        alert("Nie udało się usunąć zadania. Spróbuj ponownie.");
      })
      .finally(() => {
        setDeleteLoading(false);
      });
  };

  if (pageLoading) {
    return (
      <Container maxWidth="xl">
        <Typography variant="h6" color="black" mt={4}>
          Ładowanie zadań...
        </Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth="xl">
      {/* GÓRNY PANEL */}
      <Box
        display="flex"
        justifyContent="space-between"
        alignItems="center"
        mt={4}
        gap={12}
      >
        <Typography variant="h5" color="black">
          Zadania użytkownika: {username}
        </Typography>
        <Button variant="outlined" color="error" onClick={onLogout}>
          Wyloguj
        </Button>
      </Box>

      {/* POLE DODAWANIA */}
      <Paper elevation={3} sx={{ p: 2, mt: 4 }}>
        <Typography variant="h6" gutterBottom>
          Dodaj nowe zadanie
        </Typography>

        <Box display="flex" gap={2}>
          <TextField
            label="Treść HTML zadania"
            fullWidth
            value={task}
            onChange={(e) => setTask(e.target.value)}
          />
          <Button variant="contained" onClick={addTask}>
            {addLoading ? "Dodawanie..." : "Dodaj"}
          </Button>
        </Box>

        {/* PODGLĄD */}
        <Box mt={2}>
          <Typography variant="subtitle2">Podgląd (sanitizowany):</Typography>
          <Paper
            elevation={0}
            sx={{
              p: 2,
              bgcolor: "background.paper",
              border: "1px solid #eee",
              mt: 1,
            }}
          >
            <div
              dangerouslySetInnerHTML={{
                __html: DOMPurify.sanitize(task || "<i>brak treści</i>"),
              }}
            />
          </Paper>
        </Box>
      </Paper>

      <Divider sx={{ mt: 3 }} />

      {/* LISTA ZADAŃ */}
      <Paper elevation={1} sx={{ mt: 3 }}>
        <List>
          {tasks.length === 0 && (
            <Typography sx={{ p: 2, color: "gray" }}>
              Brak zadań — dodaj pierwsze!
            </Typography>
          )}

          {tasks.map((t) => (
            <TaskItem
              key={t.id}
              task={t}
              deleteLoading={deleteLoading}
              deleteTask={deleteTask}
            />
          ))}
        </List>
      </Paper>
    </Container>
  );
}
