using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class CharacterController2D : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    [SerializeField] float speed = 2f;
    Vector2 motionVector;
    Animator animator;
    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    public int currentHealth;
    public float timeInvincible = 2.0f;
    public bool isInvincible;
    public float invincibleTimer;


    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        motionVector = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
            );
        animator.SetFloat("horizontal", Input.GetAxisRaw("Horizontal"));

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
                
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rigidbody2d.velocity = motionVector * speed;
    }
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        Debug.Log(currentHealth + "/" + maxHealth);
    }
}
