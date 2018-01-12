using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{

    public RawImage Background;

    private float h;
    private float v;
    private GameObject cameraContainer;     //The main camera rotates relative to this container.
    private GameController gameController;
    private Quaternion calibrationRotation;

    void Awake()
    {
        if (gameController == null)
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        cameraContainer = new GameObject("Camera Container");
    }

    private void Start()
    {
        gameController.Cam.Play();
        Background.texture = gameController.Cam;
        Background.rectTransform.localEulerAngles = new Vector3(0, 0, gameController.Cam.videoRotationAngle);
        float scaleY = gameController.Cam.videoVerticallyMirrored ? -1.0f : 1.0f;
        Background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        cameraContainer.transform.SetPositionAndRotation(transform.position, transform.rotation);
        transform.SetParent(cameraContainer.transform);
        cameraContainer.transform.Rotate(90f, -90f, 0f);
        calibrationRotation = new Quaternion(0, 0, 1, 0);
    }

    void Update()
    {
#if UNITY_ANDROID //Camera Control on Android

        // Rotates the camera using feedback from the gyroscope. It flips the
        // input because the Gyroscope is right-handed and Unity is left-handed.
        transform.localRotation = Input.gyro.attitude * calibrationRotation;
#endif

#if UNITY_EDITOR_WIN //Camera Control on PC
        if (Input.GetButton("Fire2"))
        {
            h += 5 * Input.GetAxis("Mouse Y");
            v += 5 * Input.GetAxis("Mouse X");
            Camera.main.transform.eulerAngles = new Vector3(-h, v, 0);
        }
#endif
    }

    private void OnGUI()
    {
#if DEBUG
        GUILayout.Label(string.Format("Gyro attitude: {0}\nCamera attitude: {1}\nCamera local attitude: {2}", Input.gyro.attitude, transform.rotation, transform.localRotation));
#endif
    }
}