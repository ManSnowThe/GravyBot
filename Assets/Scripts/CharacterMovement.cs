using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public LayerMask groundLayer;
    public bool facingRight = true;
    private Rigidbody2D _rig;
    private Animator anim;

    private float topSpeed = 6f;

    bool direct = true;
    bool shooting = false;
    internal bool hasBoots = false;
    internal bool hasGun = false; // Взят ли предмет "Оружие"
    float time;

    public Transform ProjectileLocation;
    public GameObject bulletForward;
    public GameObject bulletBackward;

    private int maxHealth = 3;
    internal int curHealth;
    bool damaged = false;

    private EnemyMovement enemy;

    public CameraMove camMove;

    void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        curHealth = maxHealth;

        // Игнорирование коллайдера противника
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyMovement>();
        Collider2D[] ColIgnore = this.GetComponents<Collider2D>();
        Physics2D.IgnoreCollision(ColIgnore[0], enemy.GetComponent<BoxCollider2D>(), true);
    }

    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        _rig.velocity = new Vector2(move * topSpeed, _rig.velocity.y);

        if (move > 0 && !facingRight)
        {
            Vector2 pos = transform.localPosition;
            pos.x += 0.2f;
            transform.localPosition = pos;
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Vector2 pos = transform.localPosition;
            pos.x += -0.2f;
            transform.localPosition = pos;
            Flip();
        }


        anim.SetBool("Ground", isGrounded());
        anim.SetFloat("Speed", Mathf.Abs(move));
        anim.SetBool("Shoot", shooting);

        anim.SetBool("HasGun", hasGun);

        if (curHealth <= 0)
        {
            StartCoroutine(Died());
        }
    }

    private void Update()
    {
        if (hasBoots && isGrounded() && Input.GetKeyDown(KeyCode.Z))
        {
            Grav();
        }
        if (hasGun && !shooting && Input.GetKeyDown(KeyCode.X))
        {
            time = 0.15f;
            Attack();
            StartCoroutine(Shoot());
        }
    }

    bool isGrounded()
    {
        Vector2 position1 = transform.position;
        position1.x -= 0.282f;
        Vector2 position2 = transform.position;
        position2.x += 0.157f;
        Vector2 position3 = transform.position;

        Vector2 direction;

        if (direct)
        {
            direction = Vector2.down;
        }
        else
        {
            direction = Vector2.up;
        }

        float distance = 0.65f;

        RaycastHit2D hit1 = Physics2D.Raycast(position1, direction, distance, groundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(position2, direction, distance, groundLayer);
        RaycastHit2D hit3 = Physics2D.Raycast(position3, direction, distance, groundLayer);

        if (!facingRight)
        {
            position1 = transform.position;
            position1.x -= -0.282f;
            position2 = transform.position;
            position2.x += -0.157f;

            hit1 = Physics2D.Raycast(position1, direction, distance, groundLayer);
            hit2 = Physics2D.Raycast(position2, direction, distance, groundLayer);
            hit3 = Physics2D.Raycast(position3, direction, distance, groundLayer);
        }

        if (hit1.collider == null && hit2.collider == null && hit3.collider == null)
        {
            Debug.DrawRay(position1, direction, Color.red);
            Debug.DrawRay(position2, direction, Color.red);
            Debug.DrawRay(position3, direction, Color.red);
            return false;
        }
        Debug.DrawRay(position1, direction, Color.green);
        Debug.DrawRay(position2, direction, Color.green);
        Debug.DrawRay(position3, direction, Color.green);
        return true;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Grav()
    {
        direct = !direct;
        _rig.gravityScale *= -1;
        Vector3 theScale = transform.localScale;
        theScale.y *= -1;
        transform.localScale = theScale;

        camMove.Yoffset *= -1;
    }

    public IEnumerator Shoot()
    {
        shooting = true;
        yield return new WaitForSeconds(time);
        shooting = false;
    }

    public void Attack()
    {
        if (facingRight)
        {
            Instantiate(bulletForward, ProjectileLocation.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        }
        if (!facingRight)
        {
            Instantiate(bulletBackward, ProjectileLocation.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }

    void Damage(int dmg)
    {
        if (!damaged)
        {
            curHealth -= dmg;
            Debug.Log(dmg);
            Knockback(100, 1000);
            StartCoroutine(Damaged());
        }
    }

    IEnumerator Damaged()
    {
        damaged = true;
        anim.Play("chDamage");
        yield return new WaitForSeconds(0.7f);
        damaged = false;
    }

    public IEnumerator Died()
    {
        anim.Play("chDeath");

        yield return new WaitForSeconds(0.5f);
        Disable();

        yield return new WaitForSeconds(2f);
        this.transform.position = new Vector3(-100, 0, transform.position.z);
        //SceneManager.LoadScene("ma1");
    }

    public void Knockback(float kbPowerUp, float kbPowerRight)
    {
        _rig.velocity = new Vector2(0, 0);

        if ((!facingRight && !enemy.facingLeft) || (facingRight && !enemy.facingLeft))
        {
            _rig.AddForce(transform.up * kbPowerUp + transform.right * kbPowerRight);
        }

        if ((facingRight && enemy.facingLeft) || (!facingRight && enemy.facingLeft))
        {
            _rig.AddForce(transform.up * kbPowerUp + (transform.right * kbPowerRight) * -1);
        }
        //StartCoroutine(MovementDisable());
    }

    IEnumerator MovementDisable()
    {
        gameObject.GetComponent<CharacterMovement>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<CharacterMovement>().enabled = true;
    }

    public void Disable()
    {
        gameObject.GetComponent<CharacterMovement>().enabled = false;
        camMove.enabled = false;
        _rig.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
