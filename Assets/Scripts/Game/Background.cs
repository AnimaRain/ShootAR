using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
	[HideInInspector] public WebCamTexture cam;   //Rear Camera
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
				cam = new WebCamTexture(WebCamTexture.devices[i].name, Screen.width, Screen.height);
				break;
			}
		}
#elif UNITY_EDITOR_WIN
		Cam = new WebCamTexture();
#endif

		//If we did not find a back camera,exit
		if (cam == null)
		{
			const string error = "This device does not have a rear camera";
			buttonText.text = error + "\n\nTap to exit";
			Debug.LogError(error);
			throw new System.Exception(error);
		}

		cam.Play();
		backgroundTexture.texture = cam;
		backgroundTexture.rectTransform.localEulerAngles = new Vector3(0, 0, cam.videoRotationAngle);
		float scaleY =cam.videoVerticallyMirrored ? -1.0f : 1.0f;
		backgroundTexture.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
	}
}
