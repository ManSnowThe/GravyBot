using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //AI
    public float engageDistance = 10f;
    public float attackDistance = 1.5f;
    public Transform target;
    public float heightDistance = 1f;

    //Movement
    public float moveSpeed = 3f;
    internal bool facingLeft = true;

    //Components
    private Animator Anim;
    private Rigidbody2D _rig;

    //Health
    public int curHealth = 5;
    bool damaged = false;

    //Atack
    public Collider2D attackTrigger;
    private bool attacking = false;
    private float attackTimer = 0;
    private float attackCD = 0.4f;
    private CharacterMovement cm;
    int damage = 1;

    //AI Wandering
    private bool dirRight = false;
    public float speed = 2.0f;
    private Vector2 pos;

    private ParticleSystem Ps;

    private void Awake()
    {
        attackTrigger.enabled = false;
        pos = this.transform.position;
    }

    void Start()
    {
        Anim = GetComponent<Animator>();
        _rig = GetComponent<Rigidbody2D>();
        Ps = GetComponentInChildren<ParticleSystem>();

        cm = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
    }

    void FixedUpdate()
    {
        Anim.SetBool("Running", true);
        Anim.SetBool("Attacking", false);

        //AttackDistance при различном направлении персонажа
        if ((cm.facingRight && facingLeft) || (!cm.facingRight && !facingLeft))
        {
            attackDistance = 1.0f;
        }
        else
        {
            attackDistance = 1.5f;
        }

        Vector2 directn = target.position - this.transform.position;

        //Height fix
        directn.x = 0;
        float dirY = directn.magnitude;

        //Если персонаж в зоне видимости противника
        if ((Vector2.Distance(target.position, this.transform.position) < engageDistance) && (DistanceY() < heightDistance))
        {
            Vector2 direction = target.position - this.transform.position;

            if (Mathf.Sign(direction.x) == 1 && facingLeft)
            {
                Flip();
            }
            else if (Mathf.Sign(direction.x) == -1 && !facingLeft)
            {
                Flip();
            }

            if (direction.magnitude >= attackDistance)
            {
                Debug.DrawLine(target.transform.position, this.transform.position, Color.yellow);

                if (facingLeft)
                {
                    this.transform.Translate(Vector2.left * (Time.deltaTime * moveSpeed));
                }
                else if (!facingLeft)
                {
                    this.transform.Translate(Vector2.right * (Time.deltaTime * moveSpeed));
                }

                attackTrigger.enabled = false;
            }

            //Персонаж в зоне атаки противника
            if (direction.magnitude < attackDistance)
            {
                if (!attacking && cm.curHealth > 0)
                {
                    Debug.DrawLine(target.transform.position, this.transform.position, Color.green);

                    attacking = true;
                    attackTimer = attackCD;

                    attackTrigger.enabled = true;
                }
                if (attacking)
                {
                    if (attackTimer > 0)
                    {
                        attackTimer -= Time.deltaTime;
                    }
                    else
                    {
                        attacking = false;
                        attackTrigger.enabled = false;
                    }
                }
                Anim.SetBool("Attacking", attacking);
            }
        }
        // Действия, если персонаж не в зоне видимости противника
        else if ((DistanceY() > heightDistance) || (Vector2.Distance(target.position, this.transform.position) > engageDistance))
        {
            Debug.DrawLine(target.position, this.transform.position, Color.red);
            //AI wandering
            AI_Wander();
        }

        if(curHealth <= 0)
        {
            StartCoroutine(Died());
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Damage(int dmg)
    {
        if (!damaged)
        {
            curHealth -= dmg;
            StartCoroutine(Damaged());
        }
    }

    IEnumerator Damaged()
    {
        damaged = true;
        Anim.Play("en1Damage");
        Ps.Play();
        yield return new WaitForSeconds(0.01f);
        damaged = false;
    }

    IEnumerator Died()
    {
        //Anim.Play("Dead");
        gameObject.GetComponent<EnemyMovement>().enabled = false;
        //gameObject.GetComponentInChildren<>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger != true && collision.CompareTag("Player"))
        {
            collision.SendMessageUpwards("Damage", damage);
        }
    }

    float DistanceY()
    {
        Vector2 TargPos = target.transform.position;
        TargPos.x = 0;
        Vector2 ThisPos = this.transform.position;
        ThisPos.x = 0;
        return Vector2.Distance(TargPos, ThisPos);
    }

    void AI_Wander()
    {
        //AI wandering
        if (dirRight)
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        else
            transform.Translate(Vector2.right * -speed * Time.deltaTime);

        if (transform.position.x >= pos.x + 2.0f)
        {
            dirRight = false;
            if (!facingLeft)
            {
                Flip();
            }
        }
        if (transform.position.x <= pos.x - 2.0f)
        {
            dirRight = true;
            if (facingLeft)
            {
                Flip();
            }
        }
    }
}
