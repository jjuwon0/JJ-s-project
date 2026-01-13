using UnityEngine;

/// <summary>
/// 적의 스탯을 관리하는 스크립트
/// </summary>
public class EnemyStats : MonoBehaviour
{
    [Header("적 스탯")]
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float attackPower = 5f;
    [SerializeField] private int goldReward = 10; // 죽으면 주는 골드

    public float AttackPower => attackPower;
    public float CurrentHealth => currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// 데미지를 받는 함수
    /// </summary>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"적이 {damage} 데미지를 받았습니다. 남은 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 적 사망
    /// </summary>
    void Die()
    {
        Debug.Log($"적이 사망했습니다! 골드 {goldReward} 획득");

        // 나중에 골드 시스템 추가 예정
        // GameManager.Instance.AddGold(goldReward);

        Destroy(gameObject); // 적 오브젝트 삭제
    }
}
