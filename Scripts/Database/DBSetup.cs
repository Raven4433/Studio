using UnityEngine;
using SimpleSQL;
using System.IO;
using System.Collections.Generic;
using System;



namespace Studio.Data {

public class DBSetup : MonoBehaviour {



    private GameObject Library;


    void Start(){
        Core.dbSetup.CreateProject("Creations of Power");
        // **Verify:** Go to your `E:/VAULT` folder in Windows Explorer. You should see the folder "Creations of Power" with `project.db` and the `MEDIA` subfolders inside.
    }

    
    public void init(){  
        Library = GameObject.Find("LIBRARY");
  
        // 1. Ensure Vault Root Exists
        if(!Directory.Exists(Core.VaultRoot)){ Directory.CreateDirectory(Core.VaultRoot); }
        
        // 2. Load System DB
        InitializeSystemDB();

        // 1. Scan for all projects
        LoadAllProjects();
        
        // 2. Auto-open the last one
        //LoadLastProject();
    }

    // --- SYSTEM DB ---
    public void InitializeSystemDB(){
        if(Core.DBM.ContainsKey("system")){ return; }

        SimpleSQLManager dbm = CreateManager("DB_System", Core.VaultRoot, "system.db");
        
        // Create Schema if new
        dbm.CreateTable<Tags>();
        dbm.CreateTable<TagLinks>();

        Core.DBM.Add("system", dbm);
        Debug.Log("[DBSetup] System DB Initialized.");
    }

    // --- PROJECT HANDLING ---

    public void CreateProject(string projectName){
        string projectPath = Path.Combine(Core.VaultRoot, projectName);

        // 1. Create Folder Structure
        if(Directory.Exists(projectPath)){ Debug.LogError($"Project {projectName} already exists!"); return; }
        
        Directory.CreateDirectory(projectPath);
        CreateMediaFolders(projectPath);

        // 2. Create & Init DB
        SimpleSQLManager projDB = CreateManager($"DB_{projectName}", projectPath, "project.db");
        
        // 3. Create Content Schema
        projDB.CreateTable<Media>();
        projDB.CreateTable<AIPrompts>();
        projDB.CreateTable<Characters>();
        projDB.CreateTable<CharacterData>();
        projDB.CreateTable<CharacterLinks>();



        // 5. Load it into Core
        if(Core.DBM.ContainsKey(projectName)){ Core.DBM.Remove(projectName); }
        Core.DBM.Add(projectName, projDB);
        
        Debug.Log($"[DBSetup] Project '{projectName}' Created & Loaded.");
    }

    public void LoadProject(string projectName){
        string projectPath = Path.Combine(Core.VaultRoot, projectName);
        
        if(!Directory.Exists(projectPath)){ Debug.LogError($"Project {projectName} not found!"); return; }

        if(Core.DBM.ContainsKey(projectName)){ Debug.Log("Project already loaded."); return; }

        SimpleSQLManager projDB = CreateManager($"DB_{projectName}", projectPath, "project.db");
        Core.DBM.Add(projectName, projDB);
        
        Debug.Log($"[DBSetup] Project '{projectName}' Loaded.");
    }


    public void LoadAllProjects(){
        string[] directories = Directory.GetDirectories(Core.VaultRoot);
        
        foreach(string dir in directories){
            string folderName = new DirectoryInfo(dir).Name;
            string dbPath = Path.Combine(dir, "project.db");

            // Only consider it a project if it has a project.db file
            if(File.Exists(dbPath)){
                // Load it (or just register it if we want lazy loading later)
                // For now, let's load everything to populate Core.DBM
                // Optimization: In the future, we might only want to read the list, not open every DB connection.
                
                // NOTE: Loading ALL DBs at startup might be heavy if you have 50 projects.
                // Usually we just want to know they exist.
                // But per your request "loads all of them automatically":
                LoadProject(folderName);
            }
        }
    }

    // --- LAST PROJECT LOGIC ---

    public void LoadLastProject(){
        var settingsList = Core.DBM["system"].Query<Settings>("SELECT * FROM Settings WHERE ID = 1");
        
        if(settingsList != null && settingsList.Count > 0){
            string lastProj = settingsList[0].LastProject;
            if(!string.IsNullOrEmpty(lastProj)){
                Debug.Log($"[DBSetup] Loading Last Project: {lastProj}");
                LoadProject(lastProj);
            }
        }
    }

    private void SetLastProject(string projectName){
        var settingsList = Core.DBM["system"].Query<Settings>("SELECT * FROM Settings WHERE ID = 1");
        Settings settings;
        
        if(settingsList != null && settingsList.Count > 0){
            settings = settingsList[0];
        } else {
            settings = new Settings{ ID = 1 };
        }
        
        settings.LastProject = projectName;
        Core.DBM["system"].Insert(settings);
    }

    // ----------------- HELPERS -----------------

    private SimpleSQLManager CreateManager(string goName, string basePath, string fileName){
        GameObject go = new GameObject(goName);
        go.transform.parent = Library.transform;
        
        SimpleSQLManager dbm = go.AddComponent<SimpleSQLManager>();
        
        // Force Absolute Path Logic
        dbm.databaseFile = null;
        dbm.overridePathMode = SimpleSQLManager.OverridePathMode.Absolute;
        dbm.overrideBasePath = basePath; 
        dbm.changeWorkingName = true;
        dbm.workingName = fileName;

        
        dbm.Initialize(true); // 'true' usually forces persistence in SimpleSQL
        return dbm;
    }

    private void CreateMediaFolders(string root){
        string media = Path.Combine(root, "MEDIA");
        Directory.CreateDirectory(Path.Combine(media, "AUDIO"));
        Directory.CreateDirectory(Path.Combine(media, "VIDEO"));
        Directory.CreateDirectory(Path.Combine(media, "IMAGE"));
    }




}
}
