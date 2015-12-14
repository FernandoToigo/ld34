using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;

public class CommandTextWriter : MonoBehaviour
{
    private const float TOTAL_LETTERS_TIMER = 0.05f;
    private const float TOTAL_BLINK_TIMER = 0.5f;
    private float _lettersTimer;
    private float _blinkTimer;
    private bool _underline;
    private string _currentEnd = " ";
    private string _value = "";
    private string _currentValue = "";
    private Text _text;
    private List<ColorIndex> _colors;
    private List<ColorIndex> _sizes;

    void Update()
    {
        if (_text == null)
            return;

        if (_blinkTimer > 0.0f)
            _blinkTimer -= Time.deltaTime;
        else
        {
            _blinkTimer += TOTAL_BLINK_TIMER;

            _currentEnd = (_underline ? " " : "_");
            _text.text = _text.text.Substring(0, Mathf.Max(_text.text.Length - 1, 0)) + _currentEnd;
            _underline = !_underline;
        }

        if (_lettersTimer > 0.0f)
        {
            _lettersTimer -= Time.deltaTime;
            return;
        }

        _lettersTimer += TOTAL_LETTERS_TIMER;

        string cursor;
        var currLength = _currentValue.Length;
        if (currLength == _value.Length)
        {
            _currentValue = _value;
            cursor = _currentEnd;
        }
        else
        {
            _currentValue = _value.Substring(0, currLength + 1);
            cursor = "_";
        }

        var uiValue = new StringBuilder();
        var i = 0;
        foreach (var color in _colors.OrderBy(c => c.Index))
        {
            var beforeText = _currentValue.Substring(i, Mathf.Min(color.Index - i, _currentValue.Length - i));
            uiValue.Append(beforeText);
            i += beforeText.Length;

            if (color.Index >= _currentValue.Length)
                break;

            var colorBegin = string.Format("<color=#{0}>", color.Color);
            var text = _currentValue.Substring(color.Index, Mathf.Min(color.Count, _currentValue.Length - color.Index));
            var colorEnd = "</color>";
            uiValue.Append(colorBegin);
            uiValue.Append(text);
            uiValue.Append(colorEnd);
            i += text.Length;
        }

        var afterText = _currentValue.Substring(i, _currentValue.Length - i);
        uiValue.Append(afterText);

        _text.text = uiValue.ToString() + cursor;
    }

    public void WriteText(string value, Text text)
    {
        WriteText(value, text, new List<ColorIndex>(), new List<SizeIndex>());
    }

    public void WriteText(string value, Text text, List<ColorIndex> colors, List<SizeIndex> sizes)
    {
        _currentValue = "";
        _value = value;
        _text = text;
        _colors = colors;
    }

    public void StopWriting()
    {
        _text = null;
    }

    public class ColorIndex
    {
        public ColorIndex(string color, int index, int count)
        {
            Color = color;
            Index = index;
            Count = count;
        }

        public string Color;
        public int Index;
        public int Count;
    }

    public class SizeIndex
    {
        public int Size;
        public int Index;
        public int Count;
    }
}