import { CircularProgress, IconButton, ListItem } from "@mui/material";
import type { Task } from "./TasksPage";
import DeleteIcon from "@mui/icons-material/Delete";

type Props = {
  task: Task;
  deleteLoading: boolean;
  deleteTask: (id: string) => void;
};

const TaskItem = ({ task, deleteLoading, deleteTask }: Props) => {
  return (
    <ListItem
      key={task.id}
      secondaryAction={
        <IconButton edge="end" onClick={() => deleteTask(task.id)}>
          {deleteLoading ? <CircularProgress /> : <DeleteIcon />}
        </IconButton>
      }
    >
      <div
        dangerouslySetInnerHTML={{
          __html: task.text,
        }}
      />
    </ListItem>
  );
};

export default TaskItem;
