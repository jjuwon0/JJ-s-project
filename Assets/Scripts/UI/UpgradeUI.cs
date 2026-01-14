using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 업그레이드 UI를 관리하는 스크립트
/// </summary>
public class UpgradeUI : MonoBehaviour
{
    [Header("업그레이드 버튼")]
    [SerializeField] private Button attackUpgradeButton;
    [SerializeField] private Button defenseUpgradeButton;
    [SerializeField] private Button healthUpgradeButton;

    [Header("비용 텍스트")]
    [SerializeField] private TextMeshProUGUI attackCostText;
    [SerializeField] private TextMeshProUGUI defenseCostText;
    [SerializeField] private TextMeshProUGUI healthCostText;

    [Header("레벨 텍스트")]
    [SerializeField] private TextMeshProUGUI attackLevelText;
    [SerializeField] private TextMeshProUGUI defenseLevelText;
    [SerializeField] private TextMeshProUGUI healthLevelText;

    [Header("UI 패널")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private KeyCode toggleKey = KeyCode.U; // U 키로 UI 열기/닫기

    private UpgradeManager upgradeManager;

    void Start()
    {
        upgradeManager = FindObjectOfType<UpgradeManager>();

        // 버튼 이벤트 연결
        if (attackUpgradeButton != null)
            attackUpgradeButton.onClick.AddListener(OnAttackUpgradeClicked);

        if (defenseUpgradeButton != null)
            defenseUpgradeButton.onClick.AddListener(OnDefenseUpgradeClicked);

        if (healthUpgradeButton != null)
            healthUpgradeButton.onClick.AddListener(OnHealthUpgradeClicked);

        // 초기 UI 업데이트
        UpdateUI();

        // 처음엔 업그레이드 패널 닫기
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
    }

    void Update()
    {
        // U 키로 업그레이드 UI 열기/닫기
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleUpgradePanel();
        }

        // UI 업데이트
        if (upgradePanel != null && upgradePanel.activeSelf)
        {
            UpdateUI();
        }
    }

    /// <summary>
    /// 업그레이드 패널 열기/닫기
    /// </summary>
    void ToggleUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(!upgradePanel.activeSelf);
        }
    }

    /// <summary>
    /// UI 업데이트
    /// </summary>
    void UpdateUI()
    {
        if (upgradeManager == null) return;

        // 비용 텍스트 업데이트
        if (attackCostText != null)
            attackCostText.text = $"Cost: {upgradeManager.AttackUpgradeCost}G";

        if (defenseCostText != null)
            defenseCostText.text = $"Cost: {upgradeManager.DefenseUpgradeCost}G";

        if (healthCostText != null)
            healthCostText.text = $"Cost: {upgradeManager.HealthUpgradeCost}G";

        // 레벨 텍스트 업데이트
        if (attackLevelText != null)
            attackLevelText.text = $"Lv.{upgradeManager.AttackUpgradeLevel}";

        if (defenseLevelText != null)
            defenseLevelText.text = $"Lv.{upgradeManager.DefenseUpgradeLevel}";

        if (healthLevelText != null)
            healthLevelText.text = $"Lv.{upgradeManager.HealthUpgradeLevel}";

        // 버튼 활성화/비활성화 (골드 부족 시)
        if (GameManager.Instance != null)
        {
            if (attackUpgradeButton != null)
                attackUpgradeButton.interactable = GameManager.Instance.Gold >= upgradeManager.AttackUpgradeCost;

            if (defenseUpgradeButton != null)
                defenseUpgradeButton.interactable = GameManager.Instance.Gold >= upgradeManager.DefenseUpgradeCost;

            if (healthUpgradeButton != null)
                healthUpgradeButton.interactable = GameManager.Instance.Gold >= upgradeManager.HealthUpgradeCost;
        }
    }

    /// <summary>
    /// 공격력 업그레이드 버튼 클릭
    /// </summary>
    void OnAttackUpgradeClicked()
    {
        upgradeManager.UpgradeAttack();
        UpdateUI();
    }

    /// <summary>
    /// 방어력 업그레이드 버튼 클릭
    /// </summary>
    void OnDefenseUpgradeClicked()
    {
        upgradeManager.UpgradeDefense();
        UpdateUI();
    }

    /// <summary>
    /// 체력 업그레이드 버튼 클릭
    /// </summary>
    void OnHealthUpgradeClicked()
    {
        upgradeManager.UpgradeHealth();
        UpdateUI();
    }
}
