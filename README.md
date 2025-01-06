
# Execute windows apps as logged session user

Uma breve descrição sobre o que esse projeto faz e para quem ele é

# Windows Task Scheduler Sample

Este projeto foi desenvolvido para possibilitar a execução de aplicações no Windows a partir do usuário atualmente logado no sistema. Ele utiliza a biblioteca `Microsoft.Win32.TaskScheduler` para agendar tarefas no TaskScheduler do Windows, com suporte tanto para ambientes locais quanto para máquinas virtuais (VMs).

---

## 📋 Propósito do Projeto

O objetivo principal é permitir que aplicações sejam executadas automaticamente no contexto do usuário logado, garantindo permissões adequadas e um ambiente configurado para o programa ou script. 

Casos de uso incluem:
- Automação de tarefas repetitivas.
- Execução de scripts de RPA (Robotic Process Automation).
- Agendamento programado de execução de aplicações.

---

## 🚀 Recursos

### 1. **Criar Tarefa Agendada**
O método `CreateTaskSchedulerAsCurrentWindowsUser` permite criar uma tarefa agendada que será executada no contexto do usuário atualmente logado.

#### **Parâmetros**:
- `exePath` (string): Caminho completo do executável ou script a ser executado.
- `rpaWorkingDirectory` (string): Diretório de trabalho do programa/script.
- `taskName` (string): Nome da tarefa.
- `startBoundary` (DateTime): Data e hora de início da tarefa.
- `arguments` (string): Argumentos opcionais para o programa/script via cli.

#### **Exemplo de Uso CreateTaskSchedulerAsCurrentWindowsUser**:
```csharp
using windows_task_schedule_sample;

var taskService = new TaskSchedulerService();

taskService.CreateTaskSchedulerAsCurrentWindowsUser(
    exePath: @"C:\Program Files\MyApp\myapp.exe",
    rpaWorkingDirectory: @"C:\Program Files\MyApp",
    taskName: "MyAppTask",
    startBoundary: DateTime.Now.AddMinutes(10),
    arguments: "--run"
);

Console.WriteLine("Tarefa agendada com sucesso!");
```

### **Exemplo de Uso TaskRun**:
```using windows_task_schedule_sample;

var taskService = new TaskSchedulerService();

taskService.TaskRun(taskToRun: "MyAppTask");
```
