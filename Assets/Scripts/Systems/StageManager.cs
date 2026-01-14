using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 스테이지 진행을 관리하는 매니저
/// </summary>
public class StageManager : MonoBehaviour
{
    [Header("적 스폰 설정")]
    [SerializeField] private GameObject enemyPrefab; // 적 프리팹
    [SerializeField] private Transform[] spawnPoints; // 적 스폰 위치들
    [SerializeField] private int enemiesPerStage = 20; // 스테이지당 총 적 수
    [SerializeField] private int maxEnemiesAlive = 5; // 동시에 존재할 수 있는 최대 적 수
    [SerializeField] private float spawnInterval = 2f; // 스폰 간격
    [SerializeField] private float spawnRangeX = 15f; // 플레이어 오른쪽 스폰 거리

    [Header("스테이지 설정")]
    [SerializeField] private float stageClearDelay = 2f; // 클리어 후 대기 시간

    private List<GameObject> currentEnemies = new List<GameObject>();
    private int enemiesSpawned = 0;
    private int enemiesToSpawn = 0;
    private bool isSpawning = false;

    void Start()
    {
        StartNewStage();
    }

    void Update()
    {
        // 현재 스테이지의 모든 적이 스폰됐고, 살아있는 적이 없으면 클리어
        if (!isSpawning && enemiesSpawned >= enemiesToSpawn && GetAliveEnemyCount() == 0)
        {
            StartCoroutine(StageClear());
        }

        // 죽은 적 리스트에서 제거
        currentEnemies.RemoveAll(enemy => enemy == null);
    }

    /// <summary>
    /// 새 스테이지 시작
    /// </summary>
    void StartNewStage()
    {
        currentEnemies.Clear();
        enemiesSpawned = 0;
        enemiesToSpawn = enemiesPerStage + (GameManager.Instance.CurrentStage - 1) * 2; // 스테이지마다 적 2마리 증가

        Debug.Log($"=== 스테이지 {GameManager.Instance.CurrentStage} 시작 ===");
        Debug.Log($"출현할 적 수: {enemiesToSpawn}");

        StartCoroutine(SpawnEnemies());
    }

    /// <summary>
    /// 적 스폰 코루틴
    /// </summary>
    IEnumerator SpawnEnemies()
    {
        isSpawning = true;

        while (enemiesSpawned < enemiesToSpawn)
        {
            // 동시 존재 제한 확인
            if (GetAliveEnemyCount() < maxEnemiesAlive)
            {
                SpawnEnemy();
                enemiesSpawned++;
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
        Debug.Log("모든 적 스폰 완료!");
    }

    /// <summary>
    /// 적 1마리 스폰
    /// </summary>
    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("적 프리팹이 설정되지 않았습니다!");
            return;
        }

        // 스폰 위치 - 플레이어 오른쪽 x축 +6에 고정
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPos = player != null ? player.transform.position : Vector3.zero;

        // 플레이어 오른쪽 6 유닛 거리에 스폰
        float randomY = Random.Range(-2f, 2f); // 약간의 Y축 변화
        Vector3 spawnPosition = playerPos + new Vector3(6f, randomY, 0);

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemies.Add(enemy);

        // 적 타입 랜덤 설정
        AssignRandomEnemyType(enemy);

        // 스테이지에 따라 적 강화
        ScaleEnemyStats(enemy);
    }

    /// <summary>
    /// 적에게 랜덤 타입 부여
    /// </summary>
    void AssignRandomEnemyType(GameObject enemy)
    {
        EnemyType enemyType = enemy.GetComponent<EnemyType>();
        if (enemyType == null)
        {
            enemyType = enemy.AddComponent<EnemyType>();
        }

        // 랜덤으로 타입 선택 (Normal 40%, Fast 30%, Tank 30%)
        float random = Random.value;
        if (random < 0.4f)
        {
            enemyType.SetVariant(EnemyVariant.Normal);
        }
        else if (random < 0.7f)
        {
            enemyType.SetVariant(EnemyVariant.Fast);
        }
        else
        {
            enemyType.SetVariant(EnemyVariant.Tank);
        }
    }

    /// <summary>
    /// 스테이지에 따라 적 스탯 조정
    /// </summary>
    void ScaleEnemyStats(GameObject enemy)
    {
        EnemyStats stats = enemy.GetComponent<EnemyStats>();
        if (stats != null)
        {
            int currentStage = GameManager.Instance.CurrentStage;
            // 스테이지마다 적 체력과 공격력 10%씩 증가
            float multiplier = 1f + (currentStage - 1) * 0.1f;
            // 이 부분은 EnemyStats에 public setter 추가 필요
            // stats.MultiplyStats(multiplier);
        }
    }

    /// <summary>
    /// 살아있는 적 수 확인
    /// </summary>
    int GetAliveEnemyCount()
    {
        return currentEnemies.Count;
    }

    /// <summary>
    /// 스테이지 클리어 처리
    /// </summary>
    IEnumerator StageClear()
    {
        Debug.Log($"=== 스테이지 {GameManager.Instance.CurrentStage} 클리어! ===");

        // 보상 지급
        GameManager.Instance.GiveStageClearReward();

        yield return new WaitForSeconds(stageClearDelay);

        // 다음 스테이지로
        GameManager.Instance.NextStage();
        StartNewStage();
    }
}
