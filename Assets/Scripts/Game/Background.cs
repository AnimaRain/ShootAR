using UnityEngine;
using UnityEngine.UI;

namespace ShootAR
{
	public class Background : MonoBehaviour
	{
		private WebCamTexture cam;   //Rear Camera
		[SerializeField] private RawImage backgroundTexture;

		[SerializeField] private Text messageToPlayer;

		public void Start()
		{
#if UNITY_ANDROID
			//Set up the rear camera
			for (int i = 0; i < WebCamTexture.devices.Length; i++)
			{
				if (!WebCamTexture.devices[i].isFrontFacing)
				{
					cam = new WebCamTexture(WebCamTexture.devices[i].name, Screen.width, Screen.height);
					break;
				}
			}
#elif UNITY_EDITOR_WIN
		cam = new WebCamTexture();
#endif

			if (cam == null)
			{
				const string error = "This device does not have a rear camera";
				messageToPlayer.text = error + "\n\nTap to exit";
				Debug.LogError(error);
				throw new System.Exception(error);
			}

			cam.Play();
			backgroundTexture.texture = cam;
			backgroundTexture.rectTransform.localEulerAngles = new Vector3(0, 0, cam.videoRotationAngle);
			float scaleY = cam.videoVerticallyMirrored ? -1.0f : 1.0f;
			backgroundTexture.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
		}
	}
}