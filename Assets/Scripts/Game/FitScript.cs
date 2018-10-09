/* TODO: there is some magic happening here. I bet, that 2.7f was eyeballed.
 * Also, Unity could possibly do what this script does, on its own... or not. I
 * don't know. */

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