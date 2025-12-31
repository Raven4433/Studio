using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro; // To check for input field focus

namespace Studio {

[System.Flags]
public enum Modifier { None = 0, Ctrl = 1, Shift = 2, Alt = 4 }

public class KeyCombo {
    public KeyCode Key;
    public Modifier Mods;
    public Action Callback;
    public string Description;
}

public class Hotkeys : MonoBehaviour {

    private Dictionary<string, KeyCombo> Registry = new Dictionary<string, KeyCombo>();
    private bool isInputFocused = false;

    void Update(){
        // 1. Check for Input Field Focus (Block hotkeys while typing)
        // Optimization: Only check this if a key is actually pressed to save CPU
        if(Input.anyKeyDown){
            CheckInputFocus();
            if(isInputFocused) return; // Stop processing hotkeys
            
            ProcessInput();
        }
    }

    void ProcessInput(){
        Modifier currentMods = Modifier.None;
        if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) currentMods |= Modifier.Ctrl;
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) currentMods |= Modifier.Shift;
        if(Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) currentMods |= Modifier.Alt;

        foreach(var kvp in Registry){
            KeyCombo combo = kvp.Value;
            
            // Check Modifiers Match Exact
            if(combo.Mods != currentMods) continue;

            // Check Key Down
            if(Input.GetKeyDown(combo.Key)){
                combo.Callback?.Invoke();
                // Debug.Log($"[Hotkeys] Triggered: {kvp.Key}");
                return; // Consume input (first match wins)
            }
        }
    }

    void CheckInputFocus(){
        // Check standard TMP Input Fields
        var input = UnityEngine.EventSystems.EventSystem.current?.currentSelectedGameObject;
        if(input != null){
            isInputFocused = input.GetComponent<TMP_InputField>() != null;
        } else {
            isInputFocused = false;
        }
    }

    // --- API ---

    public void Register(string id, KeyCode key, Modifier mods, Action action, string desc = ""){
        if(Registry.ContainsKey(id)){
            // Overwrite existing (allows rebinds)
            Registry[id].Key = key;
            Registry[id].Mods = mods;
            Registry[id].Callback = action;
        } else {
            Registry.Add(id, new KeyCombo{ Key = key, Mods = mods, Callback = action, Description = desc });
        }
    }

    public void Unregister(string id){
        if(Registry.ContainsKey(id)) Registry.Remove(id);
    }
    
    public void ClearAll() { Registry.Clear(); }
}
}