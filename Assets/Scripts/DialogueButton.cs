using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueButton : MonoBehaviour
{
    public string inputName;

    public Button buttonStart;
    public Button buttonNext;

    public DialogueTrigger dt;
    public Animator Anim;

    public bool but1 = true;
    public bool but2 = false;

    public bool isItem;

    void Update()
    {
        if (dt.Item.activeSelf == true)
        {
            Anim.speed = 0;
            if (but1 == true)
            {
                buttonStart.onClick.Invoke();
                StartCoroutine(TrueFalse());
            }
            if (Input.GetKeyDown(inputName) && but2 == true)
            {
                buttonNext.onClick.Invoke();
            }
        }
    }

    IEnumerator TrueFalse()
    {
        but1 = false;
        yield return new WaitForSeconds(0.1f);
        but2 = true;
    }
}
