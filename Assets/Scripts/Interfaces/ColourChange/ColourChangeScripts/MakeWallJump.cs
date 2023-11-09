using UnityEngine;

namespace Interfaces.ColourChange.Gameplay01
{
    public class MakeWallJump : MonoBehaviour, IColourChange
    {
        //Make so that it can rotate in different directions
        public void ColourChange()
        {
            transform.eulerAngles = new Vector3(91, 0, 0);
        }
    
    }
}
