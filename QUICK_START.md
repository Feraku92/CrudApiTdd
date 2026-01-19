# Quick Start Guide

## ⚡ Fast Setup (5 Minutes)

### Prerequisites Check
```powershell
# Check .NET installation
dotnet --version
# Expected: 8.0.x or higher

# Check Node.js installation
node --version
# Expected: 18.x or higher

# Check MongoDB
# MongoDB should be running on localhost:27017
```

### Backend (2 minutes)
```powershell
# Navigate to API project
cd Backend\CrudApi\CrudApi.Api

# Restore and run
dotnet restore
dotnet run

# API will be available at:
# https://localhost:5001
# Swagger: https://localhost:5001/swagger
```

### Frontend (2 minutes)
```powershell
# Navigate to frontend (in a new terminal)
cd Frontend\crudapi-frontend

# Install and run
npm install
npm start

# App will open at: http://localhost:4200
```

### Test It! (1 minute)
1. Open `http://localhost:4200`
2. Click "Register"
3. Create account or use:
   - Username: `trainer_ash`
   - Password: `Pikachu123!`
4. Login and see the demo Pokémon! 🎉

---

## 🧪 Run Tests

```powershell
# Backend tests
cd Backend\CrudApi
dotnet test

# Frontend tests
cd Frontend\crudapi-frontend
npm test
```

---

## 🔑 Demo Credentials

| Username | Email | Password |
|----------|-------|----------|
| trainer_ash | ash@pokemon.com | Pikachu123! |
| trainer_misty | misty@pokemon.com | Starmie456! |
| trainer_brock | brock@pokemon.com | Onix789! |

---

## 📦 Pre-seeded Pokémon

The application includes 10 demo Pokémon:
- Bulbasaur (#1)
- Charmander (#4)
- Squirtle (#7)
- Charizard (#6)
- Blastoise (#9)
- Pikachu (#25)
- Gengar (#94)
- Snorlax (#143)
- Mewtwo (#150)
- Mew (#151)

---

## ❓ Common Issues

**MongoDB not running?**
```powershell
# Start MongoDB service (Windows)
net start MongoDB
```

**Port already in use?**
- Backend: Edit `launchSettings.json` to change ports
- Frontend: Run `ng serve --port 4300` for different port

**Database not seeding?**
- Data seeds automatically on first run
- Check terminal for seeding logs
- If needed, delete database and restart API

---

## 📚 More Information

- Full documentation: See [README.md](README.md)
- Demo guide: See [DEMO_GUIDE.md](DEMO_GUIDE.md)
- API docs: `https://localhost:5001/swagger` (when running)

---

**Need help? Check the main README.md for detailed instructions!**
