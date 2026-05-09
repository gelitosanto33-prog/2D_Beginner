using Beginner2D;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬(레벨) 재시작을 위해 필요한 네임스페이스

public class GameManager : MonoBehaviour
{
    public PlayerController player; // 플레이어 참조 (체력 확인용)
    EnemyController[] enemies;      // 맵에 있는 모든 적을 저장할 배열
    public UIHandler uiHandler;     // UI 제어 스크립트 참조
    int enemiesFixed = 0; // 수정된 적의 수

    void Start()
    {
        // 게임 시작 시 씬에 배치된 모든 EnemyController를 찾아 배열에 저장
        // FindObjectsSortMode.None는 정렬 옵션 없음
        enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        
        foreach (var enemy in enemies)
        {
            enemy.OnFixed += HandleEnemyFixed; // 적이 가진 OnFixed라는 알림 벨에 HandleEnemyFixed라는 함수를 연결
        }
        uiHandler.SetCounter(0, enemies.Length);
        player.OnTalkedToNPC += HandlePlayerTalkedToNPC; // 함수 연결
    }

    void Update()
    {
        // 1. 패배 조건: 플레이어 체력이 0 이하인가?
        if (player.health <= 0)
        {
            uiHandler.DisplayLoseScreen(); // 패배 UI 출력 (아까 만든 페이드 효과 발동)
            Invoke(nameof(ReloadScene), 3f); // 3초 뒤에 ReloadScene 함수 실행
        }

    }

    // 모든 적의 상태를 체크하는 함수
    bool AllEnemiesFixed()
    {
        foreach (EnemyController enemy in enemies)
        {
            // 한 명이라도 여전히 고장(isBroken) 상태라면 false 반환
            if (enemy.isBroken) return false;
        }
        // 모든 적을 확인했는데 고장 난 적이 없다면 true 반환
        return true;
    }

    // 현재 씬을 다시 불러오는 함수 (게임 재시작)
    // SceneManager.GetActiveScene().name 현재 내가 플레이 중인 씬의 이름을 알아냄
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void HandleEnemyFixed()
    {
        enemiesFixed++;
        uiHandler.SetCounter(enemiesFixed, enemies.Length);
    }

    void HandlePlayerTalkedToNPC()
    {
        if (AllEnemiesFixed())
        {
            uiHandler.DisplayWinScreen();
            Invoke(nameof(ReloadScene), 3f);
        }
        else
        {
            UIHandler.instance.DisplayDialogue();
        }
    }

}