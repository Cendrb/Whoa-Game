using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class CharacterShopScript : MonoBehaviour
{
    public Text CurrentCharacterNameLabel;

    public GameObject CharactersParent;
    public GameObject CharacterLinePrefab;

    public Text NameText;
    public Text MultiplierText;
    public Text HealthText;
    public Text KlidText;
    public Text KlidRegenText;
    public Text SelfSpellSlotCount;
    public Text RangedSpellSlotCount;
    public Text WeightText;
    public Text WhoaPowerText;
    public Text SpeedText;
    public Text PriceText;

    public Button BuyButton;
    public Button SelectButton;

    public ScrollRect scrollRect;

    WhoaCharacter selectedCharacter;

    // Use this for initialization
    private void Start()
    {
        float counter = 0;
        foreach (WhoaCharacter character in WhoaPlayerProperties.Characters.characters)
        {
            GameObject characterObject = (GameObject)Instantiate(CharacterLinePrefab);
            RectTransform rectTransform = characterObject.GetComponent<RectTransform>();
            rectTransform.SetParent(CharactersParent.transform);
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchoredPosition = new Vector3(0, counter);

            Text text = characterObject.transform.FindChild("Text").gameObject.GetComponent<Text>();
            text.text = character.Name;

            Button button = characterObject.GetComponent<Button>();
            WhoaCharacter characterCopy = character;
            button.onClick.AddListener(new UnityAction(() => SelectCharacter(characterCopy)));

            Image image = characterObject.transform.FindChild("Image").gameObject.GetComponent<Image>();
            image.sprite = character.Sprite;

            counter -= 80;
        }

        RectTransform rekt = CharactersParent.GetComponent<RectTransform>();
        rekt.sizeDelta = new Vector2(0, -counter);

        scrollRect.normalizedPosition = new Vector2(0, 1);

        SelectCharacter(WhoaPlayerProperties.Characters.characters[0]);
    }


    private void SelectCharacter(WhoaCharacter character)
    {
        selectedCharacter = character;
        ViewData();
    }

    private void ViewData()
    {

        if (selectedCharacter.Data.Purchased)
        {
            PriceText.text = "Purchased";
            BuyButton.interactable = false;
            SelectButton.interactable = true;
        }
        else
        {
            PriceText.text = selectedCharacter.Price.Format();
            BuyButton.interactable = true;
        }
        if (selectedCharacter == WhoaPlayerProperties.Character)
            SelectButton.interactable = false;

        WhoaCharacter active = WhoaPlayerProperties.Character;

        NameText.text = selectedCharacter.Name;
        setValueAndColor(MultiplierText, selectedCharacter.Multiplier, active.Multiplier);
        setValueAndColor(HealthText, selectedCharacter.Health, active.Health);
        setValueAndColor(SelfSpellSlotCount, selectedCharacter.SelfSpellSlots, active.SelfSpellSlots);
        setValueAndColor(RangedSpellSlotCount, selectedCharacter.RangedSpellSlots, active.RangedSpellSlots);
        setValueAndColor(WeightText, selectedCharacter.Mass, active.Mass);
        setValueAndColor(WhoaPowerText, selectedCharacter.Flap, active.Flap);
        setValueAndColor(SpeedText, selectedCharacter.Speed, active.Speed);
        setValueAndColor(KlidRegenText, selectedCharacter.KlidEnergyRegen, active.KlidEnergyRegen);
        setValueAndColor(KlidText, selectedCharacter.KlidEnergy, active.KlidEnergy);

        CurrentCharacterNameLabel.text = active.Name;
    }

    private void setValueAndColor(Text target, float selected, float active)
    {
        if (selected > active)
        {
            target.color = new Color32(0, 220, 0, 255);
            target.text = String.Format("{0} (+{1})", selected, selected - active);
        }
        else if (selected < active)
        {
            target.color = new Color32(150, 0, 0, 255);
            target.text = String.Format("{0} (-{1})", selected, active - selected);
        }
        else
        {
            target.color = Color.white;
            target.text = String.Format("{0} (=)", selected);
        }
    }

    public void BuySelectedCharacter()
    {
        if (!selectedCharacter.Data.Purchased && selectedCharacter.BuyCharacter() == BuyCharacterResult.success)
        {
            ViewData();
        }
    }

    public void SelectSelectedCharacter()
    {
        if (selectedCharacter.Data.Purchased)
        {
            WhoaPlayerProperties.SetCharacter(selectedCharacter);
            ViewData();
        }
    }
}
