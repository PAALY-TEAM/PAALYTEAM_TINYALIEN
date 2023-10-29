using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Feel.FeelDemos._Common.Scripts
{
	/// <summary>
	/// This component checks whether the user pressed Enter and plays the associated feedback if that's the case
	/// </summary>
	public class FeelDemosNextDemoButtonInput : MonoBehaviour
	{
		[FormerlySerializedAs("OnInputFeedback")] public MMFeedbacks onInputFeedback;

		protected virtual void Update()
		{
			if (FeelDemosInputHelper.CheckEnterPressedThisFrame())
			{
				onInputFeedback?.PlayFeedbacks();
			}
		}
	}	
}