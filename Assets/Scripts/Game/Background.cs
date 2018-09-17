using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 649    //the unassigned fields are actually assigned in Unity Editor

namespace ShootAR
{
	public class Background : MonoBehaviour
	{
		[HideInInspector] public WebCamTexture Cam { get; set; }   //Rear Camera
		[SerializeField] private RawImage backgroundTexture;

		private Text buttonText;

		public void Start()
		{
#if UNITY_ANDROID
			//Set up the rear camera
			for (int i = 0; i < WebCamTexture.devices.Length; i++)
			{
				if (!WebCamTexture.devices[i].isFrontFacing)
				{
					Cam = new WebCamTexture(WebCamTexture.devices[i].name, Screen.width, Screen.height);
					break;
				}
			}
#elif UNITY_EDITOR_WIN
		Cam = new WebCamTexture();
#endif

			//If we did not find a back camera,exit
			if (Cam == null)
			{
				const string error = "This device does not have a rear camera";
				buttonText.text = error + "\n\nTap to exit";
				Debug.LogError(error);
				throw new System.Exception(error);
			}

			Cam.Play();
			backgroundTexture.texture = Cam;
			backgroundTexture.rectTransform.localEulerAngles = new Vector3(0, 0, Cam.videoRotationAngle);
			float scaleY = Cam.videoVerticallyMirrored ? -1.0f : 1.0f;
			backgroundTexture.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
		}
	}
}