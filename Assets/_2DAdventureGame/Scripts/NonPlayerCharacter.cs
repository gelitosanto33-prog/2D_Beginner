using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public GameObject dialogueBubble;

    void Start()
    {
        dialogueBubble.SetActive(false); // 대화 키 말풍선 안 보이게
    }
}