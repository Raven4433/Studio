using UnityEngine;
using Studio.UI;


namespace Studio {

public class Settings : MonoBehaviour {





    void Awake(){
        
    }


    void Start(){
        
    }


    public void init(){
        ThemeController.SetTheme(Core.ELEM.DefaultTheme);
    }

    //==========================================================================
    public void SettingsOpenClose(){
        Core.ELEM.SettingsGO.SetActive(!Core.ELEM.SettingsGO.activeSelf);
    }


    public void CreateProject(){
        string name = Core.ELEM.CreateProjectInput.text;
        
        if(string.IsNullOrEmpty(name)){ Debug.LogError("Project name cannot be empty"); return; }
        
        Core.ELEM.CreateProjectInput.text = "";
        Core.CMD.Execute("CreateProject" + ": " + name);

        // Core.dbSetup.CreateProject(name);
    }





}}
