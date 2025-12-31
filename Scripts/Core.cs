using UnityEngine;
using SimpleSQL;
using System.Collections.Generic;
using Studio.Data;


namespace Studio {

public class Core : MonoBehaviour {


    public static Core Instance { get; private set; }
    public static Elements ELEM { get; private set; }
    public static Settings settings { get; private set; }

    public static Hotkeys hotkeys { get; private set; }
    public static DataStore Data { get; private set; }
    public static DBSetup dbSetup { get; private set; }

    public static CommandCenter CMD { get; private set; }
    
    

    // Core.CMD.Execute("CreateProject Matrix");


    public static Dictionary<string, SimpleSQLManager> DBM = new Dictionary<string, SimpleSQLManager>();


    public static string AP = ""; // Active Project Name - DBM[AP]
    public static string VaultRoot = "E:/VAULT";
    



    void Awake(){
        Instance = this;  Data = new DataStore();

        ELEM = gameObject.GetComponent<Elements>();
        settings = gameObject.GetComponent<Settings>(); settings.init();

        CMD = gameObject.AddComponent<CommandCenter>(); CMD.init();
        dbSetup = gameObject.AddComponent<DBSetup>();  dbSetup.init();  
        hotkeys = gameObject.AddComponent<Hotkeys>();

    }



}
}



