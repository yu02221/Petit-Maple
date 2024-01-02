using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ī�޶� ������ ����
public class CameraWork : MonoBehaviour
{
    private float backgroundLeftEnd;    // ����� ���� �� ��ǥ
    private float backgroundRightEnd;   // ����� ������ �� ��ǥ
    private float backgroundTopEnd;     // ����� ���� �� ��ǥ
    private float backgroundBottomEnd;  // ����� �Ʒ��� �� ��ǥ

    public Collider2D backgroundHeight; // �� ���� ��� �ݶ��̴�
    public Collider2D backgroundWidth;  // �� ���� ��� �ݶ��̴�

    private float offset = 1.0f;        // y�� ������
    private float cameraSpeed = 5.0f;   // ī�޶� �̵� �ӵ�

    private void Start()
    {
        float height = Camera.main.orthographicSize;    // ȭ�� ����
        float width = height * Camera.main.aspect;      // ȭ�� �ʺ�
        // ī�޶� �̵� ������ ��� ����
        backgroundLeftEnd = backgroundWidth.bounds.min.x + width;
        backgroundRightEnd = backgroundWidth.bounds.max.x - width;
        backgroundTopEnd = backgroundHeight.bounds.max.y - height;
        backgroundBottomEnd = backgroundHeight.bounds.min.y + height;
    }

    private void LateUpdate()
    {
        // �÷��̾� ��ġ�� ���󰡵� ����� ����� �ʵ��� Ÿ�� ����
        Vector3 target = new Vector3(
            Mathf.Clamp(Player.instance.transform.position.x, backgroundLeftEnd, backgroundRightEnd),
            Mathf.Clamp(Player.instance.transform.position.y + offset, backgroundBottomEnd, backgroundTopEnd),
            transform.position.z);

        // Ÿ���� �ִ� ������ ������ �̵�
        transform.position = Vector3.Lerp(transform.position,
            target, cameraSpeed * Time.deltaTime);
    }
}
