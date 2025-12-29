using UnityEngine;
using Studio.UI;


namespace Studio {

public class Settings : MonoBehaviour {


    [Header("UI Configuration")]
    public ThemeProfile DefaultTheme;




    void Awake(){
        
    }


    void Start(){
        
    }


    public void init(){
        if(DefaultTheme != null){ ThemeController.SetTheme(DefaultTheme); }
    }





}}
