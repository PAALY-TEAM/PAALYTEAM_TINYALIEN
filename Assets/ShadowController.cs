using UnityEngine;
using DG.Tweening;

public class ShadowController : MonoBehaviour
{
    [SerializeField]
    private GameObject shadow;
    [SerializeField]
    private Vector3 groundedScale = new Vector3(1, 0.09f, 1); // Scale when player is on ground
    [SerializeField]
    private Vector3 airborneScale = Vector3.zero;    // Scale when player is not on ground
    [SerializeField]
    private float scaleUpDuration = 1f;              // Duration for scaling up
    [SerializeField]
    private float scaleDownDuration = 0.5f; // Duration for scaling down

    public void UpdateShadowStatus(bool isGrounded)
    {
        if (isGrounded)
        {
            // Scale to groundedScale when on ground
            shadow.transform.DOScale(groundedScale, scaleUpDuration);
        }
        else
        {
            // Scale to airborneScale when leaving ground
            shadow.transform.DOScale(airborneScale, scaleDownDuration);
        }
    }
}
