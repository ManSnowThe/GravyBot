using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public Image mainPic;
    public Text nameText;
    public Text dialogueText;

    //public Animator animator;
    public CharacterMovement cm;
    private Queue<string> sentences;

    public DialogueButton db;
    public Rigidbody2D rig;

    public Text FlashingText;
    bool displayText = true;

    void Start()
    {
        sentences = new Queue<string>();
    }
    
    public void StartDialogue(Dialogue dialogue)
    {
        cm.enabled = false;
        rig.constraints = RigidbodyConstraints2D.FreezePosition;

        //animator.SetBool("IsOpen", true);

        mainPic.sprite = dialogue.Pic;
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        //animator.SetBool("IsOpen", false);

        cm.enabled = true;

        rig.constraints = RigidbodyConstraints2D.FreezeRotation;
        if(db.isItem == true)
        {
            db.Anim.speed = 1;
            Destroy(db.gameObject);
        }
        db.but1 = true;
        db.but2 = false;
    }

    private void OnGUI()
    {
        if(displayText == true)
        {
            FlashingText.text = ">";
            StartCoroutine(FlashButton1());
        }
        else
        {
            FlashingText.text = "";
            StartCoroutine(FlashButton2());
        }
    }

    IEnumerator FlashButton1()
    {
        yield return new WaitForSeconds(0.5f);
        displayText = false;
    }
    IEnumerator FlashButton2()
    {
        yield return new WaitForSeconds(0.5f);
        displayText = true;
    }
}
