using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    private float backgroundLeftEnd;
    private float backgroundRightEnd;
    private float backgroundTopEnd;
    private float backgroundBottomEnd;

    public Collider2D backgroundHeight;
    public Collider2D backgroundWidth;

    private float offset = 1.0f;
    private float cameraSpeed = 5.0f;

    private void Start()
    {
        float height = Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        backgroundLeftEnd = backgroundWidth.bounds.min.x + width;
        backgroundRightEnd = backgroundWidth.bounds.max.x - width;
        backgroundTopEnd = backgroundHeight.bounds.max.y - height;
        backgroundBottomEnd = backgroundHeight.bounds.min.y + height;
    }

    private void LateUpdate()
    {
        Vector3 target = new Vector3(
            Mathf.Clamp(Player.instance.transform.position.x, backgroundLeftEnd, backgroundRightEnd),
            Mathf.Clamp(Player.instance.transform.position.y + offset, backgroundBottomEnd, backgroundTopEnd),
            transform.position.z);

        transform.position = Vector3.Lerp(transform.position,
            target, cameraSpeed * Time.deltaTime);
    }
}
