using UnityEngine;
using System.Collections;

/// <summary>
/// 플레이어 스킬 시스템
/// </summary>
public class PlayerSkill : MonoBehaviour
{
    [Header("스킬 설정")]
    [SerializeField] private float skillDamageMultiplier = 3f; // 기본 공격력의 3배 데미지
    [SerializeField] private float skillRange = 5f; // 스킬 범위
    [SerializeField] private float skillCooldown = 5f; // 스킬 쿨타임 (초)

    private PlayerStats playerStats;
    private float lastSkillTime = -999f; // 게임 시작 시 바로 사용 가능하도록

    public float SkillCooldownRemaining => Mathf.Max(0, skillCooldown - (Time.time - lastSkillTime));
    public bool IsSkillReady => SkillCooldownRemaining <= 0;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        // Q 키로 스킬 사용
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryUseSkill();
        }
    }

    /// <summary>
    /// 스킬 사용 시도
    /// </summary>
    public void TryUseSkill()
    {
        if (!IsSkillReady)
        {
            Debug.Log($"스킬 쿨타임 중입니다. 남은 시간: {SkillCooldownRemaining:F1}초");
            return;
        }

        UseSkill();
    }

    /// <summary>
    /// 스킬 실행 - 범위 내 모든 적에게 강력한 데미지
    /// </summary>
    void UseSkill()
    {
        lastSkillTime = Time.time;

        float skillDamage = playerStats.AttackPower * skillDamageMultiplier;

        // 범위 내 모든 적 찾기
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int hitCount = 0;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= skillRange)
            {
                EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(skillDamage);
                    hitCount++;
                }
            }
        }

        Debug.Log($"[스킬] 광역 공격! {hitCount}마리의 적에게 {skillDamage} 데미지!");

        // 시각적 피드백 (나중에 이펙트로 교체)
        StartCoroutine(ShowSkillEffect());
    }

    /// <summary>
    /// 스킬 이펙트 표시 (임시)
    /// </summary>
    IEnumerator ShowSkillEffect()
    {
        // 나중에 파티클이나 이펙트로 교체
        yield return new WaitForSeconds(0.1f);
    }

    // 스킬 범위를 Scene 창에서 볼 수 있게 그려줍니다
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, skillRange);
    }
}
