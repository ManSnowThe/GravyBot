using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoots : MonoBehaviour
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
            cm.hasBoots = true;
            anim.runtimeAnimatorController = Resources.Load("NewCharacterWithGrav") as RuntimeAnimatorController;

            //Destroy(this.gameObject);
        }
    }
}
