using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityAtoms.BaseAtoms;

public class ExTitleMenu : SimpleMenu<ExTitleMenu>{
    // Start is called before the first frame update
    [SerializeField]
    private BoolVariable playerCanMove;
    public void OnNewPressed(){

        Debug.Log("TitleMenu :: OnNewPressed");
        
        ExGlobalNavbarMenu.Show();

    }
    public override void OnBackPressed(){

        #if UNITY_EDITOR
        Debug.Log("Quit Scene");
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
