using UnityEngine;

public class FitScript : MonoBehaviour
{
    public Camera cam;
    public Transform planeObject;
    void Start()
    {
        var scale = planeObject.localScale;
        scale.x = scale.y * cam.aspect;
        planeObject.localScale = scale/2.7f;
    }
}