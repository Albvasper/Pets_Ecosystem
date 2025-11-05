![Pet Ecosystem Banner](.github/banner.png)

# 🐾 Pet Ecosystem - Multiplayer Pet Simulation

[![Unity](https://img.shields.io/badge/Unity-6000.0.59f2%20LTS-black.svg?style=for-the-badge&logo=unity)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Firebase](https://img.shields.io/badge/Firebase-Realtime%20DB-FFCA28?style=for-the-badge&logo=firebase&logoColor=black)](https://firebase.google.com/)
[![WebGL](https://img.shields.io/badge/WebGL-Ready-blue?style=for-the-badge)](https://docs.unity3d.com/Manual/webgl.html)
[![Status](https://img.shields.io/badge/Status-Production%20Ready-brightgreen?style=for-the-badge)](https://github.com/yourusername/pet-ecosystem)

> **A multiplayer pet ecosystem with Firebase integration, AI behaviors, and real-time synchronization. Features autonomous pet AI with state machines, breeding systems, and viewer interaction through WebGL.**

## Project Overview

This project demonstrates advanced game programming and backend integration through a real-time pet simulation system. Built for WebGL deployment, it features intelligent pet AI, Firebase database integration for multi-user interaction, and complex state-driven behaviors. The system maintains stable performance with 20+ autonomous entities interacting simultaneously.

### Key Technical Achievements
- **Custom REST API Wrapper** for Firebase Realtime Database (WebGL-compatible)
- **Advanced State Machine Architecture** with inheritance-based pet behaviors
- **Real-Time Queue System** managing viewer connections and spawn requests
- **Autonomous AI Behaviors** including breeding, hunting, and pack dynamics
- **WebGL-Optimized Architecture** with efficient polling and data synchronization

## Technical Stack

| **Category** | **Technology** | **Purpose** |
|--------------|----------------|-------------|
| **Engine** | Unity 6000.0.59f2 LTS | Core game development platform |
| **Language** | C# | Primary programming language |
| **Backend** | Firebase Realtime Database | Multi-user data synchronization |
| **API** | Custom REST Wrapper | WebGL-compatible database operations |
| **AI System** | State Machines | Pet behavior and decision-making |

## Core Systems Architecture

### Firebase REST Integration
```
FirebaseREST
├── GET Operations → Read pet/queue data
├── SET Operations → Update pet states
├── PUSH Operations → Create spawn requests
└── DELETE Operations → Cleanup and removal
```

### Pet Behavior Hierarchy
```
BaseAnimal (Abstract)
├── Pet (Peaceful Animals)
│   ├── Idle State → Wandering/Following pack leader
│   ├── Breeding State → Mate finding and reproduction
│   └── Bumping State → Social interactions
└── HostilePet (Predators)
    ├── Hunting State → Hunting prey
    ├── Attack State → Combat mechanics
    └── Hostil Bumping State → Transitions to hunting state
```

### Spawner Management System
- **Firebase Polling**: Checks spawn requests every 2 seconds
- **Queue Processing**: Handles viewer spawn requests in order
- **Capacity Management**: Enforces 20 pet limit
- **Pet Instantiation**: Spawns prefabs with unique IDs and Firebase registration

## Game Features

### **Intelligent Pet AI**
- **State-Driven Behaviors**: Clean state transitions with virtual methods
- **Pack Dynamics**: Pets follow pack leaders and coordinate movement
- **Breeding System**: Autonomous mate finding and reproduction
- **Predator-Prey Mechanics**: Hostile pets hunt and consume passive animals
- **Adaptive Responses**: Dynamic behavior based on environment and interactions

### **Multi-User Viewer System**
- **Queue Management**: Automatic queue positioning when ecosystem is full
- **Real-Time Updates**: Live capacity monitoring and position tracking
- **Random Pet Assignment**: Fair distribution of pet types across viewers
- **Spawn Request System**: Asynchronous pet creation via Firebase
- **WebGL Compatible**: Runs in browser without plugins

### **Firebase Integration**
- **Real-Time Synchronization**: All pet data synced across clients
- **Persistent State**: Pet information survives across sessions
- **REST API Operations**: Full CRUD functionality for WebGL
- **Request Polling**: Efficient 2-second interval updates
- **Automatic Cleanup**: Dead pets and completed requests removed

## Project Structure
```
Scripts/
├── Animals/
│   ├── Hostile Pets/
│   │   ├── HostilePet.cs               # Predator implementation
│   │   ├── HostilePetAnimator.cs       # Hostile pet animation controller
│   │   ├── HostilePetBehavior.cs       # Behavior management
│   │   ├── HostilePetPhysics.cs        # Physics and collision handling
│   │   └── States/
│   │       ├── State_Attack.cs         # Attack state implementation
│   │       ├── State_Hunt.cs           # Hunting/chase behavior
│   │       └── State_HostileBumping.cs # Hostile collision interactions
│   └── Pets/
│       ├── Pet.cs                      # Passive animal implementation
│       ├── PetAnimator.cs              # Pet animation controller
│       ├── PetBehavior.cs              # Behavior management
│       ├── PetPhysics.cs               # Physics and collision handling
│       ├── AnimalData.cs               # Animal data structure
│       └── States/
│           ├── State_IDLE.cs           # Wandering and pack following
│           ├── State_Breeding.cs       # Mate finding and reproduction
│           └── State_Bumping.cs        # Social collision interactions
├── Base/
│   ├── Base Animal/
│   │   ├── BaseAnimal.cs               # Abstract base class for all animals
│   │   ├── BaseAnimator.cs             # Base animation controller
│   │   ├── BaseBehavior.cs             # Base behavior logic
│   │   └── BasePhysics.cs              # Base physics handling
│   └── State/
│       ├── State.cs                    # Base state class with enter/tick/exit
│       ├── StateTypeAnimal.cs          # Animal-specific state base
│       ├── StateTypePets.cs            # Pet-specific state base
│       └── StateTypeHostilePet.cs      # Hostile pet-specific state base
├── Firebase/
│   ├── FirebaseREST.cs                 # Custom REST API wrapper 
│   └── FirebaseInit.cs                 # Firebase initialization
├── Game Logic/
│   ├── Spawner_Manager.cs              # Firebase polling and pet instantiation
│   ├── Pet_Manager.cs                  # Pet collection and lifecycle management
│   ├── UI_Manager.cs                   # Main ecosystem UI controller
│   ├── Weather_Manager.cs              # Weather system and effects
│   └── Cave.cs                         # Cave shelter mechanics
└── Viewer Screen/
    └── ViewerScreen_UI_Manager.cs      # Viewer UI, queue, and spawn flow
```

## Key Scripts

| Script | Description |
|--------|-------------|
| [`FirebaseREST.cs`](#) | Custom REST API wrapper for Firebase with GET/SET/PUSH/DELETE operations |
| [`Spawner_Manager.cs`](#) | Polls Firebase spawn requests and manages pet instantiation |
| [`ViewerScreen_UI_Manager.cs`](#) | Handles viewer queue, capacity checks, and spawn flow |
| [`BaseAnimal.cs`](#) | Core animal class managing health, breeding, and state coordination |
| [`BaseAnimator.cs`](#) | Animation controller base handling sprite animations and transitions |
| [`BaseBehavior.cs`](#) | Behavior management base for movement and state machine execution |
| [`BasePhysics.cs`](#) | Physics handling base for collisions and agent interactions |
| [`State.cs`](#) | State machine base class with enter/tick/exit pattern |
| [`Pet.cs`](#) | Passive animal implementation extending base animal structure |
| [`HostilePet.cs`](#) | Predator implementation with hunting and attack behaviors |

## Performance Metrics

- **Firebase Operations**: REST API calls every 2-3 seconds
- **Entity Capacity**: 20 concurrent autonomous pets with complex AI
- **Framerate**: Stable 144 FPS during peak activity
- **Memory**: Efficient resource management with proper cleanup
- **Response Time**: Real-time spawn request processing (<5s)

## Installation & Setup

### Prerequisites
- Unity 6000.0.59f2 LTS or later
- Firebase Realtime Database account
- Visual Studio 2019/2022 or VS Code
- Git for version control

### Quick Start
```bash
# Clone the repository
git clone https://github.com/Albvasper/Pets_Ecosystem.git

# Open in Unity
# File → Open Project → Select the cloned folder

# Configure Firebase
# 1. Create a Firebase Realtime Database
# 2. Set database URL in FirebaseREST component
# 3. Configure database rules for read/write access

# Play in Editor
# Open EcosystemScene and ViewerScene
# Press Play on both scenes
```

### Firebase Database Rules
```json
{
  "rules": {
    "ecosystem": {
      ".read": true,
      ".write": true
    }
  }
}
```

## Play

| Version | Link |
|---------|------|
| **Client** | [![Play](https://img.shields.io/badge/Play_on-itch.io-FA5C5C?logo=itchdotio&logoColor=white)](https://albvasper.itch.io/pumpets) |
| **Host** | [![Play](https://img.shields.io/badge/Play_on-itch.io-FA5C5C?logo=itchdotio&logoColor=white)](https://albvasper.itch.io/pets-ecosystem) |

## Screenshots

![Screenshot 1](.github/Screenshot (1).png)

![Screenshot 2](.github/Screenshot (1).png)

![Screenshot 3](.github/Screenshot (1).png)

## Technical Highlights

### Advanced Programming Concepts
- **Design Patterns**: State, Singleton, Template Method implementations
- **OOP Principles**: Abstract classes, inheritance hierarchies, polymorphism
- **Async Operations**: Coroutine-based Firebase polling and request handling
- **System Architecture**: Modular design with clear separation of concerns

### Unity Expertise
- **State Machine Framework**: Custom state system with virtual methods
- **Firebase Integration**: WebGL-compatible REST API wrapper
- **UI Systems**: Dynamic queue management and real-time feedback
- **Animation Control**: Runtime animator switching and parameter management

### Problem-Solving Skills
- **WebGL Compatibility**: Custom REST solution for Firebase SDK limitations
- **Queue System Design**: Fair and efficient viewer management
- **State Coordination**: Complex AI behaviors with clean transitions

## Contact

**Alberto Vásquez** - Game Programmer  
https://codebyalberto.framer.website/
