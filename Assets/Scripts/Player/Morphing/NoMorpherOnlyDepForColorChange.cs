using UnityEngine;

namespace Morphing
{
    public class NoMorpherOnlyDepForColorChange : MonoBehaviour
    {
        public GameObject sourceObject;
        
        [SerializeField] private new Renderer renderer;
        
        [Range(0f, 1f)]
        [SerializeField] private float slider;

        private Material _finalMaterial;

        void Start()
        {
            _finalMaterial = sourceObject.GetComponent<Renderer>().sharedMaterial;
        }
        void Update()
        {
            Material currentMaterial = sourceObject.GetComponent<Renderer>().sharedMaterial;
            if (_finalMaterial != currentMaterial)
            {
                _finalMaterial = currentMaterial;
                renderer.material = _finalMaterial;
            }
        }
        public void SetSlider(float slider)
        {
            this.slider = slider;
        }

    }
}