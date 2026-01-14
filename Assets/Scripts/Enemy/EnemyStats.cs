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
    public float MaxHealth => maxHealth;
    public int GoldReward => goldReward;

    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 최대 체력 설정 (타입 적용용)
    /// </summary>
    public void SetMaxHealth(float health)
    {
        maxHealth = health;
        currentHealth = maxHealth; // 현재 체력도 새로운 최대치로
    }

    /// <summary>
    /// 공격력 설정 (타입 적용용)
    /// </summary>
    public void SetAttackPower(float power)
    {
        attackPower = power;
    }

    /// <summary>
    /// 골드 보상 설정 (타입 적용용)
    /// </summary>
    public void SetGoldReward(int gold)
    {
        goldReward = gold;
    }

    /// <summary>
    /// 데미지를 받는 함수
    /// </summary>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"적이 {damage} 데미지를 받았습니다. 남은 체력: {currentHealth}");

        // Hit 애니메이션 트리거
        if (animator != null && currentHealth > 0)
        {
            animator.SetTrigger("hit");
        }

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

        // 골드 지급
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddGold(goldReward);
        }

        // Death 애니메이션 트리거
        if (animator != null)
        {
            animator.SetTrigger("death");
        }

        // Death 애니메이션 재생 후 삭제 (애니메이션 길이만큼 대기)
        Destroy(gameObject, 1f); // 1초 후 삭제 (애니메이션 길이에 맞게 조정)
    }
}
