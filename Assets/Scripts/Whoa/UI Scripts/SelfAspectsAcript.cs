using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class SelfAspectsAcript : MonoBehaviour {

    public GameObject aspectsParent;
    public GameObject aspectLinePrefab;

    // Use this for initialization
    private void Start()
    {
        float counter = 0;
        foreach (WhoaCharacter character in WhoaPlayerProperties.Characters.characters)
        {
            GameObject characterObject = (GameObject)Instantiate(aspectLinePrefab);
            RectTransform rectTransform = characterObject.GetComponent<RectTransform>();
            rectTransform.parent = aspectsParent.transform;
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchoredPosition = new Vector3(0, counter);

            Text text = characterObject.transform.FindChild("Text").gameObject.GetComponent<Text>();
            text.text = character.Name;

            Button button = characterObject.transform.FindChild("Button").gameObject.GetComponent<Button>();
            int index = WhoaPlayerProperties.Characters.characters.IndexOf(character);
            button.onClick.AddListener(new UnityAction(() => SelectCharacter(index)));

            Image image = characterObject.transform.FindChild("Image").gameObject.GetComponent<Image>();
            image.sprite = character.Sprite;

            counter -= 80;
        }

        SelectCharacter(0);
    }


    private void SelectCharacter(int index)
    {
        //selectedCharacter = WhoaPlayerProperties.Characters.characters[index];
        ViewData();
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
