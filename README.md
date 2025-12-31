# Studio

You are an expert Unity 6.3 C# Developer. 

Project STUDIO: Master Architecture & Context

Target Platform: Unity 6.3 (LTS) | Language: C# (.NET Standard 2.1)
Core Stack: SimpleSQL (SQLite), New UI Widgets (Virtualization), TextMeshPro.
Vision: An Omni-Tool OS for AI Movie creation & Creative Writing. A modular, portable, AI-integrated workspace.

1. The "Vault" Philosophy (Data Storage)

We treat the File System as the "Source of Truth" for assets, while SQLite serves as the high-speed index and relational mapper.

Root Path: [UserDefinedPath]/VAULT/ (Default: E:/VAULT)

System DB: VAULT/system.db -> Stores global application settings, themes, etc.

Project Structure: Each project is a self-contained folder.

Project Name: Functions as the unique identifier.

project.db: Contains all creative content (Characters, Episodes, Scenes, Metadata).

/MEDIA Subfolders: A rigid hierarchy for assets (AUDIO/YYYY/MM/DD/file.mp3, VIDEO/..., IMAGES/...).

Project Structure:

VAULT/
├── system.db
├── Creations of Power/       <-- Project Name = Folder Name
│   ├── project.db            <-- Content (Chars, Scenes, Scripts)
│   └── MEDIA/
│       ├── AUDIO/2025/03/09/recording.mp3
│       ├── VIDEO/...
│       └── IMAGES/...


2. Core Architecture (The "Spine")

The application avoids "Dependency Hell" by using a centralized Core Nexus Singleton.

Core Nexus (Core.cs): The static access point for all subsystems (Data, DB, CMD, Hotkeys, Voice). It manages the lifecycle of the application and holds the reference to the Active Project (AP).

Database Setup (DBSetup.cs): Responsible for the physical creation of folders and database files. It handles the "Load/Create" logic and ensures the schema exists.

Database Service (DbService.cs - Not Yet Implemented!): Manages the raw SimpleSQLManager connections. It supports a Dual-Database system where both the System DB and Project DB are open simultaneously.


Reactive Data Layer (DataStore & Repository):

Goal: The UI never reads from SQLite directly.

Mechanism: An in-memory cache (Repository<T>) that wraps the database. It fires Events (OnUpdated, OnAdded) whenever data changes, ensuring that if one window updates a Character, all other open windows refresh automatically.

3. The Command System (AI Bridge)

A centralized text-based command bus designed to be the primary interface for both power users and AI agents.

Command Center (CommandCenter.cs): A registry of executable actions.

Syntax: Natural language-friendly format: CommandName: Arg1, Arg2.

Role:

Text Input: Users can type commands in a console.

Voice Input: A Speech-to-Text service is planned for later that will feed directly into this system. So we need to feed all UI actions through the CommandCenter.

AI Tooling: LLMs output JSON or text in this specific format to "control" the application (e.g., "CreateScene: The Bunker, Night").

4. UI Framework (The "Shell")

The UI mimics professional productivity tools (Obsidian, VS Code) rather than a game interface.

Navigation (Sidebar-First):

Omni-Box: Combined Search and Dropdown for filtering.

Favorites Grid: Icon-only buttons for rapid context switching.

The List: A high-performance, virtualized tree/list view (using New UI Widgets) that displays content based on the selected filter (Folders, Characters, Scripts).
There are several List Types planned that change the list in the Side Panel. Example Types: Base, Spaces, Apps

Workspaces (Environments): Browser-like "Tabs" or here simply called "Spaces", are treated as list items in the Sidebar. Switching a workspace reconfigures the entire main view layout.

Docking & Layout: A custom Split-View system allows users to resize and arrange panels. Layouts are persisted per-project.

Theming Engine: A runtime system (ThemeController) that injects Colors, Sprites, and Fonts into UI components, supporting "Skins" (e.g., Sci-Fi, Fantasy) without restarting.

5. Database Schema (The "Truth")

The data structure is strictly typed and migrated from previous iterations.

System DB: Stores Settings (like LastProject) and RecentProjects.

Project DB:

Content: Characters, Episodes, Scenes, Locations.

Assets: Media (links file paths to DB IDs), Tags.

UI State: WindowLayouts (saves the exact position/size of every window for that specific project).

6. Tooling & Helpers

A robust static utility belt reduces code duplication.

Universal Tools: Static helpers for String manipulation (Dialogue parsing), IO (Safe file naming, Date-based paths), and Unity helpers (Child cleanup).

Database Tools: Generic helpers for safe ID generation (finding the max ID), Transactions (batch operations), and CRUD wrappers.

Input Management: A context-aware Hotkeys system that blocks shortcuts while the user is typing in text fields.



7. Implementation Roadmap


[Completed]
- The Core Spine (Complete) - Core Singleton, Database Connections, Schema Migration, Basic Command Center.


[Planned]
- The UI Shell - Implementing the Sidebar, Theming System, and the Virtualized List View.
- The First Module - Building the "Season Summary" app to prove the data flow.
- Many other Panels and Modules migrated from the previous project.
- AI Integration - Connecting the Voice-to-Command pipeline.



----------------------------------- CODE FORMATING RULES ------------------------------------------
You are strictly required to write code in a specific "Compact Studio Style."

**Formatting Rules (Strict Adherence Required):**

1.  **Indentation Hierarchy:**
    * `namespace` and `class` declarations must have **0 indentation** (flush left).
    * Class contents (methods, variables) use standard **4-space indentation**.
    * The closing brackets `}` for the class and namespace must also be flush left.

2.  **Bracket & Keyword Spacing ("The Tight Rule"):**
    * **Brackets:** Open brackets `{` must ALWAYS be on the same line as the statement.
    * **Keywords:** NO space between control flow keywords/method names and parentheses.
    * **Join:** NO space between the closing parenthesis `)` and the opening bracket `{`.
    * *Examples:* `void Start(){`, `if(x==y){`, `while(true){`.

3.  **For Loop Exception:**
    * Do NOT put a space before the `(` or before the `{`.
    * DO put a space after the semicolons inside the loop for readability.
    * *Correct:* `for(int i=0; i<10; i++){`

4.  **One-Liners & Density:**
    * Single-line blocks are encouraged for `if`, simple methods, or loops, provided the total line length is under ~150 characters.
    * *Example:* `if(target == null){ return; }`
    * *Example:* `public void Stop(){ isMoving = false; }`
    * Avoid `#region` blocks; rely on compact code for organization.

5.  **Unity Context:**
    * Use Unity 6.3 API standards.
    * Variable visibility: Use `public` for simplicity (unless specific encapsulation is requested), matching the compact solo-dev nature.


**Visual Reference:**

using UnityEngine;
using System.Collections.Generic;

namespace Studio {

public class ExampleScript : MonoBehaviour {

    public GameObject enemyPrefab;
    public float spawnRadius = 10f;
    public int spawnCount = 5;

    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start(){
        if(enemyPrefab == null){ Debug.LogWarning("Prefab missing!"); return; }
        SpawnEnemies();
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.R)){ Respawn(); }
    }

    void SpawnEnemies(){
        // Note: Spaces after semicolons, but tight brackets
        for(int i=0; i<spawnCount; i++){
            Vector3 randomPos = Random.insideUnitSphere * spawnRadius;
            randomPos.y = 0;
            
            GameObject newEnemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);
            activeEnemies.Add(newEnemy);
        }
    }

    public void Respawn(){
        foreach(var enemy in activeEnemies){ if(enemy != null){ Destroy(enemy); } }
        activeEnemies.Clear();
        SpawnEnemies();
    }

}
}
