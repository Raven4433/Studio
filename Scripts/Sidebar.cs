
using UnityEngine;


namespace Studio {

public class Sidebar : MonoBehaviour {
    





    void Awake(){
        
    }



    void Start(){

        Core.hotkeys.Register("OpenCloseSidebar", KeyCode.F1, Modifier.None, OpenCloseSidebar);

    }




    void OpenCloseSidebar(){
        gameObject.SetActive(!gameObject.activeSelf);
    }





    void OnDestroy(){
        // Always unregister to prevent memory leaks or ghost calls
        if(Core.hotkeys != null){ Core.hotkeys.Unregister("OpenCloseSidebar"); }
    }




}
}
