using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

/// <summary>
/// 게임 UI를 자동으로 생성하는 에디터 스크립트
/// </summary>
public class GameUISetup : EditorWindow
{
    [MenuItem("Tools/Setup Game UI")]
    public static void ShowWindow()
    {
        GetWindow<GameUISetup>("UI Setup");
    }

    void OnGUI()
    {
        GUILayout.Label("게임 UI 자동 생성", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (GUILayout.Button("게임 UI 생성", GUILayout.Height(40)))
        {
            CreateGameUI();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("업그레이드 UI 생성", GUILayout.Height(40)))
        {
            CreateUpgradeUI();
        }

        GUILayout.Space(10);

        EditorGUILayout.HelpBox("버튼을 클릭하면 자동으로 UI가 생성됩니다.\n이미 UI가 있다면 덮어씌워집니다.", MessageType.Info);
    }

    static void CreateGameUI()
    {
        // Canvas 찾기 또는 생성
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // 기존 GameUI 오브젝트 삭제
        Transform existingUI = canvas.transform.Find("GameUI");
        if (existingUI != null)
        {
            DestroyImmediate(existingUI.gameObject);
        }

        // GameUI 오브젝트 생성
        GameObject gameUIObj = new GameObject("GameUI");
        gameUIObj.transform.SetParent(canvas.transform, false);
        GameUI gameUI = gameUIObj.AddComponent<GameUI>();

        // 상단 정보 패널
        GameObject topPanel = CreatePanel(gameUIObj.transform, "TopPanel", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, 0), new Vector2(400, 100));
        topPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);

        // 골드 텍스트
        GameObject goldTextObj = CreateText(topPanel.transform, "GoldText", "Gold: 0", 24, TextAlignmentOptions.Left);
        RectTransform goldRect = goldTextObj.GetComponent<RectTransform>();
        goldRect.anchorMin = new Vector2(0, 0.5f);
        goldRect.anchorMax = new Vector2(0, 0.5f);
        goldRect.anchoredPosition = new Vector2(20, 20);

        // 스테이지 텍스트
        GameObject stageTextObj = CreateText(topPanel.transform, "StageText", "Stage 1", 24, TextAlignmentOptions.Left);
        RectTransform stageRect = stageTextObj.GetComponent<RectTransform>();
        stageRect.anchorMin = new Vector2(0, 0.5f);
        stageRect.anchorMax = new Vector2(0, 0.5f);
        stageRect.anchoredPosition = new Vector2(20, -20);

        // 자동 전투 텍스트
        GameObject autoTextObj = CreateText(topPanel.transform, "AutoAttackText", "[A] Auto: OFF", 20, TextAlignmentOptions.Right);
        RectTransform autoRect = autoTextObj.GetComponent<RectTransform>();
        autoRect.anchorMin = new Vector2(1, 0.5f);
        autoRect.anchorMax = new Vector2(1, 0.5f);
        autoRect.anchoredPosition = new Vector2(-20, 0);
        autoTextObj.GetComponent<TextMeshProUGUI>().color = Color.gray;

        // 체력바 패널
        GameObject healthPanel = CreatePanel(gameUIObj.transform, "HealthPanel", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -110), new Vector2(300, 40));
        healthPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);

        // 체력바
        GameObject healthBarBG = CreatePanel(healthPanel.transform, "HealthBarBG", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 0), new Vector2(280, 20));
        healthBarBG.GetComponent<Image>().color = Color.red;

        GameObject healthBarFill = CreatePanel(healthBarBG.transform, "Fill", new Vector2(0, 0.5f), new Vector2(1, 0.5f), new Vector2(0, 0), new Vector2(0, 20));
        healthBarFill.GetComponent<Image>().color = Color.green;

        Slider healthSlider = healthBarBG.AddComponent<Slider>();
        healthSlider.fillRect = healthBarFill.GetComponent<RectTransform>();
        healthSlider.value = 1;
        healthSlider.interactable = false;

        // 체력 텍스트
        GameObject healthTextObj = CreateText(healthPanel.transform, "HealthText", "100 / 100", 18, TextAlignmentOptions.Center);
        RectTransform healthTextRect = healthTextObj.GetComponent<RectTransform>();
        healthTextRect.anchoredPosition = new Vector2(0, -25);

        // 스킬 쿨타임 UI
        GameObject skillPanel = CreatePanel(gameUIObj.transform, "SkillPanel", new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0, 100), new Vector2(80, 80));
        skillPanel.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

        GameObject skillCooldownImage = CreatePanel(skillPanel.transform, "CooldownImage", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 0), new Vector2(70, 70));
        Image cooldownImg = skillCooldownImage.GetComponent<Image>();
        cooldownImg.type = Image.Type.Filled;
        cooldownImg.fillMethod = Image.FillMethod.Radial360;
        cooldownImg.color = new Color(0, 0, 0, 0.7f);

        GameObject skillTextObj = CreateText(skillPanel.transform, "SkillText", "Q", 32, TextAlignmentOptions.Center);

        // GameUI 컴포넌트 연결
        SerializedObject serializedGameUI = new SerializedObject(gameUI);
        serializedGameUI.FindProperty("healthBar").objectReferenceValue = healthSlider;
        serializedGameUI.FindProperty("healthText").objectReferenceValue = healthTextObj.GetComponent<TextMeshProUGUI>();
        serializedGameUI.FindProperty("goldText").objectReferenceValue = goldTextObj.GetComponent<TextMeshProUGUI>();
        serializedGameUI.FindProperty("stageText").objectReferenceValue = stageTextObj.GetComponent<TextMeshProUGUI>();
        serializedGameUI.FindProperty("skillCooldownImage").objectReferenceValue = cooldownImg;
        serializedGameUI.FindProperty("skillCooldownText").objectReferenceValue = skillTextObj.GetComponent<TextMeshProUGUI>();
        serializedGameUI.FindProperty("autoAttackText").objectReferenceValue = autoTextObj.GetComponent<TextMeshProUGUI>();
        serializedGameUI.ApplyModifiedProperties();

        Debug.Log("게임 UI 생성 완료!");
        EditorUtility.DisplayDialog("완료", "게임 UI가 성공적으로 생성되었습니다!", "확인");
    }

    static void CreateUpgradeUI()
    {
        // Canvas 찾기
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("오류", "먼저 게임 UI를 생성해주세요!", "확인");
            return;
        }

        // 기존 UpgradeUI 오브젝트 삭제
        Transform existingUI = canvas.transform.Find("UpgradeUI");
        if (existingUI != null)
        {
            DestroyImmediate(existingUI.gameObject);
        }

        // UpgradeUI 오브젝트 생성
        GameObject upgradeUIObj = new GameObject("UpgradeUI");
        upgradeUIObj.transform.SetParent(canvas.transform, false);
        UpgradeUI upgradeUI = upgradeUIObj.AddComponent<UpgradeUI>();

        // 업그레이드 패널 (처음엔 비활성)
        GameObject upgradePanel = CreatePanel(upgradeUIObj.transform, "UpgradePanel", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 0), new Vector2(500, 400));
        upgradePanel.GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 0.95f);

        // 제목
        GameObject titleObj = CreateText(upgradePanel.transform, "Title", "업그레이드 (U키로 열기/닫기)", 28, TextAlignmentOptions.Center);
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchoredPosition = new Vector2(0, 150);

        // 공격력 업그레이드
        CreateUpgradeButton(upgradePanel.transform, "AttackUpgrade", "공격력 업그레이드", new Vector2(0, 60), out GameObject atkBtn, out GameObject atkCost, out GameObject atkLevel);

        // 방어력 업그레이드
        CreateUpgradeButton(upgradePanel.transform, "DefenseUpgrade", "방어력 업그레이드", new Vector2(0, -20), out GameObject defBtn, out GameObject defCost, out GameObject defLevel);

        // 체력 업그레이드
        CreateUpgradeButton(upgradePanel.transform, "HealthUpgrade", "체력 업그레이드", new Vector2(0, -100), out GameObject hpBtn, out GameObject hpCost, out GameObject hpLevel);

        // UpgradeUI 컴포넌트 연결
        SerializedObject serializedUpgradeUI = new SerializedObject(upgradeUI);
        serializedUpgradeUI.FindProperty("upgradePanel").objectReferenceValue = upgradePanel;
        serializedUpgradeUI.FindProperty("attackUpgradeButton").objectReferenceValue = atkBtn.GetComponent<Button>();
        serializedUpgradeUI.FindProperty("defenseUpgradeButton").objectReferenceValue = defBtn.GetComponent<Button>();
        serializedUpgradeUI.FindProperty("healthUpgradeButton").objectReferenceValue = hpBtn.GetComponent<Button>();
        serializedUpgradeUI.FindProperty("attackCostText").objectReferenceValue = atkCost.GetComponent<TextMeshProUGUI>();
        serializedUpgradeUI.FindProperty("defenseCostText").objectReferenceValue = defCost.GetComponent<TextMeshProUGUI>();
        serializedUpgradeUI.FindProperty("healthCostText").objectReferenceValue = hpCost.GetComponent<TextMeshProUGUI>();
        serializedUpgradeUI.FindProperty("attackLevelText").objectReferenceValue = atkLevel.GetComponent<TextMeshProUGUI>();
        serializedUpgradeUI.FindProperty("defenseLevelText").objectReferenceValue = defLevel.GetComponent<TextMeshProUGUI>();
        serializedUpgradeUI.FindProperty("healthLevelText").objectReferenceValue = hpLevel.GetComponent<TextMeshProUGUI>();
        serializedUpgradeUI.ApplyModifiedProperties();

        upgradePanel.SetActive(false);

        Debug.Log("업그레이드 UI 생성 완료!");
        EditorUtility.DisplayDialog("완료", "업그레이드 UI가 성공적으로 생성되었습니다!", "확인");
    }

    static GameObject CreatePanel(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);
        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = sizeDelta;
        panel.AddComponent<Image>();
        return panel;
    }

    static GameObject CreateText(Transform parent, string name, string text, int fontSize, TextAlignmentOptions alignment)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);
        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 50);
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.alignment = alignment;
        tmp.color = Color.white;
        return textObj;
    }

    static void CreateUpgradeButton(Transform parent, string name, string label, Vector2 position, out GameObject button, out GameObject costText, out GameObject levelText)
    {
        // 버튼
        button = new GameObject(name);
        button.transform.SetParent(parent, false);
        RectTransform btnRect = button.AddComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(0.5f, 0.5f);
        btnRect.anchorMax = new Vector2(0.5f, 0.5f);
        btnRect.anchoredPosition = position;
        btnRect.sizeDelta = new Vector2(400, 60);
        Image btnImage = button.AddComponent<Image>();
        btnImage.color = new Color(0.2f, 0.4f, 0.6f, 1f);
        button.AddComponent<Button>();

        // 레이블
        GameObject labelObj = CreateText(button.transform, "Label", label, 22, TextAlignmentOptions.Left);
        RectTransform labelRect = labelObj.GetComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0, 0.5f);
        labelRect.anchorMax = new Vector2(0, 0.5f);
        labelRect.anchoredPosition = new Vector2(20, 10);

        // 비용
        costText = CreateText(button.transform, "CostText", "Cost: 50G", 18, TextAlignmentOptions.Right);
        RectTransform costRect = costText.GetComponent<RectTransform>();
        costRect.anchorMin = new Vector2(1, 0.5f);
        costRect.anchorMax = new Vector2(1, 0.5f);
        costRect.anchoredPosition = new Vector2(-20, 10);

        // 레벨
        levelText = CreateText(button.transform, "LevelText", "Lv.0", 18, TextAlignmentOptions.Left);
        RectTransform levelRect = levelText.GetComponent<RectTransform>();
        levelRect.anchorMin = new Vector2(0, 0.5f);
        levelRect.anchorMax = new Vector2(0, 0.5f);
        levelRect.anchoredPosition = new Vector2(20, -15);
    }
}
