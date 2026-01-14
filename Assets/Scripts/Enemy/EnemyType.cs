using UnityEngine;

/// <summary>
/// 적 타입 정의
/// </summary>
public enum EnemyVariant
{
    Normal,    // 일반 - 균형잡힌 스탯
    Fast,      // 빠름 - 낮은 체력, 빠른 이동속도
    Tank       // 탱커 - 높은 체력, 느린 이동속도
}

/// <summary>
/// 적의 타입을 관리하는 컴포넌트
/// </summary>
public class EnemyType : MonoBehaviour
{
    [Header("적 타입")]
    [SerializeField] private EnemyVariant variant = EnemyVariant.Normal;

    public EnemyVariant Variant => variant;

    void Start()
    {
        ApplyVariantStats();
        ApplyVariantColor();
    }

    /// <summary>
    /// 타입에 따른 스탯 적용
    /// </summary>
    void ApplyVariantStats()
    {
        EnemyStats stats = GetComponent<EnemyStats>();
        EnemyAI ai = GetComponent<EnemyAI>();

        if (stats == null || ai == null) return;

        switch (variant)
        {
            case EnemyVariant.Normal:
                // 기본 스탯 유지
                break;

            case EnemyVariant.Fast:
                // 빠른 적: 체력 50%, 이동속도 150%, 공격력 80%
                stats.SetMaxHealth(stats.MaxHealth * 0.5f);
                stats.SetAttackPower(stats.AttackPower * 0.8f);
                stats.SetGoldReward(Mathf.RoundToInt(stats.GoldReward * 1.2f)); // 골드 20% 더
                ai.SetMoveSpeed(ai.MoveSpeed * 1.5f);
                break;

            case EnemyVariant.Tank:
                // 탱커: 체력 200%, 이동속도 60%, 공격력 120%
                stats.SetMaxHealth(stats.MaxHealth * 2f);
                stats.SetAttackPower(stats.AttackPower * 1.2f);
                stats.SetGoldReward(Mathf.RoundToInt(stats.GoldReward * 2f)); // 골드 2배
                ai.SetMoveSpeed(ai.MoveSpeed * 0.6f);
                break;
        }
    }

    /// <summary>
    /// 타입에 따른 색상 적용
    /// </summary>
    void ApplyVariantColor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;

        switch (variant)
        {
            case EnemyVariant.Normal:
                spriteRenderer.color = Color.white; // 흰색
                break;

            case EnemyVariant.Fast:
                spriteRenderer.color = Color.yellow; // 노란색
                break;

            case EnemyVariant.Tank:
                spriteRenderer.color = Color.red; // 빨간색
                break;
        }
    }

    /// <summary>
    /// 타입 설정 (외부에서 호출)
    /// </summary>
    public void SetVariant(EnemyVariant newVariant)
    {
        variant = newVariant;
        ApplyVariantStats();
        ApplyVariantColor();
    }
}
