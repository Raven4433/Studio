using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI.ProceduralImage; 

namespace Studio.UI {

public enum ElementType {
    WindowBG, PanelBG, Outline, 
    Button, ButtonAccent, ButtonDanger, InputField,
    TextPrimary, TextSecondary, TextHeader, TextAccent
}

public class ThemedUI : MonoBehaviour {

    public ElementType Type;
    
    // Components
    private Image _img; // Standard Unity Image
    private ProceduralImage _proceduralImg; // Generic Graphic to hold ProceduralImage if present
    private TMP_Text _txt;

    void Awake(){
        // Try getting ProceduralImage first (it inherits from Image, so we need to be specific if we want to distinguish)
        // We use GetComponent("ProceduralImage") or check type to avoid compile errors if asset is missing, 
        // OR simply rely on Image if ProceduralImage behaves like one for Color.
        
        // However, typically ProceduralImage inherits from Image. 
        // So _img = GetComponent<Image>() works for BOTH.
        _img = GetComponent<Image>();
        _txt = GetComponent<TMP_Text>();
        
        // Optimization: Check if it is actually procedural to decide on Sprite swapping logic
        // (Procedural images usually don't want their sprite swapped by the theme system)
        var procCheck = GetComponent("ProceduralImage");
        if(procCheck != null){ _proceduralImg = procCheck as ProceduralImage; }
    }

    void Start(){
        ThemeController.OnThemeChanged += ApplyTheme;
        ApplyTheme(); 
    }

    void OnDestroy(){
        ThemeController.OnThemeChanged -= ApplyTheme;
    }

    public void ApplyTheme(){
        var p = ThemeController.Current;
        if(p == null) return;

        if(_img != null){ ApplyImageStyle(p); }
        if(_txt != null){ ApplyTextStyle(p); }
    }

    void ApplyImageStyle(ThemeProfile p){
        // Color is always applied
        Color targetColor = Color.white;
        Sprite targetSprite = null;

        switch(Type){
            case ElementType.WindowBG:
                targetColor = p.WindowBG;
                targetSprite = p.WindowFrame;
                break;
            case ElementType.PanelBG:
                targetColor = p.PanelBG;
                targetSprite = p.PanelBackground;
                break;
            case ElementType.Outline:
                targetColor = p.Outline;
                targetSprite = null;
                break;
            case ElementType.Button:
                targetColor = p.PanelBG; 
                targetSprite = p.ButtonNormal;
                break;
            case ElementType.ButtonAccent:
                targetColor = p.Accent;
                targetSprite = p.ButtonNormal;
                break;
            case ElementType.InputField:
                targetColor = p.PanelBG;
                targetSprite = p.InputField;
                break;
        }

        
        if(_img != null){ 
            _img.color = targetColor;
            _img.sprite = targetSprite; 
        }

        if(_proceduralImg != null){ 
            _proceduralImg.color = targetColor;
            _proceduralImg.sprite = targetSprite; 
        }
    }

    void ApplyTextStyle(ThemeProfile p){
        switch(Type){
            case ElementType.TextPrimary:
                _txt.color = p.TextPrimary;
                if(p.MainFont != null) _txt.font = p.MainFont;
                break;
            case ElementType.TextSecondary:
                _txt.color = p.TextSecondary;
                if(p.MainFont != null) _txt.font = p.MainFont;
                break;
            case ElementType.TextHeader:
                _txt.color = p.TextPrimary;
                if(p.HeaderFont != null) _txt.font = p.HeaderFont;
                break;
            case ElementType.TextAccent:
                _txt.color = p.TextAccent;
                if(p.MainFont != null) _txt.font = p.MainFont;
                break;
        }
    }
    
    void OnValidate(){
        if(!Application.isPlaying && ThemeController.Current != null){
            Awake(); 
            ApplyTheme();
        }
    }
}
}