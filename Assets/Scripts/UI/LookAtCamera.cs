using UnityEngine;

namespace UI
{
    public class LookAtCamera : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (UnityEngine.Camera.main != null) transform.LookAt(UnityEngine.Camera.main.transform);
        }
    }
}
