using UnityEngine;
using TMPro;

namespace Studio.UI {

[CreateAssetMenu(fileName = "NewTheme", menuName = "Studio/Theme Profile")]
public class ThemeProfile : ScriptableObject {

    [Header("Core Colors")]
    public Color WindowBG = new Color(0.1f, 0.1f, 0.1f);
    public Color PanelBG = new Color(0.2f, 0.2f, 0.2f);
    public Color Outline = new Color(0.37f, 0.37f, 0.37f);
    public Color Accent = new Color(0f, 0.5f, 1f); // Google Blue
    public Color Danger = new Color(1f, 0.2f, 0.2f);

    [Header("Text Colors")]
    public Color TextPrimary = Color.white;
    public Color TextSecondary = Color.gray;
    public Color TextAccent = Color.cyan;

    [Header("Skins (Sprites)")]
    // If null, simple color tinting is used. If assigned, sliced sprites are used.
    public Sprite WindowFrame; 
    public Sprite PanelBackground;
    public Sprite ButtonNormal;
    public Sprite ButtonHover;
    public Sprite InputField;

    [Header("Typography")]
    public TMP_FontAsset MainFont;
    public TMP_FontAsset HeaderFont;
}
}