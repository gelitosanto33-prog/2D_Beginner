using UnityEngine;

// HealthCollectible 클래스: 아이템(체력 회복 아이템 등)에 부착하는 스크립트입니다.
public class HealthCollectible : MonoBehaviour
{

    public AudioClip collectedClip;

    // OnTriggerEnter2D: 이 오브젝트의 Trigger Collider에 다른 오브젝트가 들어왔을 때 호출됩니다.
    // 'other' 변수는 방금 부딪힌(겹쳐진) 상대방의 Collider 정보입니다.
    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 부딪힌 상대방(other)에게 'PlayerController'라는 스크립트가 있는지 확인합니다.
        PlayerController controller = other.GetComponent<PlayerController>();

        // 2. 만약 상대방에게 PlayerController가 있고 최대 체력이 아니면
        if (controller != null && controller.health < controller.maxHealth)
        {
            controller.PlaySound(collectedClip);
            controller.ChangeHealth(1);

            Destroy(gameObject);
        }
    }
}