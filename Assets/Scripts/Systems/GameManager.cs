using UnityEngine;
using System;

/// <summary>
/// 게임 전체를 관리하는 매니저 (싱글톤)
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("재화")]
    [SerializeField] private int gold = 0;

    [Header("스테이지")]
    [SerializeField] private int currentStage = 1;

    [Header("오프라인 보상")]
    [SerializeField] private int offlineGoldPerHour = 100; // 시간당 골드
    [SerializeField] private int maxOfflineHours = 12; // 최대 12시간까지만 보상

    // 이벤트 - UI 업데이트용
    public event Action<int> OnGoldChanged;
    public event Action<int> OnStageChanged;

    public int Gold => gold;
    public int CurrentStage => currentStage;

    private const string LAST_PLAY_TIME_KEY = "LastPlayTime";

    void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CheckOfflineReward();
    }

    void OnApplicationQuit()
    {
        SaveLastPlayTime();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        // 모바일에서 앱이 백그라운드로 갈 때
        if (pauseStatus)
        {
            SaveLastPlayTime();
        }
    }

    /// <summary>
    /// 골드 추가
    /// </summary>
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"골드 획득: +{amount} (총: {gold})");
        OnGoldChanged?.Invoke(gold);
    }

    /// <summary>
    /// 골드 사용 (구매, 업그레이드 등)
    /// </summary>
    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            Debug.Log($"골드 사용: -{amount} (남은 골드: {gold})");
            OnGoldChanged?.Invoke(gold);
            return true;
        }
        else
        {
            Debug.Log("골드가 부족합니다!");
            return false;
        }
    }

    /// <summary>
    /// 다음 스테이지로 이동
    /// </summary>
    public void NextStage()
    {
        currentStage++;
        Debug.Log($"스테이지 {currentStage}로 이동!");
        OnStageChanged?.Invoke(currentStage);
    }

    /// <summary>
    /// 스테이지 클리어 보상
    /// </summary>
    public void GiveStageClearReward()
    {
        int baseReward = 50;
        int stageBonus = currentStage * 10;
        int totalReward = baseReward + stageBonus;

        AddGold(totalReward);
        Debug.Log($"스테이지 {currentStage} 클리어! 보상: {totalReward} 골드");
    }

    /// <summary>
    /// 마지막 플레이 시간 저장
    /// </summary>
    void SaveLastPlayTime()
    {
        string currentTime = System.DateTime.Now.ToBinary().ToString();
        PlayerPrefs.SetString(LAST_PLAY_TIME_KEY, currentTime);
        PlayerPrefs.Save();
        Debug.Log("게임 종료 시간 저장됨");
    }

    /// <summary>
    /// 오프라인 보상 확인 및 지급
    /// </summary>
    void CheckOfflineReward()
    {
        if (!PlayerPrefs.HasKey(LAST_PLAY_TIME_KEY))
        {
            // 첫 실행
            Debug.Log("첫 실행입니다. 오프라인 보상 없음");
            SaveLastPlayTime();
            return;
        }

        // 저장된 시간 불러오기
        string lastTimeString = PlayerPrefs.GetString(LAST_PLAY_TIME_KEY);
        long lastTimeBinary = long.Parse(lastTimeString);
        System.DateTime lastPlayTime = System.DateTime.FromBinary(lastTimeBinary);

        // 현재 시간과 비교
        System.DateTime currentTime = System.DateTime.Now;
        System.TimeSpan offlineTime = currentTime - lastPlayTime;

        // 오프라인 시간이 1분 이상일 때만 보상 (테스트용으로 짧게 설정)
        if (offlineTime.TotalMinutes >= 1)
        {
            // 시간 계산 (최대 12시간)
            float offlineHours = (float)offlineTime.TotalHours;
            if (offlineHours > maxOfflineHours)
            {
                offlineHours = maxOfflineHours;
            }

            // 보상 계산
            int reward = Mathf.RoundToInt(offlineGoldPerHour * offlineHours);

            if (reward > 0)
            {
                AddGold(reward);
                Debug.Log($"=== 오프라인 보상 ===");
                Debug.Log($"오프라인 시간: {offlineHours:F1}시간");
                Debug.Log($"획득 골드: {reward}");
                Debug.Log($"==================");
            }
        }

        // 현재 시간 저장
        SaveLastPlayTime();
    }
}
