using UnityEngine;
using Photon.VR;
using TMPro;

public class ColorManager : MonoBehaviour
{
    public float redMultiplier = 0.1f;
    public float greenMultiplier = 0.1f;
    public float blueMultiplier = 0.1f;
    public Color currentColor;
    public Material offlineColor;

    public TextMeshPro redValueText;
    public TextMeshPro greenValueText;
    public TextMeshPro blueValueText;

    private void Start()
    {
        LoadColor();
    }

    public void UpdateColor()
    {
        float trueRed = currentColor.r * redMultiplier;
        float trueGreen = currentColor.g * greenMultiplier;
        float trueBlue = currentColor.b * blueMultiplier;
        Color myColour = new Color(trueRed, trueGreen, trueBlue);
        PhotonVRManager.SetColour(myColour);

        UpdateColorCode();
        SaveColor();
    }

    private void UpdateColorCode()
    {
        int redValue = Mathf.RoundToInt(currentColor.r * 10);
        int greenValue = Mathf.RoundToInt(currentColor.g * 10);
        int blueValue = Mathf.RoundToInt(currentColor.b * 10);

        redValueText.text = "Red: " + redValue.ToString();
        greenValueText.text = "Green: " + greenValue.ToString();
        blueValueText.text = "Blue: " + blueValue.ToString();
    }

    private void SaveColor()
    {
        PlayerPrefs.SetFloat("RedMultiplier", redMultiplier);
        PlayerPrefs.SetFloat("GreenMultiplier", greenMultiplier);
        PlayerPrefs.SetFloat("BlueMultiplier", blueMultiplier);
        PlayerPrefs.SetFloat("RedValue", currentColor.r);
        PlayerPrefs.SetFloat("GreenValue", currentColor.g);
        PlayerPrefs.SetFloat("BlueValue", currentColor.b);
        PlayerPrefs.Save();
    }

    private void LoadColor()
    {
        if (PlayerPrefs.HasKey("RedMultiplier"))
        {
            redMultiplier = PlayerPrefs.GetFloat("RedMultiplier");
        }

        if (PlayerPrefs.HasKey("GreenMultiplier"))
        {
            greenMultiplier = PlayerPrefs.GetFloat("GreenMultiplier");
        }

        if (PlayerPrefs.HasKey("BlueMultiplier"))
        {
            blueMultiplier = PlayerPrefs.GetFloat("BlueMultiplier");
        }

        if (PlayerPrefs.HasKey("RedValue") && PlayerPrefs.HasKey("GreenValue") && PlayerPrefs.HasKey("BlueValue"))
        {
            currentColor = new Color(PlayerPrefs.GetFloat("RedValue"), PlayerPrefs.GetFloat("GreenValue"), PlayerPrefs.GetFloat("BlueValue"));
            UpdateColorCode();
        }
    }
}
