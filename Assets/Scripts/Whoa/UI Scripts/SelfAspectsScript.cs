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

    public Text nameText;
    public Text descriptionText;
    public Text priceText;
    public Text requiredCharacterText;
    public Text requiredHighscoreText;
    public Text statusText;
    public Button buyButton;

    Color normalColor;
    public Color selectedColor;

    List<Button> templateButtons = new List<Button>();
    Button lastSelectedButton;

    SelfAspectTemplate selectedTemplate;

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
        selectedTemplate = WhoaPlayerProperties.AspectsTemplates.SelfAspectsTemplates[index];
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
        buyButton.interactable = false;
        nameText.text = selectedTemplate.Name;
        descriptionText.text = selectedTemplate.Description;
        if (selectedTemplate.RequiredMoney == 0)
            priceText.text = "Free";
        else
            priceText.text = selectedTemplate.RequiredMoney.FormatAD();
        requiredHighscoreText.text = selectedTemplate.RequiredHighscore.ToString();
        if (selectedTemplate.RequiredCharacter == null)
            requiredCharacterText.text = "None";
        else
            requiredCharacterText.text = selectedTemplate.RequiredCharacter.Name;

        bool characterAvailable = false;
        if (selectedTemplate.RequiredCharacter != null)
        {
            foreach (WhoaCharacter character in WhoaPlayerProperties.Characters.characters)
                if (character == selectedTemplate.RequiredCharacter && character.Data.Purchased)
                    characterAvailable = true;
        }
        else
            characterAvailable = true;

        if (selectedTemplate.Data.Purchased)
            statusText.text = "Purchased";
        else if (selectedTemplate.RequiredHighscore > WhoaPlayerProperties.HighScore)
            statusText.text = "Insufficient highscore";
        else if (!characterAvailable)
            statusText.text = selectedTemplate.RequiredCharacter.Name + " is not purchased";
        else if (selectedTemplate.RequiredMoney > WhoaPlayerProperties.Money)
            statusText.text = "Insufficient AD";
        else
        {
            statusText.text = "Available";
            buyButton.interactable = true;
        }
    }

    public void BuySelectedTemplate()
    {
        bool characterAvailable = false;
        if (selectedTemplate.RequiredCharacter != null)
        {
            foreach (WhoaCharacter character in WhoaPlayerProperties.Characters.characters)
                if (character == selectedTemplate.RequiredCharacter && character.Data.Purchased)
                    characterAvailable = true;
        }
        else
            characterAvailable = true;
        if (selectedTemplate.RequiredHighscore <= WhoaPlayerProperties.HighScore && selectedTemplate.RequiredMoney <= WhoaPlayerProperties.Money && !selectedTemplate.Data.Purchased && characterAvailable)
        {
            WhoaPlayerProperties.Money -= selectedTemplate.RequiredMoney;
            WhoaPlayerProperties.SavePrefs();
            selectedTemplate.Data.Purchased = true;
            selectedTemplate.Save();
            ViewData();
        }
    }
}

