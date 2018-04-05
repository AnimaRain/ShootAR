using UnityEngine;

public class FitScript : MonoBehaviour
{
	[SerializeField] private Camera cam;
	[SerializeField] private Transform planeObject;

    private void Start()
    {
        var scale = planeObject.localScale;
        scale.x = scale.y * cam.aspect;
        planeObject.localScale = scale/2.7f;
    }
}