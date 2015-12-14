using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

public class LifesText : MonoBehaviour
{
    private Text _text;

    void Start()
    {
        _text = GetComponent<Text>();
    }

    void Update()
    {
        var stringBuilder = new StringBuilder();

        for (int i = 0; i < SignalCreator.Lifes; i++)
        {
            stringBuilder.Append("<color=#00FF00>:) </color>");
        }

        for (int i = 0; i < SignalCreator.MAX_LIFES - SignalCreator.Lifes; i++)
        {
            stringBuilder.Append("<color=#FF0000>:( </color>");
        }

        _text.text = stringBuilder.ToString();
    }
}