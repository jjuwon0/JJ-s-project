using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 게임 내 UI를 관리하는 스크립트
/// </summary>
public class GameUI : MonoBehaviour
{
    [Header("플레이어 정보")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("게임 정보")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI stageText;

    [Header("스킬 쿨타임")]
    [SerializeField] private Image skillCooldownImage;
    [SerializeField] private TextMeshProUGUI skillCooldownText;

    [Header("자동 전투 표시")]
    [SerializeField] private TextMeshProUGUI autoAttackText;

    private PlayerStats playerStats;
    private PlayerSkill playerSkill;
    private PlayerCombat playerCombat;

    void Start()
    {
        // 플레이어 컴포넌트 찾기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
            playerSkill = player.GetComponent<PlayerSkill>();
            playerCombat = player.GetComponent<PlayerCombat>();
        }

        // GameManager 이벤트 구독
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGoldChanged += UpdateGold;
            GameManager.Instance.OnStageChanged += UpdateStage;

            // 초기값 설정
            UpdateGold(GameManager.Instance.Gold);
            UpdateStage(GameManager.Instance.CurrentStage);
        }
    }

    void OnDestroy()
    {
        // 이벤트 구독 해제
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGoldChanged -= UpdateGold;
            GameManager.Instance.OnStageChanged -= UpdateStage;
        }
    }

    void Update()
    {
        UpdateHealthBar();
        UpdateSkillCooldown();
        UpdateAutoAttackStatus();
    }

    /// <summary>
    /// 체력바 업데이트
    /// </summary>
    void UpdateHealthBar()
    {
        if (playerStats == null || healthBar == null) return;

        float healthPercent = playerStats.CurrentHealth / playerStats.MaxHealth;
        healthBar.value = healthPercent;

        if (healthText != null)
        {
            healthText.text = $"{playerStats.CurrentHealth:F0} / {playerStats.MaxHealth:F0}";
        }
    }

    /// <summary>
    /// 골드 표시 업데이트
    /// </summary>
    void UpdateGold(int gold)
    {
        if (goldText != null)
        {
            goldText.text = $"Gold: {gold}";
        }
    }

    /// <summary>
    /// 스테이지 표시 업데이트
    /// </summary>
    void UpdateStage(int stage)
    {
        if (stageText != null)
        {
            stageText.text = $"Stage {stage}";
        }
    }

    /// <summary>
    /// 스킬 쿨타임 표시 업데이트
    /// </summary>
    void UpdateSkillCooldown()
    {
        if (playerSkill == null) return;

        if (skillCooldownImage != null)
        {
            if (playerSkill.IsSkillReady)
            {
                skillCooldownImage.fillAmount = 0;
            }
            else
            {
                float cooldownPercent = playerSkill.SkillCooldownRemaining / 5f; // 쿨타임 5초 기준
                skillCooldownImage.fillAmount = cooldownPercent;
            }
        }

        if (skillCooldownText != null)
        {
            if (playerSkill.IsSkillReady)
            {
                skillCooldownText.text = "Q";
            }
            else
            {
                skillCooldownText.text = $"{playerSkill.SkillCooldownRemaining:F1}";
            }
        }
    }

    /// <summary>
    /// 자동 전투 상태 표시 업데이트
    /// </summary>
    void UpdateAutoAttackStatus()
    {
        if (playerCombat == null || autoAttackText == null) return;

        if (playerCombat.AutoAttack)
        {
            autoAttackText.text = "[T] Auto: ON";
            autoAttackText.color = Color.green;
        }
        else
        {
            autoAttackText.text = "[T] Auto: OFF";
            autoAttackText.color = Color.gray;
        }
    }
}
