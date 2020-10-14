using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    Rigidbody2D bullet;

    int dmg = 1;
    private EnemyMovement enemy;

    void Awake()
    {
        bullet = GetComponent<Rigidbody2D>();
        bullet.AddForce(new Vector2(1, 0) * bulletSpeed, ForceMode2D.Impulse);
        StartCoroutine(BulletDestroy());
    }
    private void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyMovement>();
    }

    void Update()
    {

    }
    public void removeForce()
    {
        bullet.velocity = new Vector2(0, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            removeForce();
            Destroy(gameObject);
        }
        else if (collision.isTrigger != true && collision.CompareTag("Enemy"))
        {
            collision.SendMessageUpwards("Damage", dmg);
            //Debug.Log("Damage - " + dmg);

            removeForce();
            Destroy(gameObject);

            //Добавить
            //StartCoroutine(enemy.Knockback());
        }
    }

    // Можно использовать, чтобы сделать улучшение для оружия (Дальность стрельбы).
     public IEnumerator BulletDestroy()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }
}
