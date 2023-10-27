using UnityEngine;

namespace Camera
{
    public class HideCursor : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
}
