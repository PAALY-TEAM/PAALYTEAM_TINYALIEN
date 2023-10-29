using UnityEngine;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    public static SceneFadeManager Instance;

    [SerializeField] private Image fadeOutImage; //what image are we referencing 
    [Range(0.1f, 10f), SerializeField] private float fadeOutSpeed = 5f;
    [Range(0.1f, 10f), SerializeField] private float fadeInSpeed = 5f;

    [SerializeField] private Color fadeOutStartColor;
    
    //public bool to know when we are fading in and out
    public bool IsFadingOut { get; private set; }
    public bool IsFadingIn { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        fadeOutStartColor.a = 0f;
    }
    //Update is where the fade happens
    private void Update()
    {
        if (IsFadingOut)
        {
            if (fadeOutImage.color.a < 1f)
            {
                fadeOutStartColor.a += Time.deltaTime * fadeOutSpeed;
                fadeOutImage.color = fadeOutStartColor;
            }
            else
            {
                IsFadingOut = false;
            }
        }

        if (IsFadingIn)
        {
            if (fadeOutImage.color.a > 0f)
            {
                fadeOutStartColor.a -= Time.deltaTime * fadeInSpeed;
                fadeOutImage.color = fadeOutStartColor;
            }
            else
            {
                IsFadingIn = false;
            }
        }
    }

    public void StartFadeOut()
    {
        fadeOutImage.color = fadeOutStartColor;
        IsFadingOut = true;
    }
    public void StartFadeIn()
    {
        if (fadeOutImage.color.a >= 1f)
        {
            fadeOutImage.color = fadeOutStartColor;
            IsFadingIn = true;
        }
    }
    
}