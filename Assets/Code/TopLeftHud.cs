using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TopLeftHud : MonoBehaviour
{
    private Text _levelText;
    private Text _lifesText;
    private Text _dataTransferedText;
    private Text _fuelText;
    private Satelite _satelite;

    void Start()
    {
        _levelText = transform.FindChild("Level").GetComponent<Text>();
        _lifesText = transform.FindChild("Lifes").GetComponent<Text>();
        _dataTransferedText = transform.FindChild("DataTransfered").GetComponent<Text>();
        _fuelText = transform.FindChild("Fuel").GetComponent<Text>();
        _satelite = GameObject.Find("Satellite").GetComponent<Satelite>();
    }

    void Update()
    {
        if (LevelControl.CurrentLevel != null)
        {
            _levelText.text = string.Format("Level {0}", LevelControl.CurrentLevel.Number);
            _dataTransferedText.text = string.Format("{0}/{1} gigabytes transmitted", LevelControl.DataTransmitted.ToString("0.#"), LevelControl.CurrentLevel.TotalData.ToString("0.#"));
            _lifesText.text = string.Format("{0}/{1} timeouts", LevelControl.MAX_LIFES - LevelControl.Lifes, LevelControl.MAX_LIFES);
            _fuelText.text = string.Format("Fuel {0}%", (int)(_satelite.Fuel * 100.0f));
        }
        else
        {
            _lifesText.text = _levelText.text = _dataTransferedText.text = "";
        }
    }
}