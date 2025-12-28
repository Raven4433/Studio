using UnityEngine;
using SimpleSQL;
using System.Collections.Generic;
using Studio.Data;


namespace Studio {

public class Core : MonoBehaviour {


    public static Core Instance { get; private set; }



    public static DataStore Data { get; private set; }
    public static DBSetup dbSetup { get; private set; }
    



    public static Dictionary<string, SimpleSQLManager> DBM = new Dictionary<string, SimpleSQLManager>();


    public static string AP = ""; // Active Project Name - DBM[AP]
    public static string VaultRoot = "E:/VAULT";
    


    void Awake(){
        Instance = this;

        Data = new DataStore();
        dbSetup = gameObject.AddComponent<DBSetup>();  dbSetup.init();

     
        
    }


    
    void Start(){



    }

    
    void Update(){
        
    }









}
}
