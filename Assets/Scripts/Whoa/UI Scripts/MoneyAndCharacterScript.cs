using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoneyAndCharacterScript : MonoBehaviour
{

    public Text moneyText;
    public Image characterSprite;

    // Use this for initialization
    void Start()
    {
        ShowMoney();
        ShowCharacterSprite();
    }

    public void ShowMoney()
    {
        moneyText.text = WhoaPlayerProperties.Money.FormatAD();
    }

    public void ShowCharacterSprite()
    {
        characterSprite.sprite = WhoaPlayerProperties.Character.Sprite;
    }
}
