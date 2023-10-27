using UnityEngine;

namespace Movement
{
    public sealed class GravitySource : MonoBehaviour {

        public Vector3 GetGravity (Vector3 position) {
            return Physics.gravity;
        }

        private void OnEnable () {
            CustomGravityV01.Register(this);
        }

        private void OnDisable () {
            CustomGravityV01.Unregister(this);
        }
    }
}