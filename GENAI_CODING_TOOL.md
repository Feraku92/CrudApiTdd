# Task Management API Example

## 📚 What is This Document?

This document shows a real example of how I worked with an AI assistant to plan a REST API project. I'll explain:
- What I asked (the prompt)
- What the AI answered
- How the AI validated its own suggestions
- What worked well and what needed clarification

This is based on **Chain of Thought Prompting** - a technique where you ask the AI to think step-by-step instead of just giving a direct answer.

---

## 🎯 My Original Prompt

```
You are a senior software engineer, and you have the task of creating a REST API 
for task management with the following rules:

- Use .NET Core 8
- Use the MVC pattern and DTOs to communicate between layers
- Use Swagger to visualize and test the services
- Create only the models Task and User
- The API must allow creating, reading, updating, and deleting tasks
- The Task fields are: title, description, status, and due_date
- User has a one-to-many relationship with Task
- Use SQLite for persistence

Analyze everything step by step. Do not implement anything yet; instead, show me 
the folder structure and objects you would create.
If there is anything you are not sure about or do not know, tell me.
```

### 💡 Why This Prompt Works Well

**Good things I did:**
1. ✅ **Clear role**: "You are a senior software engineer" - sets context
2. ✅ **Specific requirements**: Listed exactly what I need (.NET 8, SQLite, etc.)
3. ✅ **Step-by-step request**: "Analyze everything step by step" - triggers deeper thinking
4. ✅ **Don't implement yet**: Prevents the AI from rushing into code
5. ✅ **Ask for uncertainty**: "If there is anything you are not sure about or do not know, tell me"

**What makes this "Chain of Thought" prompting:**
- I didn't just say "create a task API"
- I asked to **analyze step by step**
- I asked to **show the structure first** before coding
- I invited the AI to **ask questions** about unclear parts

---

## 🤖 AI's Response Summary

The AI gave me:
1. **Complete folder structure** - 4 projects following Clean Architecture
2. **List of all objects** - Entities, DTOs, Services, Controllers
3. **Breakdown of each class** - What properties and methods they'd have
4. **Questions section** - 7 things the AI wasn't sure about!

---

## ✅ How the AI Validated Its Own Suggestions

### 1. **Checked Against My Requirements**
The AI made sure to include:
- ✅ .NET Core 8
- ✅ MVC pattern (Controllers)
- ✅ DTOs (separate from entities)
- ✅ Swagger
- ✅ Only Task and User models
- ✅ CRUD operations
- ✅ SQLite

### 2. **Proposed Best Practices**
The AI suggested things I didn't explicitly ask for but are good practice:
- **Repository Pattern** - To separate data access from business logic
- **Service Layer** - To keep controllers thin
- **Custom Exceptions** - For better error handling
- **Enum for TaskStatus** - Instead of just string (more type-safe)
- **Timestamps** - CreatedAt, UpdatedAt (common in real apps)

### 3. **Asked Clarifying Questions**
Instead of making assumptions, the AI asked:
- Should User have full CRUD or just read operations?
- Should TaskStatus be an enum or string?
- Do we need pagination/filtering?
- Do we need authentication?
- Should we seed sample data?

**This is important!** The AI admitted what it didn't know instead of guessing.

---

## 🔧 What I Would Validate/Improve

As a developer reviewing this response, here's what I'd check:

### ✅ Things That Look Good

1. **Clean Architecture structure** - Separates concerns properly
2. **DTOs separate from entities** - Follows the requirement
3. **Repository pattern** - Good for testing and maintainability
4. **Swagger integration** - As required

### 🤔 Things I'd Question or Improve

1. **Is this too complex for a simple task API?**
   - 4 projects might be overkill if this is a small app
   - Could simplify to 2 projects (API + Data) for a starter project
   - **Decision**: Keep it if learning Clean Architecture, simplify if you need quick delivery

2. **What about validation?**
   - AI mentioned it but didn't detail it
   - **Add**: Data annotations on DTOs (`[Required]`, `[MaxLength]`, etc.)
   - **Add**: Business validation in services (e.g., due date can't be in the past)

3. **Error handling**
   - AI suggested custom exceptions but didn't detail the approach
   - **Add**: Global exception middleware for consistent error responses

---

## 🛡️ Handling Edge Cases & Validations

The AI's response made me think about these scenarios:

### Edge Cases to Handle:

**For Tasks:**
- ❓ What if dueDate is in the past when creating?
- ❓ Can description be empty/null?
- ❓ Can you delete a task that belongs to a user?
- ❓ What happens if you try to update a task that doesn't exist?
- ❓ Can status only transition in certain ways? (e.g., can't go from Completed back to Pending?)

**For Users:**
- ❓ Can a user be deleted if they have tasks?
- ❓ Should we cascade delete tasks when user is deleted?
- ❓ Or prevent deletion if user has tasks?

**For the Relationship:**
- ❓ Can a task exist without a user (orphaned task)?
- ❓ When creating a task, does the userId need to exist?

### Validations I Would Add:

```csharp
CreateTaskDto:
- [Required] Title - Can't be empty
- [MaxLength(200)] Title - Not too long
- [Required] Status - Must have a status
- [Required] DueDate - Must have a due date
- Custom validation: DueDate > DateTime.Now (can't be in past)
- [Required] UserId - Must belong to a user

User:
- [Required] Name
- [EmailAddress] Email - Valid email format
- Unique email constraint in database
```

---

## 🔐 Authentication Considerations

The AI asked: "Do you want authentication/authorization?"

**Why this matters:**
- Without auth: Anyone can see/modify any tasks
- With auth: Users can only see their own tasks

**My thought process:**
1. The prompt didn't mention authentication
2. But it has a User entity - suggests multi-user system
3. In real life, tasks should be private

**Decision points:**
- **Option 1**: No auth (simple, good for learning/demo)
  - GET /api/tasks returns ALL tasks from ALL users
  - Need to pass userId when creating tasks
  
- **Option 2**: JWT auth (like the Pokemon API)
  - Users log in and get a token
  - GET /api/tasks returns only logged-in user's tasks
  - UserId comes from the token automatically

**For learning**: Start with Option 1 (simpler), add Option 2 later if needed.

---

## 📚 Resources

- [Chain of Thought Prompting Guide](https://www.prompt.org.es/docs/prompt-basics/chain-of-thought-prompting-guia-completa)