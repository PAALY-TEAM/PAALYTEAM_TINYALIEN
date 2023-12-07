using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class LookAtCamera : MonoBehaviour
    {
        void Update()
        {
            // Make Panel always turn towards camera
            if (UnityEngine.Camera.main != null) transform.LookAt(UnityEngine.Camera.main.transform);
        }
    }

}
