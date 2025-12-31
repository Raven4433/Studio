using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Studio.Data;


namespace Studio { 

public class CommandCenter : MonoBehaviour {

    private Dictionary<string, Action<string[]>> _registry = new Dictionary<string, Action<string[]>>();

    public void init(){
        Debug.Log("[CommandCenter] Initializing...");
        
        Register("CreateProject", Cmd_CreateProject, "Usage: CreateProject: Project Name");
        Register("LoadProject", Cmd_LoadProject, "Usage: LoadProject: Project Name");
        Register("ListProjects", Cmd_ListProjects, "Usage: ListProjects");
    }

    // --- API ---

    public void Register(string id, Action<string[]> action, string help=""){
        string key = id.ToLower();
        if(_registry.ContainsKey(key)){ _registry[key] = action; }
        else { _registry.Add(key, action); }
    }

    public void Execute(string input){
        if(string.IsNullOrEmpty(input)) return;

        Debug.Log($"[CommandCenter] Input: '{input}'");

        string commandKey = "";
        string[] args = new string[0];

        // 1. Parse "Command: Arg1, Arg2"
        int colonIndex = input.IndexOf(':');

        if(colonIndex > -1){
            // Has arguments
            commandKey = input.Substring(0, colonIndex).Trim().ToLower();
            string argsRaw = input.Substring(colonIndex + 1);
            
            // Split by comma, Trim each part
            args = argsRaw.Split(new char[] { ',' }, StringSplitOptions.None)
                          .Select(s => s.Trim())
                          .ToArray();
        } else {
            // No arguments (e.g., "ListProjects")
            commandKey = input.Trim().ToLower();
        }

        // 2. Execute
        if(_registry.ContainsKey(commandKey)){
            try {
                _registry[commandKey].Invoke(args);
            } catch(Exception e){
                Debug.LogError($"[CommandCenter] Error in '{commandKey}': {e.Message}");
            }
        } else {
            Debug.LogWarning($"[CommandCenter] Unknown: '{commandKey}'");
        }
    }

    // --- HANDLERS ---

    private void Cmd_CreateProject(string[] args){
        Debug.Log("Creating Project... \""+Tools.ArrayToString<string>(args)+"\"");
        if(args.Length < 1 || string.IsNullOrEmpty(args[0])){ 
            Debug.LogError("Usage: CreateProject: Name"); return; 
        }
        Core.dbSetup.CreateProject(args[0]);
    }

    private void Cmd_LoadProject(string[] args){
        if(args.Length < 1 || string.IsNullOrEmpty(args[0])){ 
            Debug.LogError("Usage: LoadProject: Name"); return; 
        }
        Core.dbSetup.LoadProject(args[0]);
    }

    private void Cmd_ListProjects(string[] args){
        Debug.Log("--- Projects ---");
        foreach(var key in Core.DBM.Keys){ Debug.Log($"- {key}"); }
    }
}
}