using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    private Vector2 startPosition;
    private float startZ;

    // => meaning update per frame
    private Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startPosition;

    private float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;
    // if object is in front of target, use near clip pane, else use far clip pane
    private float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
    // The further the object from the player, the faster the parrallax effect will move, which base on it's z 
    private float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;

    private void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
    }   

    private void Update()
    {
        Vector2 newPosition = startPosition + camMoveSinceStart * parallaxFactor;
        transform.position = new Vector3(newPosition.x, newPosition.y, startZ);
    }
}
