using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DataTransferedText : MonoBehaviour
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

        _text.text = string.Format("{0}/{1} gigabytes transmitted", LevelControl.DataTransmitted.ToString("0.#"), LevelControl.CurrentLevel.TotalData.ToString("0.#"));
    }
}