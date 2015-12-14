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
        _text.text = SignalCreator.DataTransmitted.ToString(".0") + " gigabytes transmitted";
    }
}