# Windows Task Scheduler Sample

This project was developed to make it possible to run applications in Windows from the user currently logged into the system. It uses the `Microsoft.Win32.TaskScheduler` library to schedule tasks in the Windows TaskScheduler, with support for both local environments and virtual machines (VMs).

---

## 📋 Purpose of the Project

The main goal is to allow applications to run automatically in the context of the logged-in user, ensuring proper permissions and a configured environment for the program or script. 

Use cases include:
- Automation of repetitive tasks.
- Execution of RPA (Robotic Process Automation) scripts.
- Scheduling the execution of applications.

---

## 🚀 Features

### 1. **Create Scheduled Task**
The `CreateTaskSchedulerAsCurrentWindowsUser` method allows you to create a scheduled task that will be executed in the context of the currently logged in user.

#### **Parameters**:
- `exePath` (string): Full path of the executable or script to be run.
- `rpaWorkingDirectory` (string): Working directory of the program/script.
- `taskName` (string): Name of the task.
- `startBoundary` (DateTime): Start date and time of the task.
- `arguments` (string): Optional arguments for the program/script via cli.

#### **Exemplo de uso CreateTaskSchedulerAsCurrentWindowsUser**:
```csharp
usando windows_task_schedule_sample;

var taskService = new TaskSchedulerService();

taskService.CreateTaskSchedulerAsCurrentWindowsUser(
    exePath: @“C:\Program Files\MyApp\myapp.exe”,
    rpaWorkingDirectory: @“C:\Program Files\MyApp”,
    taskName: “MyAppTask”,
    startBoundary: DateTime.Now.AddMinutes(10),
    argumentos: “--run”
);

Console.WriteLine(“Tarefa agendada com sucesso!”);
```

### **Exemplo de Uso TaskRun**:
```usando windows_task_schedule_sample;

var taskService = new TaskSchedulerService();

taskService.TaskRun(taskToRun: “MyAppTask”);
```
