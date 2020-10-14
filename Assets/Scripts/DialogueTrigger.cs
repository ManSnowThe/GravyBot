using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject Item;
    //public Image ima;
    public CharacterMovement cm;

    public DialogueSystem ds;
    //public GameObject indicator;

    void Start()
    {
        Item.SetActive(false);
        //indicator.SetActive(false); // использовать для появления значка диалога
    }
    
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueSystem>().StartDialogue(dialogue);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Item.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cm.enabled = true;
            Item.SetActive(false);

            ds.EndDialogue();
        }
    }
}
