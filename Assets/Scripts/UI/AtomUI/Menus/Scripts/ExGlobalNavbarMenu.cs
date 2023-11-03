using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

using UnityAtoms.BaseAtoms;
public class ExGlobalNavbarMenu : SimpleMenu<ExGlobalNavbarMenu>
{
    public Vector2 startSizeDelta; 
    
    void Start(){
        Debug.Log( startSizeDelta.ToString() );
    }
    // Update is called once per frame
    public void OnPausePressed(){
        Debug.Log("GlobalNavbarMenu :: OnPausePressed");
        ExPauseMenu.Show();
    }
}
