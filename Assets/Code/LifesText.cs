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
        if (LevelControl.CurrentLevel == null)
        {
            _text.text = "";
            return;
        }
        //var stringBuilder = new StringBuilder();

        //for (int i = 0; i < LevelControl.Lifes; i++)
        //{
        //    stringBuilder.Append("<color=#00FF00>:) </color>");
        //}

        //for (int i = 0; i < LevelControl.MAX_LIFES - LevelControl.Lifes; i++)
        //{
        //    stringBuilder.Append("<color=#FF0000>:( </color>");
        //}

        //_text.text = stringBuilder.ToString();

        _text.text = string.Format("{0}/{1} timeouts", LevelControl.MAX_LIFES - LevelControl.Lifes, LevelControl.MAX_LIFES);
    }
}