using UnityEngine;

/// <summary>
/// 적의 공격 기능
/// </summary>
public class EnemyCombat : MonoBehaviour
{
    [Header("공격 설정")]
    [SerializeField] private float attackCooldown = 2f; // 공격 쿨타임
    [SerializeField] private float attackRange = 1.5f; // 공격 범위

    private EnemyStats enemyStats;
    private float lastAttackTime = 0f;
    private Transform player;

    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();

        // 플레이어 찾기
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 공격 범위 안에 있으면 공격 시도
        if (distanceToPlayer <= attackRange)
        {
            TryAttack();
        }
    }

    /// <summary>
    /// 공격 시도
    /// </summary>
    void TryAttack()
    {
        // 쿨타임 확인
        if (Time.time < lastAttackTime + attackCooldown)
        {
            return;
        }

        Attack();
        lastAttackTime = Time.time;
    }

    /// <summary>
    /// 실제 공격 실행
    /// </summary>
    void Attack()
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(enemyStats.AttackPower);
            Debug.Log($"적이 플레이어를 공격했습니다! 데미지: {enemyStats.AttackPower}");
        }
    }
}
