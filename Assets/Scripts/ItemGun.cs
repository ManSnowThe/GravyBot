using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGun : MonoBehaviour
{
    private CharacterMovement cm;
    public Animator anim;

    void Start()
    {
        cm = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
    }

    void FixedUpdate()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger != true && collision.CompareTag("Player"))
        {
            cm.hasGun = true;
            anim.runtimeAnimatorController = Resources.Load("NewCharacter") as RuntimeAnimatorController;

            Destroy(this.gameObject);
        }
    }
}
