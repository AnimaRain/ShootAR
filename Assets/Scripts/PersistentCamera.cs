using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentCamera : MonoBehaviour
{
	/* The canvas containing the raw image; it must be assigned through
	 * the inspector. We only need the image but the image is required to be
	 * child of the canvas and only the root object can be instructed to not
	 * get destroyed when the scene changes. That's why we get the canvas
	 * instead of the image. */
	[SerializeField] private Canvas backgroundCanvas;
	private WebCamTexture cameraCapture;

	private void Awake() {
		cameraCapture = new WebCamTexture();

		/* The object with this script and the canvas must be protected
		 * to survive the new scene being loaded. */
		DontDestroyOnLoad(backgroundCanvas.gameObject);
		DontDestroyOnLoad(gameObject);

		/* The image required is extracted from the canvas container. */
		UnityEngine.UI.RawImage background =
			backgroundCanvas.GetComponentInChildren<UnityEngine.UI.RawImage>();

		/* The camera capture freezes when a new scene is loaded.
		 * Calling Play after the scene is loaded seems to fix that.
		 * sceneLoaded is called before Start. */
		SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => {
			cameraCapture.Play();
		};
		background.texture = cameraCapture;
	}

	/* Create a button that loads the new scene when pressed. */
	private void OnGUI() {
		if (GUI.Button(new Rect(10, 10, 20, 20), "O"))
			SceneManager.LoadScene(1);
	}
}
