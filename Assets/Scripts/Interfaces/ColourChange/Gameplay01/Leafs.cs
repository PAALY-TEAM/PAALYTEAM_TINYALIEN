using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Interfaces.ColourChange.Gameplay01
{
    public class Leafs : MonoBehaviour, IColourChange
    {
        private bool _isColoured = false;
        private Vector3 _scaleChange;
        int _dir= 1;
    

        public void ColourChange()
        {
            _isColoured = true;
        }

        private void Update()
        {
            if (_isColoured)
            {
                transform.localScale += new UnityEngine.Vector3(1, 1, 1) * (_dir * Time.deltaTime);
                if (transform.localScale.y <  1)
                {
                    _dir = 1;
                }
                else if (transform.localScale.y > 2)
                {
                    _dir = -1;
                }
            }
        }
    }
}

