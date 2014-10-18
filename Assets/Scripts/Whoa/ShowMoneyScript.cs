using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowMoneyScript : MonoBehaviour
{

    public Text moneyText;

    // Use this for initialization
    void Start()
    {
        WhoaPlayerProperties.Load();
        moneyText.text = WhoaPlayerProperties.Money.ToString();
    }
}
