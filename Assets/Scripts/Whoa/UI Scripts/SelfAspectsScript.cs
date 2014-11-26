using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using Aspects.Self;
using System.Collections.Generic;

public class SelfAspectsScript : MonoBehaviour
{

    public GameObject aspectsParent;
    public GameObject aspectLinePrefab;

    Color normalColor;
    public Color selectedColor;

    List<Button> templateButtons = new List<Button>();
    Button lastSelectedButton;

    // Use this for initialization
    private void Start()
    {
        normalColor = aspectLinePrefab.GetComponent<Button>().image.color;
        float counter = 0;
        foreach (SelfAspectTemplate template in WhoaPlayerProperties.AspectsTemplates.SelfAspectsTemplates)
        {
            GameObject aspectObject = (GameObject)Instantiate(aspectLinePrefab);
            RectTransform rectTransform = aspectObject.GetComponent<RectTransform>();
            rectTransform.parent = aspectsParent.transform;
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchoredPosition = new Vector3(0, counter);

            Text text = aspectObject.transform.FindChild("Text").gameObject.GetComponent<Text>();
            text.text = template.Name;

            Image image = aspectObject.transform.FindChild("Image").gameObject.GetComponent<Image>();
            image.sprite = template.Sprite;

            Text adCostText = aspectObject.transform.FindChild("Price").gameObject.GetComponent<Text>();
            adCostText.text = template.RequiredMoney.ToString() + " AD";

            Text highscoreText = aspectObject.transform.FindChild("Highscore").gameObject.GetComponent<Text>();
            highscoreText.text = template.RequiredHighscore.ToString() + " HS";

            Button button = aspectObject.GetComponent<Button>();
            int index = WhoaPlayerProperties.AspectsTemplates.SelfAspectsTemplates.IndexOf(template);
            button.onClick.AddListener(new UnityAction(() => SelectTemplate(index)));
            templateButtons.Add(button);
            counter -= 70;
        }

        SelectTemplate(0);
    }

    private void SelectTemplate(int index)
    {
        SelectButton(templateButtons[index]);
        ViewData();
    }

    private void SelectButton(Button button)
    {
        if (lastSelectedButton != null)
            DeselectButton(lastSelectedButton);
        lastSelectedButton = button;
        button.image.color = selectedColor;
    }

    private void DeselectButton(Button button)
    {
        button.image.color = normalColor;
    }

    private void ViewData()
    {
        /*
        if (selectedCharacter.Data.Purchased)
        {
            PriceText.text = "Purchased";
            BuyButton.interactable = false;
            SelectButton.interactable = true;
        }
        else
        {
            PriceText.text = selectedCharacter.Price.ToString();
            BuyButton.interactable = true;
        }
        if (selectedCharacter == WhoaPlayerProperties.Character)
            SelectButton.interactable = false;

        WhoaCharacter active = WhoaPlayerProperties.Character;

        CurrentCharacterNameLabel.text = active.Name;*/
    }

    public void BuySelectedCharacter()
    {

    }

    public void SelectSelectedCharacter()
    {/*
        if (selectedCharacter.Data.Purchased)
        {
            WhoaPlayerProperties.SetCharacter(selectedCharacter);
            ViewData();
        }*/
    }
}

