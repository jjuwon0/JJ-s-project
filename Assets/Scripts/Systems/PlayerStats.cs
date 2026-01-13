using UnityEngine;

/// <summary>
/// 플레이어의 스탯을 관리하는 스크립트
/// </summary>
public class PlayerStats : MonoBehaviour
{
    [Header("기본 스탯")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float attackPower = 10f;
    [SerializeField] private float defense = 5f;

    // 프로퍼티 (다른 스크립트에서 읽기 전용으로 접근)
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public float AttackPower => attackPower;
    public float Defense => defense;

    void Start()
    {
        // 시작할 때 체력을 최대로 설정
        currentHealth = maxHealth;
    }

    /// <summary>
    /// 데미지를 받는 함수
    /// </summary>
    public void TakeDamage(float damage)
    {
        // 방어력을 적용한 실제 데미지 계산
        float actualDamage = Mathf.Max(damage - defense, 0);
        currentHealth -= actualDamage;

        Debug.Log($"플레이어가 {actualDamage} 데미지를 받았습니다. 남은 체력: {currentHealth}");

        // 체력이 0 이하면 사망
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 체력 회복
    /// </summary>
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"플레이어가 {amount} 체력을 회복했습니다. 현재 체력: {currentHealth}");
    }

    /// <summary>
    /// 스탯 업그레이드
    /// </summary>
    public void UpgradeAttack(float amount)
    {
        attackPower += amount;
        Debug.Log($"공격력 증가! 현재 공격력: {attackPower}");
    }

    public void UpgradeDefense(float amount)
    {
        defense += amount;
        Debug.Log($"방어력 증가! 현재 방어력: {defense}");
    }

    public void UpgradeMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount; // 최대 체력 증가와 함께 현재 체력도 증가
        Debug.Log($"최대 체력 증가! 현재 최대 체력: {maxHealth}");
    }

    /// <summary>
    /// 사망 처리
    /// </summary>
    void Die()
    {
        Debug.Log("플레이어가 사망했습니다!");
        // 나중에 게임 오버 처리 추가
    }
}
