using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카메라 움직임 구현
public class CameraWork : MonoBehaviour
{
    private float backgroundLeftEnd;    // 배경의 왼쪽 끝 좌표
    private float backgroundRightEnd;   // 배경의 오른쪽 끝 좌표
    private float backgroundTopEnd;     // 배경의 위쪽 끝 좌표
    private float backgroundBottomEnd;  // 배경의 아래쪽 끝 좌표

    public Collider2D backgroundHeight; // 맵 세로 경계 콜라이더
    public Collider2D backgroundWidth;  // 맵 가로 경계 콜라이더

    private float offset = 1.0f;        // y축 오프셋
    private float cameraSpeed = 5.0f;   // 카메라 이동 속도

    private void Start()
    {
        float height = Camera.main.orthographicSize;    // 화면 높이
        float width = height * Camera.main.aspect;      // 화면 너비
        // 카메라가 이동 가능한 경계 산출
        backgroundLeftEnd = backgroundWidth.bounds.min.x + width;
        backgroundRightEnd = backgroundWidth.bounds.max.x - width;
        backgroundTopEnd = backgroundHeight.bounds.max.y - height;
        backgroundBottomEnd = backgroundHeight.bounds.min.y + height;
    }

    private void LateUpdate()
    {
        // 플레이어 위치를 따라가되 배경을 벗어나지 않도록 타겟 지정
        Vector3 target = new Vector3(
            Mathf.Clamp(Player.instance.transform.position.x, backgroundLeftEnd, backgroundRightEnd),
            Mathf.Clamp(Player.instance.transform.position.y + offset, backgroundBottomEnd, backgroundTopEnd),
            transform.position.z);

        // 타겟이 있는 곳으로 서서히 이동
        transform.position = Vector3.Lerp(transform.position,
            target, cameraSpeed * Time.deltaTime);
    }
}
