﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class DataLog : MonoBehaviour
{
    public GameObject LogPrefab;

    private Stack<Data> _data;
    private const float TOTAL_LETTERS_TIMER = 0.05f;
    private const float TOTAL_BLINK_TIMER = 0.5f;
    private float _lettersTimer;
    private float _blinkTimer;
    private bool _underline;
    private string _currentEnd = " ";

    void Start()
    {
        _data = new Stack<Data>();
        Log("system initialized...", Color.green);
    }

    void Update()
    {
        if (_blinkTimer > 0.0f)
            _blinkTimer -= Time.deltaTime;
        else
        {
            _blinkTimer += TOTAL_BLINK_TIMER;

            var d = _data.Peek();
            _currentEnd = (_underline ? " " : "_");
            d.Text.text = d.Text.text.Substring(0, d.Text.text.Length - 1) + _currentEnd;
            _underline = !_underline;
        }

        if (_lettersTimer > 0.0f)
        {
            _lettersTimer -= Time.deltaTime;
            return;
        }

        _lettersTimer += TOTAL_LETTERS_TIMER;

        var data = _data.Peek();
        var currLength = data.Text.text.Length - 1;
        if (currLength == data.Value.Length)
            data.Text.text = data.Value + _currentEnd;
        else
            data.Text.text = data.Value.Substring(0, currLength + 1) + "_";
    }

    public static void LogStatic(string value)
    {
        GameObject.Find("Canvas").transform.FindChild("Panel").GetComponent<DataLog>().Log(value, new Color(1.0f, 1.0f, 1.0f, 1.0f));
    }

    public static void LogStatic(string value, Color color)
    {
        GameObject.Find("Canvas").transform.FindChild("Panel").GetComponent<DataLog>().Log(value, color);
    }
    
    public void Log(string value, Color color)
    {
        for (int i = 0; i < _data.Count; i++)
        {
            var data = _data.ElementAt(i);
            data.Text.rectTransform.anchoredPosition = new Vector2(0.0f, (i + 1) * 20.0f);
            data.Text.color = new Color(data.Text.color.r, data.Text.color.g, data.Text.color.b, 1.0f - ((float)i / 5.0f));
        }

        if (_data.Count > 0)
            _data.Peek().Text.text = _data.Peek().Value;

        var textObject = GameObject.Instantiate(LogPrefab);
        var text = textObject.GetComponent<Text>();
        text.text = _currentEnd;
        text.color = color;
        text.rectTransform.SetParent(GetComponent<Image>().rectTransform, false);
        _data.Push(new Data(value, text.GetComponent<Text>(), color));
    }

    public class Data
    {
        public Data(string value, Text text, Color color)
        {
            Value = value;
            Text = text;
            Color = color;
        }

        public string Value;
        public Text Text;
        public Color Color;
    }
}