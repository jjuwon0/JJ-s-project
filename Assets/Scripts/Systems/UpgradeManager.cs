using UnityEngine;

/// <summary>
/// 업그레이드 시스템을 관리하는 매니저
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    [Header("업그레이드 비용")]
    [SerializeField] private int attackUpgradeCost = 50;
    [SerializeField] private int defenseUpgradeCost = 50;
    [SerializeField] private int healthUpgradeCost = 50;

    [Header("업그레이드 수치")]
    [SerializeField] private float attackUpgradeAmount = 5f;
    [SerializeField] private float defenseUpgradeAmount = 3f;
    [SerializeField] private float healthUpgradeAmount = 20f;

    [Header("비용 증가율")]
    [SerializeField] private float costIncreaseRate = 1.2f; // 업그레이드마다 20% 증가

    private PlayerStats playerStats;
    private int attackUpgradeLevel = 0;
    private int defenseUpgradeLevel = 0;
    private int healthUpgradeLevel = 0;

    public int AttackUpgradeCost => attackUpgradeCost;
    public int DefenseUpgradeCost => defenseUpgradeCost;
    public int HealthUpgradeCost => healthUpgradeCost;

    public int AttackUpgradeLevel => attackUpgradeLevel;
    public int DefenseUpgradeLevel => defenseUpgradeLevel;
    public int HealthUpgradeLevel => healthUpgradeLevel;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
        }
    }

    /// <summary>
    /// 공격력 업그레이드
    /// </summary>
    public bool UpgradeAttack()
    {
        if (GameManager.Instance.SpendGold(attackUpgradeCost))
        {
            playerStats.UpgradeAttack(attackUpgradeAmount);
            attackUpgradeLevel++;
            attackUpgradeCost = Mathf.RoundToInt(attackUpgradeCost * costIncreaseRate);
            Debug.Log($"공격력 업그레이드 완료! 레벨: {attackUpgradeLevel}, 다음 비용: {attackUpgradeCost}");
            return true;
        }
        return false;
    }

    /// <summary>
    /// 방어력 업그레이드
    /// </summary>
    public bool UpgradeDefense()
    {
        if (GameManager.Instance.SpendGold(defenseUpgradeCost))
        {
            playerStats.UpgradeDefense(defenseUpgradeAmount);
            defenseUpgradeLevel++;
            defenseUpgradeCost = Mathf.RoundToInt(defenseUpgradeCost * costIncreaseRate);
            Debug.Log($"방어력 업그레이드 완료! 레벨: {defenseUpgradeLevel}, 다음 비용: {defenseUpgradeCost}");
            return true;
        }
        return false;
    }

    /// <summary>
    /// 체력 업그레이드
    /// </summary>
    public bool UpgradeHealth()
    {
        if (GameManager.Instance.SpendGold(healthUpgradeCost))
        {
            playerStats.UpgradeMaxHealth(healthUpgradeAmount);
            healthUpgradeLevel++;
            healthUpgradeCost = Mathf.RoundToInt(healthUpgradeCost * costIncreaseRate);
            Debug.Log($"체력 업그레이드 완료! 레벨: {healthUpgradeLevel}, 다음 비용: {healthUpgradeCost}");
            return true;
        }
        return false;
    }
}
