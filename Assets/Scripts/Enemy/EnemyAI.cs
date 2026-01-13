using UnityEngine;

/// <summary>
/// 적의 AI - 플레이어를 추적합니다
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float chaseRange = 10f; // 추적 시작 거리
    [SerializeField] private float attackRange = 1.5f; // 공격 범위

    private Transform player;
    private EnemyStats enemyStats;

    void Start()
    {
        // 플레이어 찾기
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        enemyStats = GetComponent<EnemyStats>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 추적 범위 안에 있으면 플레이어에게 이동
        if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange)
        {
            ChasePlayer();
        }
        // 공격 범위 안에 있으면 멈춤 (나중에 공격 추가)
        else if (distanceToPlayer <= attackRange)
        {
            // 공격 로직은 나중에 추가
        }
    }

    /// <summary>
    /// 플레이어 추적
    /// </summary>
    void ChasePlayer()
    {
        // 플레이어 방향으로 이동
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    // 추적 범위를 Scene 창에서 볼 수 있게 그려줍니다
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
