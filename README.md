# Dungeon Generator – Co-op Online Dungeon Crawler (Unity Prototype)

A diploma project prototype of a cooperative online dungeon crawler built in Unity. Features **procedurally generated dungeons** and **real-time multiplayer over Steam**, powered by Mirror and Steamworks.NET.

> 🎯 **Purpose**: Demonstrate core gameplay systems — procedural level generation, networked player interaction, and Steam integration — in a playable Unity prototype.

---

## ✨ Implemented Features

- **Procedural Dungeon Generation**  
  Multi-level dungeons are generated randomly using hand-crafted room and corridor prefabs. Room connectivity is established by building a **minimum spanning tree**, ensuring full traversability. The system supports **configurable loops** — you can specify the number of cycles and set a minimum cycle length (in rooms). Corridor routing between rooms uses the **A* pathfinding algorithm** for natural layouts. Adjacent dungeon levels are connected via special **transition rooms** (e.g., stairs or elevators).

- **Steam-based Multipline Multiplayer (2–4 players)**  
  Host-or-join lobby system using **Steam P2P networking**. All player actions (movement, interactions) are synchronized in real time.

- **Networked Player Avatars**  
  Smooth movement replication, player instantiation/destruction, and basic state sync (health, position, etc.) via **Mirror**.

- **Expandable Inventory & Usable Items System**  
  Players can collect, store, and use items (e.g., health potions, keys). The inventory is implemented as a slot-based system with support for new item types via scriptable objects. **All inventory changes and item usage are fully synchronized across the network** using Mirror’s server-authoritative model.

- **In-Game UI & Steam Overlay Integration**  
  Main menu, lobby screen with Steam friend list support, and in-game HUD (including inventory panel) — all functional in networked sessions.

---

## 🛠️ Technologies & Tools

![Unity](https://img.shields.io/badge/Unity-2022+-5281D4?logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-125688?logo=csharp&logoColor=white)
![Mirror](https://img.shields.io/badge/Mirror-Networking-8A2BE2)
![Steamworks.NET](https://img.shields.io/badge/Steamworks.NET-Steam_API-000000?logo=steam&logoColor=white)
![Windows](https://img.shields.io/badge/Platform-Windows-0078D7?logo=windows)

---

## 🖼️ Screenshots
Coming soon
<!-- Add 2–4 high-quality screenshots or short GIFs here -->
<!-- Example: -->
<!-- ![Dungeon Screenshot](images/screenshot1.png) -->
<!-- ![Multiplayer Lobby](images/screenshot2.png) -->


---

## ▶️ How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/Yudjerick/DungeonGenerator.git
