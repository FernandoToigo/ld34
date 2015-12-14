using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class DataLog : MonoBehaviour
{
    public static readonly Color SUCCESS_COLOR = new Color(0.4313f, 1.0f, 0.4313f, 1.0f);
    public static readonly Color FAIL_COLOR = new Color(1.0f, 0.4313f, 0.4313f, 1.0f);

    private const int MAX_LOGS = 6;

    public GameObject LogPrefab;

    private List<Data> _data;
    private CommandTextWriter _writer;

    void Start()
    {
        _writer = GetComponent<CommandTextWriter>();
        _data = new List<Data>();
        Log("system initialized...", DataLog.SUCCESS_COLOR);
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
            var data = _data[i];
            data.Text.rectTransform.anchoredPosition = new Vector2(0.0f, (i + 1) * 20.0f);
            data.Text.color = new Color(data.Text.color.r, data.Text.color.g, data.Text.color.b, 1.0f - ((float)(i + 1) / (float)MAX_LOGS));
        }

        if (_data.Count > 0)
            _data.First().Text.text = _data.First().Value;

        var textObject = GameObject.Instantiate(LogPrefab);
        var text = textObject.GetComponent<Text>();
        text.color = color;
        text.rectTransform.SetParent(GetComponent<Image>().rectTransform, false);
        _data.Insert(0, new Data(value, text.GetComponent<Text>(), color));
        _writer.WriteText(value, text);
        for (int i = MAX_LOGS; i < _data.Count; i++)
        {
            var data = _data[i];
            GameObject.Destroy(data.Text.gameObject);
        }

        if (_data.Count > MAX_LOGS)
            _data.RemoveRange(MAX_LOGS, _data.Count - MAX_LOGS);
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