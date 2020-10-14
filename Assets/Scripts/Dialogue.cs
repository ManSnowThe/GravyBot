// Класс, который принимает массив строк
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public Sprite Pic; // Картинка в диалоговом окне
    public string name; // имя объекта

    [TextArea(3, 10)]
    public string[] sentences; // предложения
}