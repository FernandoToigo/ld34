using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelsText : MonoBehaviour
{
    private Text _text;

    void Start()
    {
        _text = GetComponent<Text>();
    }

    void Update()
    {
        if (LevelControl.CurrentLevel == null)
            return;

        _text.text = string.Format("Level {0}", LevelControl.CurrentLevel.Number);
    }
}