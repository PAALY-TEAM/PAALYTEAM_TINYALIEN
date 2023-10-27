using UnityEngine;
using UnityEngine.UI;

namespace SceneLogic
{
    public class SceneFadeManager : MonoBehaviour
    {
        public static SceneFadeManager instance;

        [SerializeField] private Image _fadeOutImage; //what image are we referencing 
        [Range(0.1f, 10f), SerializeField] private float _fadeOutSpeed = 5f;
        [Range(0.1f, 10f), SerializeField] private float _fadeInSpeed = 5f;

        [SerializeField] private Color _fadeOutStartColor;
    
        //public bools to know when we are fadeing in and out
        public bool isFadingOut { get; private set; }
        public bool isFadingIn { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            _fadeOutStartColor.a = 0f;
        }
        //Update is where the fade happens
        private void Update()
        {
            if (isFadingOut)
            {
                if (_fadeOutImage.color.a < 1f)
                {
                    _fadeOutStartColor.a += Time.deltaTime * _fadeOutSpeed;
                    _fadeOutImage.color = _fadeOutStartColor;
                }
                else
                {
                    isFadingOut = false;
                }
            }

            if (isFadingIn)
            {
                if (_fadeOutImage.color.a > 0f)
                {
                    _fadeOutStartColor.a -= Time.deltaTime * _fadeInSpeed;
                    _fadeOutImage.color = _fadeOutStartColor;
                }
                else
                {
                    isFadingIn = false;
                }
            }
        }

        public void StartFadeOut()
        {
            _fadeOutImage.color = _fadeOutStartColor;
            isFadingOut = true;
        }
        public void StartFadeIn()
        {
            if (_fadeOutImage.color.a >= 1f)
            {
                _fadeOutStartColor.a -= Time.deltaTime * _fadeOutSpeed;
                _fadeOutImage.color = _fadeOutStartColor;
            }
            else
            {
                isFadingOut = false;
            }
        }
    
    }
}
