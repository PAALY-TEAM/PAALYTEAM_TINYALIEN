using UnityEngine;
using UnityEngine.UI;

namespace Pickup.Shade
{
    public class ShowColour : MonoBehaviour
    {
        [SerializeField] private Sprite[] colours;

        private Image _thisImage;
        private void Awake()
        {
            _thisImage = GetComponent<Image>();
            _thisImage.sprite = null;
        }

        public void ChangeIcon(int colourIndex)
        {
            if (colourIndex == 0)
            {
                _thisImage.sprite = null;
                return;
            }
            _thisImage.sprite = colours[colourIndex-1];
        }
    }
}
