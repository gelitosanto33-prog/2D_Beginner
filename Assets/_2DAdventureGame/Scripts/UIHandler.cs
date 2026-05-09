using UnityEngine;
using UnityEngine.UIElements; // UI Toolkit 기능을 사용하기 위해 필요한 네임스페이스

public class UIHandler : MonoBehaviour
{
    
    private VisualElement m_Healthbar; // UI의 개별 요소(VisualElement)를 담을 변수
    public static UIHandler instance { get; private set; }

    // UI dialogue window variables
    public float displayTime = 4.0f; // 대화창이 떠 있을 시간
    private VisualElement m_NonPlayerDialogue; // NPC 대화창 UI 요소
    private float m_TimerDisplay; // 남은 표시 시간을 체크할 타이머

    // 승패 장면
    private VisualElement m_WinScreen;
    private VisualElement m_LoseScreen;

    // 로봇 카운터
    private Label m_RobotCounter;

    // Awake is called when the script instance is being loaded (in this situation, when the game scene loads)
    private void Awake()
    {
        instance = this;
    }

    // 객체가 생성된 후 첫 번째 Update 직전에 호출되는 함수
    void Start()
    {
        // 1. 현재 오브젝트에 붙어있는 UIDocument 컴포넌트를 가져옵니다.
        UIDocument uiDocument = GetComponent<UIDocument>();

        // 2. UI 레이아웃(UXML)에서 이름이 "HealthBar"인 요소를 찾아 변수에 할당합니다.
        // Q는 'Query'의 약자로, 특정 요소를 찾는 기능을 합니다.
        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");

        // 3. 시작할 때 체력바를 100%(1.0)로 초기화합니다.
        SetHealthValue(1.0f);

        // 이름이 "NPCDialogue"인 요소를 찾고, 처음에는 화면에서 숨김
        m_NonPlayerDialogue = uiDocument.rootVisualElement.Q<VisualElement>("NPCDialogue");
        m_NonPlayerDialogue.style.display = DisplayStyle.None;
        m_TimerDisplay = -1.0f; // 타이머 초기화 (-1은 작동하지 않는 상태를 의미)

        m_LoseScreen = uiDocument.rootVisualElement.Q<VisualElement>("LoseScreenContainer");
        m_WinScreen = uiDocument.rootVisualElement.Q<VisualElement>("WinScreenContainer");

        m_RobotCounter = uiDocument.rootVisualElement.Q<Label>("CounterLabel");

    }

    // 외부(예: Player 스크립트)에서 체력 수치를 변경할 때 호출하는 함수
    public void SetHealthValue(float percentage)
    {
        // m_Healthbar의 가로 길이(width) 스타일을 퍼센트 단위로 변경합니다.
        // 0.0 ~ 1.0 사이의 값을 받아 0% ~ 100%로 변환하여 적용합니다.
        // 퍼센트는 부모 너비 기준으로 작동합니다.
        m_Healthbar.style.width = Length.Percent(100 * percentage);
    }

    private void Update()
    {
        if (m_TimerDisplay > 0)
        {
            m_TimerDisplay -= Time.deltaTime;

            // 시간이 다 되면 대화창을 다시 숨김
            if (m_TimerDisplay < 0)
            {
                m_NonPlayerDialogue.style.display = DisplayStyle.None;
            }
        }
    }

    public void DisplayDialogue()
    {
        m_NonPlayerDialogue.style.display = DisplayStyle.Flex; // 대화창 보이기
        m_TimerDisplay = displayTime; // 타이머 리셋
    }

    public void DisplayWinScreen()
    {
        m_WinScreen.style.opacity = 1.0f;
    }

    public void DisplayLoseScreen()
    {
        m_LoseScreen.style.opacity = 1.0f;
    }

    // 로봇 숫자를 업데이트하는 함수
    public void SetCounter(int current, int enemies)
    {
        m_RobotCounter.text = $"{current} / {enemies}";
    }

}