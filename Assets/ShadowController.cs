using UnityEngine;

namespace MoreMountains.Tools
{
    public class ShadowController : MonoBehaviour
    {
        public GameObject shadow;

        public void UpdateShadowStatus(bool isGrounded)
        {
            shadow.SetActive(isGrounded);
        }
    }
}

