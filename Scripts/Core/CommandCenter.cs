using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Studio.Data;


namespace Studio {

public class CommandCenter : MonoBehaviour {

    public struct CommandInfo {
        public Action<string[]> Action;
        public string Help;
    }

    private Dictionary<string, CommandInfo> _registry = new Dictionary<string, CommandInfo>();

    // Event for UI to listen to logs (e.g. Debug Console)
    public event Action<string, LogType> OnCommandLog;

    public void init(){
        Log("Initializing...", LogType.Log);

        Register("CreateProject", Cmd_CreateProject, "Usage: CreateProject: Project Name");
        Register("LoadProject", Cmd_LoadProject, "Usage: LoadProject: Project Name");
        Register("ListProjects", Cmd_ListProjects, "Usage: ListProjects");
        Register("Help", Cmd_Help, "Usage: Help [CommandName]");
    }

    // --- API ---

    public void Register(string id, Action<string[]> action, string help=""){
        string key = id.ToLower();
        CommandInfo info = new CommandInfo { Action = action, Help = help };

        if(_registry.ContainsKey(key)){ _registry[key] = info; }
        else { _registry.Add(key, info); }
    }

    public void Execute(string input){
        if(string.IsNullOrEmpty(input)) return;

        Log($"Input: '{input}'", LogType.Log);

        string commandKey = "";
        string[] args = new string[0];

        // 1. Parse "Command: Arg1, Arg2"
        int colonIndex = input.IndexOf(':');

        if(colonIndex > -1){
            // Has arguments
            commandKey = input.Substring(0, colonIndex).Trim().ToLower();
            string argsRaw = input.Substring(colonIndex + 1);

            // Use generic Tools parser
            args = Tools.ParseCommandArguments(argsRaw);

        } else {
            // No arguments (e.g., "ListProjects")
            commandKey = input.Trim().ToLower();
        }

        // 2. Execute
        if(_registry.ContainsKey(commandKey)){
            try {
                _registry[commandKey].Action.Invoke(args);
            } catch(Exception e){
                Log($"Error in '{commandKey}': {e.Message}", LogType.Error);
            }
        } else {
            Log($"Unknown: '{commandKey}'", LogType.Warning);
        }
    }

    // --- HELPERS ---

    public bool Validate(string cmd, string[] args, int minCount){
        if(!Tools.ValidateArgs(args, minCount)){
            string help = _registry.ContainsKey(cmd.ToLower()) ? _registry[cmd.ToLower()].Help : "No help available.";
            Log($"Invalid Arguments. {help}", LogType.Error);
            return false;
        }
        return true;
    }

    private void Log(string msg, LogType type){
        string text = $"[CommandCenter] {msg}";
        // Send to Unity Console
        if(type == LogType.Error) Debug.LogError(text);
        else if(type == LogType.Warning) Debug.LogWarning(text);
        else Debug.Log(text);

        // Send to Subscribers
        OnCommandLog?.Invoke(text, type);
    }

    // --- HANDLERS ---

    private void Cmd_Help(string[] args){
        if(args.Length > 0 && !string.IsNullOrEmpty(args[0])){
            // Help for specific command
            string key = args[0].ToLower();
            if(_registry.ContainsKey(key)){
                Log($"{args[0]}: {_registry[key].Help}", LogType.Log);
            } else {
                Log($"Command '{args[0]}' not found.", LogType.Warning);
            }
        } else {
            // List all
            Log("--- Available Commands ---", LogType.Log);
            foreach(var kvp in _registry){
                Log($"{kvp.Key}: {kvp.Value.Help}", LogType.Log);
            }
        }
    }

    private void Cmd_CreateProject(string[] args){
        Log("Creating Project... \""+Tools.ArrayToString<string>(args)+"\"", LogType.Log);
        if(!Validate("CreateProject", args, 1)) return;

        Core.dbSetup.CreateProject(args[0]);
    }

    private void Cmd_LoadProject(string[] args){
        if(!Validate("LoadProject", args, 1)) return;
        Core.dbSetup.LoadProject(args[0]);
    }

    private void Cmd_ListProjects(string[] args){
        Log("--- Projects ---", LogType.Log);
        // Note: Core.DBM might be empty if we just started, need to check how DBM is populated.
        // Assuming Core.DBM keys are project names.
        foreach(var key in Core.DBM.Keys){ Log($"- {key}", LogType.Log); }
    }
}
}