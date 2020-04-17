using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraRotator : MonoBehaviour
{
	private float mouseY;
	private float mouseX;
	/// <summary>
	/// The main camera rotates relative to this container.
	/// </summary>
	private GameObject cameraContainer;
	/// <summary>
	/// A rotation used to calibrate the phone's camera to the game's camera.
	/// </summary>
	private Quaternion calibrationRotation;

	private void Awake() {
		cameraContainer = new GameObject("Camera Container");
	}

	private void Start() {
		cameraContainer.transform.SetPositionAndRotation(transform.position, transform.rotation);
		transform.SetParent(cameraContainer.transform);
		cameraContainer.transform.Rotate(90f, -90f, 0f);
#if UNITY_ANDROID
		calibrationRotation = new Quaternion(0, 0, 1, 0);
#endif
	}

	private void Update() {
#if UNITY_ANDROID //Camera Control on Android

		// Rotate the camera using feedback from the gyroscope. The input is
		// flipped because the Gyroscope is right-handed and Unity is left-handed.
		transform.localRotation = Input.gyro.attitude * calibrationRotation;
#endif

#if UNITY_EDITOR //Camera Control on PC
		if (Input.GetButton("Fire2")) {
			mouseY += 5 * Input.GetAxis("Mouse Y");
			mouseX += 5 * Input.GetAxis("Mouse X");
			Camera.main.transform.eulerAngles = new Vector3(-mouseY, mouseX, 0);
		}
#endif
	}

	/*Debug: Camera rotation
	#if DEBUG
		private void OnGUI()
		{
			GUILayout.Label(
				string.Format(
					"Gyro attitude: {0}\nCamera attitude: {1}\n" +
					"Camera local attitude: {2}",
					Input.gyro.attitude, transform.rotation, transform.localRotation
				)
			);
		}
	#endif*/
}
