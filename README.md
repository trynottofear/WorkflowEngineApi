# 🛠️ Workflow Engine API

A simple, extensible **state machine API** built using **.NET 8 Minimal APIs** that supports the definition of workflows with states and actions, and allows creating and progressing workflow instances.

---

## ⚙️ Features

- Define workflows with multiple **states** (initial, final) and **actions**.
- Create **workflow instances** from a workflow definition.
- Execute **enabled actions** to transition between states.
- Fully in-memory; no external DB or storage.
- Minimal API & Swagger UI support for easy testing.
- Built with extensibility in mind for future additions like persistence or validation rules.

---

## 🚀 Getting Started

### 📦 Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### ▶️ Run Locally

```bash
git clone https://github.com/your-username/WorkflowEngineApi.git
cd WorkflowEngineApi
dotnet run
```

Visit Swagger UI:
```
http://localhost:5205/swagger
```

---

## 🔌 API Endpoints

---

### 1. 📄 Create Workflow Definition

**POST** `/api/workflows`

Creates a new workflow definition.

#### Request

```json
{
  "states": [
    { "id": "Draft", "isInitial": true, "isFinal": false },
    { "id": "UnderReview", "isInitial": false, "isFinal": false },
    { "id": "Approved", "isInitial": false, "isFinal": true },
    { "id": "Rejected", "isInitial": false, "isFinal": true }
  ],
  "actions": [
    { "id": "submit", "enabled": true, "fromStates": ["Draft"], "toState": "UnderReview" },
    { "id": "approve", "enabled": true, "fromStates": ["UnderReview"], "toState": "Approved" },
    { "id": "reject", "enabled": true, "fromStates": ["UnderReview"], "toState": "Rejected" }
  ]
}
```

#### Response

```json
{
  "id": "c388510d-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "states": [...],
  "actions": [...]
}
```

---

### 2. 📥 Get Workflow Definition by ID

**GET** `/api/workflows/{definitionId}`

Returns the full workflow definition.

#### Response

```json
{
  "id": "c388510d-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "states": [...],
  "actions": [...]
}
```

---

### 3. 📄 List All Workflow Definitions

**GET** `/api/workflows`

Returns a list of all workflow definitions.

#### Response

```json
[
  {
    "id": "c388510d-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
    "states": [...],
    "actions": [...]
  }
]
```

---

### 4. 🧬 Create Workflow Instance

**POST** `/api/workflowInstances`

Creates a new workflow instance from an existing workflow definition.

#### Request

```json
{
  "definitionId": "c388510d-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
}
```

#### Response

```json
{
  "id": "96319cc0-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "definitionId": "c388510d-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "currentStateId": "Draft",
  "history": []
}
```

---

### 5. 🧾 Get Workflow Instance by ID

**GET** `/api/workflowInstances/{instanceId}`

Returns a specific workflow instance.

#### Response

```json
{
  "id": "96319cc0-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "definitionId": "c388510d-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "currentStateId": "UnderReview",
  "history": [...]
}
```

---

### 6. 📋 List All Workflow Instances

**GET** `/api/workflowInstances`

Returns a list of all workflow instances.

#### Response

```json
[
  {
    "id": "96319cc0-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
    "definitionId": "c388510d-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
    "currentStateId": "Draft",
    "history": []
  }
]
```

---

### 7. 🔁 Execute Action on Workflow Instance

**POST** `/api/workflowInstances/{instanceId}/actions`

Executes a state transition by sending an action string.

#### Request (raw body text)

```text
"submit"
```

#### Response

```json
{
  "id": "96319cc0-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "definitionId": "c388510d-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "currentStateId": "UnderReview",
  "history": [
    {
      "action": "submit",
      "fromState": "Draft",
      "toState": "UnderReview",
      "timestamp": "2025-07-17T12:00:00Z"
    }
  ]
}
```

#### Errors

- `400 Bad Request` – Invalid or disabled action
- `404 Not Found` – Instance or definition not found

---

## 📮 Testing via Postman

### Required Headers

```
Content-Type: application/json
Accept: application/json
```

### Sample Postman Flow

1. **POST** `/api/workflows` → define a workflow
2. **GET** `/api/workflows` → view all definitions
3. **POST** `/api/workflowInstances` → create instance
4. **GET** `/api/workflowInstances` → list instances
5. **POST** `/api/workflowInstances/{id}/actions` with raw string body like `"submit"` → trigger transition

---

## 🧪 Project Structure

```
WorkflowEngineApi/
├── State-Machine API.csproj
├── Program.cs
├── Properties/
│   └── launchSettings.json
├── Models/
│   ├── WorkflowState.cs
│   ├── WorkflowAction.cs
│   ├── WorkflowDefinition.cs
│   ├── WorkflowInstance.cs
│   ├── CreateWorkflowDefinitionRequest.cs
│   ├── WorkflowDefinitionResponse.cs
├── Services/
│   ├── IWorkflowService.cs
│   └── WorkflowService.cs
├── Controllers/
│   ├── WorkflowDefinitionsController.cs
│   └── WorkflowInstancesController.cs
└── README.md

```

## 🧠 Assumptions & Notes

- Workflow state is stored **in memory**; restarting the app clears all data.
- No database or persistence used.
- Each action must be marked as `enabled` to be executed.
- Only one initial state allowed per workflow.
- No authentication/authorization implemented.

---

## 🧩 Extension Ideas

- Add persistence (EF Core / SQLite).
- Support action-level conditions.
- Add audit logs for state transitions.
- Implement role-based action permissions.

---

## 📜 License

This project is for interview evaluation purposes only.

