using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetTextToValueScript : MonoBehaviour
{
    public Text targetText;
    public string postfixAfterValue = "";

    public void OnValueChanged(float value)
    {
        targetText.text = value.ToString() + postfixAfterValue;
    }
}
