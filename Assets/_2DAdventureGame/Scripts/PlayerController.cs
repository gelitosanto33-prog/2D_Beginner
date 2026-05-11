using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    public float speed = 3.0f;

    public AudioClip hitSound;


    public int maxHealth = 5;
    int currentHealth;
    
  
    public int health { get { return currentHealth; } }

   
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown; 

    
    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0); 

    
    public GameObject projectilePrefab;
    public InputAction LaunchAction;

   
    private NonPlayerCharacter lastNonPlayerCharacter;
    public InputAction TalkAction; 

    
    AudioSource audioSource;

    
    public event Action OnTalkedToNPC;

    
    void Start()
    {
        MoveAction.Enable();
        LaunchAction.Enable();
        TalkAction.Enable(); 
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();

    }

    
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();

        
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y); 
            moveDirection.Normalize(); 
        }

        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }

        if (LaunchAction.WasPressedThisFrame()) 
        {
            Launch();
        }

        
        RaycastHit2D hit = Physics2D.Raycast(
            rigidbody2d.position + Vector2.up * 0.2f, 
            moveDirection,                             
            1.5f,                                      
            LayerMask.GetMask("NPC")                  
        );

        
        if (hit.collider != null)
        {
            
            NonPlayerCharacter npc = hit.collider.GetComponent<NonPlayerCharacter>();

            npc.dialogueBubble.SetActive(true); 
            lastNonPlayerCharacter = npc;       
            FindFriend(); 
        }
        
        else
        {
            
            if (lastNonPlayerCharacter != null)
            {
                lastNonPlayerCharacter.dialogueBubble.SetActive(false); 
                lastNonPlayerCharacter = null; 
            }
        }

    }

    
    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }


    public void ChangeHealth(int amount)
    {
        if (amount < 1)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");

            // ADICIONE ESTA LINHA AQUI:
            PlaySound(hitSound);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, 300);
        animator.SetTrigger("Launch");
    }

    void FindFriend()
    {
        if (TalkAction.WasPressedThisFrame()) 
        {
            OnTalkedToNPC?.Invoke(); 
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip); 
    }

}