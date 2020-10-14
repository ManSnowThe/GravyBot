using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Sprite[] HealthSprites;
    public Image HealthUI;
    private CharacterMovement cm;

    private void Start()
    {
        cm = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
    }
    private void FixedUpdate()
    {
        try
        {
            HealthUI.sprite = HealthSprites[cm.curHealth];
            if(cm.curHealth <= 0)
            {
                HealthUI.sprite = HealthSprites[0];
            }
        }
        catch (System.IndexOutOfRangeException)
        {
            HealthUI.sprite = HealthSprites[0];
        }
    }
}
