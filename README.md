# Dungeon Generator – Co-op Online Dungeon Crawler (Unity Prototype)

A diploma project prototype of a cooperative online dungeon crawler built in Unity. Features **procedurally generated dungeons** and **real-time multiplayer over Steam**, powered by Mirror and Steamworks.NET.

> 🎯 **Purpose**: Demonstrate core gameplay systems — procedural level generation, networked player interaction, and Steam integration — in a playable Unity prototype.

---

## ✨ Implemented Features

- **Procedural Dungeon Generation**  
  Multi-level dungeons are generated randomly using hand-crafted room and corridor prefabs. Room connectivity is established by building a **minimum spanning tree**, ensuring full traversability. The system supports **configurable loops** — you can specify the number of cycles and set a minimum cycle length (in rooms). Corridor routing between rooms uses the **A\*  pathfinding algorithm** for natural layouts. Adjacent dungeon levels are connected via special **transition rooms** (e.g., stairs or elevators).

- **Steam-based Multiplayer (2–4 players)**  
  Host-or-join lobby system using **Steam P2P networking**. All player actions (movement, interactions) are synchronized in real time.

- **Expandable Inventory & Usable Items System**  
  Players can pick up, drop, select, and use items (e.g., sword, fireball staff) during gameplay. The system distinguishes between **pickable world items** and **inventory slot items**, adhering to the Single Responsibility Principle. New item types with custom behaviours are added by subclassing the base `InventoryItem` class. **All actions — pickup, drop, usage — are fully synchronized across the network** using Mirror’s server-authoritative model, ensuring consistent state for all players.

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
### Generated one-level dungeons with different room count

<img width="1210" height="917" alt="image" src="https://github.com/user-attachments/assets/19310349-4031-4b84-be76-613d53d57ab0" />

### Two-level dungeon example with *transition rooms*

<img width="860" height="390" alt="image" src="https://github.com/user-attachments/assets/1e502b20-f6a6-4947-a7d6-1c64ee4f8f75" />

### Dungeoun from player perspective

<img width="1564" height="879" alt="image" src="https://github.com/user-attachments/assets/829f0d8c-ba1e-4c51-893c-31f048034e25" />



---
