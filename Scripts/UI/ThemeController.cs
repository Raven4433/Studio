using System;
using UnityEngine;

namespace Studio.UI {

public static class ThemeController {

    public static ThemeProfile Current { get; private set; }
    public static event Action OnThemeChanged;

    public static void SetTheme(ThemeProfile newProfile){
        if(newProfile == null) return;
        
        Current = newProfile;
        OnThemeChanged?.Invoke();
        
        Debug.Log($"[ThemeController] Switched to: {newProfile.name}");
    }
}
}