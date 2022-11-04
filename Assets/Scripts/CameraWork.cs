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

    public GameObject player;
    private Vector3 offset;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;

        float height = Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        backgroundLeftEnd = backgroundWidth.bounds.min.x + width;
        backgroundRightEnd = backgroundWidth.bounds.max.x - width;
        backgroundTopEnd = backgroundHeight.bounds.max.y - height;
        backgroundBottomEnd = backgroundHeight.bounds.min.y + height;
    }

    private void Update()
    {
        transform.position = player.transform.position + offset;
        
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, backgroundLeftEnd, backgroundRightEnd),
            Mathf.Clamp(transform.position.y, backgroundBottomEnd, backgroundTopEnd),
            transform.position.z);
        
    }
}
