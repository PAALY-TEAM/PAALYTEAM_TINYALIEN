using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Feel.FeelDemos._Common.Scripts
{
	/// <summary>
	/// This class handles the instruction texts that appear in the Feel demo scenes
	/// </summary>
	public class FeelDemosInstructions : MonoBehaviour
	{
		[FormerlySerializedAs("TargetText")] [Header("Bindings")]
		/// a text component where we'll display instructions
		public Text targetText;
		/// the delay, in seconds, before instructions disappear
		[FormerlySerializedAs("DisappearDelay")] public float disappearDelay = 3f;
		/// the duration, in seconds, of the instructions disappearing transition
		[FormerlySerializedAs("DisappearDuration")] public float disappearDuration = 0.3f;

		[FormerlySerializedAs("DesktopText")] [Header("Texts")]
		/// the text to display when running the demos on desktop
		public string desktopText = "Press space to...";
		/// the text to display when running the demos on mobile
		[FormerlySerializedAs("MobileText")] public string mobileText = "Tap anywhere to...";

		protected CanvasGroup CanvasGroup;
        
		/// <summary>
		/// On Awake we detect our platform and assign text accordingly
		/// </summary>
		protected virtual void Awake()
		{
			#if UNITY_ANDROID || UNITY_IPHONE
                TargetText.text = MobileText;
			#else
			targetText.text = desktopText;
			#endif

			CanvasGroup = this.gameObject.GetComponent<CanvasGroup>();
			StartCoroutine(DisappearCo());
		}

		/// <summary>
		/// A coroutine used to hide the instructions after a while
		/// </summary>
		/// <returns></returns>
		protected virtual IEnumerator DisappearCo()
		{
			yield return MMCoroutine.WaitFor(disappearDelay);
			StartCoroutine(MMFade.FadeCanvasGroup(CanvasGroup, disappearDuration, 0f, true));
			yield return  MMCoroutine.WaitFor(disappearDuration + 0.1f);
			this.gameObject.SetActive(false);
		}
	}
}