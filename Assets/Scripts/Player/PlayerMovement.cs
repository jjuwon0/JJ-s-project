using UnityEngine;

/// <summary>
/// 플레이어의 좌우 이동을 담당하는 스크립트
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5f; // 이동 속도

    private float moveInput; // 입력 값 (-1, 0, 1)

    void Update()
    {
        // A/D 키 또는 화살표 키로 입력 받기
        moveInput = Input.GetAxisRaw("Horizontal");

        // 실제 이동 처리
        transform.position += new Vector3(moveInput * moveSpeed * Time.deltaTime, 0f, 0f);
    }
}
