using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{ 
    public HealthbarBehaviour Healthbar;
    //Player Stats



    //Movimiento

    public float mMovementSpeed = 0.5f;
    public bool bIsGoingRight = true;
    public bool inRange = false;

    //Combate

    public float hitpoints;
    public float MaxHitpoints = 5.0f;
    public int damage;
    public bool alive = true;
    private bool enemyAlive;
    private bool cooling;
    public float attackSpeed;

    //reloj

    public float timer;
    private float intTimer;

    //Object components

    private SpriteRenderer _mSpriteRenderer;
    private Animator anim;
    private GameObject target;


    // Start is called before the first frame update
    void Start()
    {
        timer = attackSpeed;
        intTimer = attackSpeed;
        _mSpriteRenderer = gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();


        anim = gameObject.GetComponent<Animator>();
        hitpoints = MaxHitpoints;
        Healthbar.SetHealth(hitpoints, MaxHitpoints);

    }


    void Update()
    {
        Debug.Log(target);
        Flip();
        EnemyLogic();
        

    }


    void Move()
    {

        Vector3 directionTranslation = (bIsGoingRight) ? transform.right : -transform.right;
        directionTranslation *= Time.deltaTime * mMovementSpeed;
        transform.Translate(directionTranslation);
        anim.SetBool("canWalk", true);
        anim.SetBool("Attack", false);
        anim.SetBool("Hurt", false);
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Enemy")
        {
            target = trig.gameObject;
            enemyAlive = target.GetComponentInParent<Enemy>().alive;

            
        }
    }

    void Flip()
    {
        if (!bIsGoingRight)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
            _mSpriteRenderer.flipX = false;
        }
    }



    void Cooldown()
    {
        anim.SetBool("Attack", false);
        anim.SetBool("canWalk", false);
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && inRange)
        {
            cooling = false;
            timer = intTimer;

        }
    }

    void EnemyLogic()
    {
        if (alive)
        {
            if (!enemyAlive)
            {
                target = null;
            }
            if (target != null)
            {
                inRange = true;
            }
            else
            {
                inRange = false;
            }
            if (inRange)
            {
                if (!cooling)
                {
                    Attack();
                }
                else
                {
                    Cooldown();
                }
            }
            else
            {
                Move();
            }

        }
        else
        {
            Die();
        }

    }

    void Attack()
    {
        anim.SetBool("canWalk", false);
        anim.SetBool("Attack", true);
        target.GetComponentInParent<Enemy>().TakeHit(damage);
        cooling = true;
    }
    void unleashAttack()
    {

        target.GetComponentInParent<Enemy>().TakeHit(damage);
    }

    void cancelHurtAnim()
    {

        anim.SetBool("Hurt", false);
    }

    void Die()
    {
        anim.SetBool("Death", true);
        anim.SetBool("Attack", false);
        anim.SetBool("canWalk", false);
        anim.SetBool("Hurt", false);
        Destroy(gameObject, 3f);
    }

    public void TakeHit(float damage)
    {
        hitpoints -= damage;
        Healthbar.SetHealth(hitpoints, MaxHitpoints);

        if (hitpoints <= 0)
        {

            alive = false;

        }
        else
        {
            anim.SetBool("Hurt", true);
        }
    }
}