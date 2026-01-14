using UnityEngine;

/// <summary>
/// 플레이어의 전투(공격) 기능
/// </summary>
public class PlayerCombat : MonoBehaviour
{
    [Header("공격 설정")]
    [SerializeField] private float attackRange = 10f; // 공격 범위
    [SerializeField] private float attackCooldown = 0.5f; // 공격 쿨타임
    [SerializeField] private LayerMask enemyLayer; // 적 레이어 (나중에 설정)

    [Header("자동 전투")]
    [SerializeField] private bool autoAttack = false; // 자동 전투 ON/OFF

    private PlayerStats playerStats;
    private float lastAttackTime = 0f;

    public bool AutoAttack => autoAttack;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        // 수동 공격 - 스페이스바를 누르면 공격
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryAttack();
        }

        // 자동 전투가 켜져 있으면 자동으로 공격
        if (autoAttack)
        {
            TryAttack();
        }

        // T 키로 자동 전투 ON/OFF
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleAutoAttack();
        }
    }

    /// <summary>
    /// 자동 전투 토글
    /// </summary>
    public void ToggleAutoAttack()
    {
        autoAttack = !autoAttack;
        Debug.Log($"자동 전투: {(autoAttack ? "ON" : "OFF")}");
    }

    /// <summary>
    /// 자동 전투 설정
    /// </summary>
    public void SetAutoAttack(bool value)
    {
        autoAttack = value;
        Debug.Log($"자동 전투: {(autoAttack ? "ON" : "OFF")}");
    }

    /// <summary>
    /// 공격 시도
    /// </summary>
    void TryAttack()
    {
        // 쿨타임 확인
        if (Time.time < lastAttackTime + attackCooldown)
        {
            Debug.Log("공격 쿨타임 중입니다.");
            return;
        }

        // 범위 내 가장 가까운 적 찾기
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = attackRange;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        // 적을 찾았으면 공격
        if (closestEnemy != null)
        {
            Attack(closestEnemy);
            lastAttackTime = Time.time;
        }
        else
        {
            Debug.Log("공격 범위 내에 적이 없습니다.");
        }
    }

    /// <summary>
    /// 실제 공격 실행
    /// </summary>
    void Attack(GameObject target)
    {
        EnemyStats enemyStats = target.GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            enemyStats.TakeDamage(playerStats.AttackPower);
            Debug.Log($"적을 공격했습니다! 데미지: {playerStats.AttackPower}");
        }
    }

    // 공격 범위를 Scene 창에서 볼 수 있게 그려줍니다
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
