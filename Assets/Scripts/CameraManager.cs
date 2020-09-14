using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShootAR
{

    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;
        private float mouseY;
        private float mouseX;
        [HideInInspector] public WebCamTexture cam;
        [SerializeField]
        private RawImage backgroundTexture;
        [SerializeField]
        private UIManager ui;
        /// <summary>
        /// The main camera rotates relative to this container.
        /// </summary>
        private GameObject cameraContainer;
        /// <summary>
        /// A rotation used to calibrate the phone's camera to the game's camera.
        /// </summary>
        private Quaternion calibrationRotation;


        public static CameraManager Create(
        RawImage background = null

        )
        {
            var o = new GameObject(nameof(CameraManager)).AddComponent<CameraManager>();

            o.backgroundTexture = background ?? new GameObject("Background")
                                                        .AddComponent<RawImage>();
            return o;
        }

        private void Awake()
        {
#if UNITY_ANDROID
			if (!SystemInfo.supportsGyroscope) {
				const string error = "This device does not have Gyroscope";
				if (ui != null)
					ui.MessageOnScreen.text = error;
				throw new UnityException(error);
			}
			else {
				Input.gyro.enabled = true;
			}

			//Set up the rear camera
			for (int i = 0;i < WebCamTexture.devices.Length;i++) {
				if (!WebCamTexture.devices[i].isFrontFacing) {
					cam = new WebCamTexture(WebCamTexture.devices[i].name, Screen.width, Screen.height);
					break;
				}
			}
#endif
            /* Do not use elif here. While testing
             * using Unity Remote 5, it does not use
             * the camera on the phone and it has to
             * fall back on the webcam. We need both
             * UNITY_ANDROID and UNITY_EDITOR for that. */
#if UNITY_EDITOR
			cam = new WebCamTexture();
#endif
            cameraContainer = new GameObject("Camera Container");
            if(Instance)
                 DestroyImmediate(cameraContainer);
             else
             {
                DontDestroyOnLoad(cameraContainer);
                Instance = this;
            }
        }

        void Start()
        {

            if (cam == null)
            {
                const string error = "This device does not have a rear camera";
                ui.MessageOnScreen.text = error;
                throw new UnityException(error);
            }

            cameraContainer.transform.SetPositionAndRotation(transform.position, transform.rotation);
            transform.SetParent(cameraContainer.transform);
            cameraContainer.transform.Rotate(90f, -90f, 0f);
#if UNITY_ANDROID
		calibrationRotation = new Quaternion(0, 0, 1, 0);
#endif

            cam.Play();
            backgroundTexture.texture = cam;
            backgroundTexture.rectTransform
                .localEulerAngles = new Vector3(0, 0, cam.videoRotationAngle);
            float scaleY = cam.videoVerticallyMirrored ? -1.0f : 1.0f;
            float videoRatio = (float)cam.width / (float)cam.height;
            backgroundTexture.rectTransform.localScale = new Vector3(scaleY, scaleY / videoRatio, 1);  //Through testing i found out that using these settings makes the most optimal outcome

            /*
    #if UNITY_ANDROID
            backgroundTexture.rectTransform.localScale = new Vector3(Screen.width, Screen.height, 0);    //Didnt try this, but got the inspiration from it
    #endif
            */

        }

        // Update is called once per frame
        private void Update()
        {
#if UNITY_ANDROID //Camera Control on Android

		// Rotate the camera using feedback from the gyroscope. The input is
		// flipped because the Gyroscope is right-handed and Unity is left-handed.
		transform.localRotation = Input.gyro.attitude * calibrationRotation;
#endif

#if UNITY_EDITOR_WIN //Camera Control on PC
		if (Input.GetButton("Fire2")) {
			mouseY += 5 * Input.GetAxis("Mouse Y");
			mouseX += 5 * Input.GetAxis("Mouse X");
			Camera.main.transform.eulerAngles = new Vector3(-mouseY, mouseX, 0);
		}
#endif
        }
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