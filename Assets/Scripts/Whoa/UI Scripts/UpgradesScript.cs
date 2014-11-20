using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UpgradesScript : MonoBehaviour
{
    public ShowMoneyScript MoneyScript;

    public Button buyButton;
    public Text selectedUpgradeName;
    public Text selectedUpgradePrice;

    public GameObject upgradeLinePrefab;
    public GameObject effectLinePrefab;

    public GameObject upgradesParent;
    public GameObject effectsParent;

    WhoaCharacter currentCharacter;
    CharacterUpgrade selectedUpgrade;
    int selectedIndex;

    private void Start()
    {
        currentCharacter = WhoaPlayerProperties.Character;

        float counter = 0;
        foreach (CharacterUpgrade upgrade in WhoaPlayerProperties.Character.Upgrades)
        {
            GameObject upgradeObject = (GameObject)Instantiate(upgradeLinePrefab);
            upgradeObject.name = upgrade.Name;
            RectTransform rectTransform = upgradeObject.GetComponent<RectTransform>();
            rectTransform.parent = upgradesParent.transform;
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchoredPosition = new Vector3(0, counter);

            Text nameText = upgradeObject.transform.FindChild("NameText").gameObject.GetComponent<Text>();
            nameText.text = upgrade.Name;

            Text levelText = upgradeObject.transform.FindChild("LevelText").gameObject.GetComponent<Text>();
            levelText.text = upgrade.GetLevel().ToString();

            Image image = upgradeObject.transform.FindChild("Image").gameObject.GetComponent<Image>();
            image.sprite = upgrade.Sprite;

            int index = currentCharacter.Upgrades.IndexOf(upgrade);
            Button button = upgradeObject.transform.FindChild("Button").gameObject.GetComponent<Button>();
            button.onClick.AddListener(new UnityEngine.Events.UnityAction(() => selectUpgrade(index)));

            counter -= 80;
        }
        if (currentCharacter.Upgrades.Count > 0)
            selectUpgrade(0);
    }

    private void selectUpgrade(int index)
    {
        selectedIndex = index;
        selectedUpgrade = currentCharacter.Upgrades[index];
        try
        {
            showUpgrade();
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void BuySelectedUpgrade()
    {
        switch (currentCharacter.BuyUpgrade(selectedUpgrade))
        {
            case BuyUpgradeResult.success:
                upgradesParent.transform.FindChild(selectedUpgrade.Name).FindChild("LevelText").gameObject.GetComponent<Text>().text = selectedUpgrade.GetLevel().ToString();
                showUpgrade();
                MoneyScript.ShowMoney();
                break;
        }
    }

    private void showUpgrade()
    {
        selectedUpgradeName.text = selectedUpgrade.Name;

        // Effects
        // remove old labels
        foreach (Transform transform in effectsParent.transform)
        {
            GameObject.Destroy(transform.gameObject);
        }

        if (selectedUpgrade.GetLevel() >= selectedUpgrade.MaxLevel)
        {
            buyButton.interactable = false;
            selectedUpgradePrice.text = "Max level";
        }
        else
        {
            buyButton.interactable = true;
            selectedUpgradePrice.text = selectedUpgrade.GetPrice().ToString();

            // add new labels
            float counter = 0;
            foreach (UpgradeEffect effect in selectedUpgrade.Effects)
            {
                GameObject effectObject = (GameObject)Instantiate(effectLinePrefab);
                effectObject.name = Static.GetName(effect.AffectedProperty);
                RectTransform rectTransform = effectObject.GetComponent<RectTransform>();
                rectTransform.parent = effectsParent.transform;
                rectTransform.localScale = new Vector3(1, 1, 1);
                rectTransform.anchoredPosition = new Vector3(0, counter);

                float currentValue = 0;
                switch (effect.AffectedProperty)
                {
                    case EffectAffectedProperty.health:
                        currentValue = currentCharacter.Health;
                        break;
                    case EffectAffectedProperty.klid:
                        currentValue = currentCharacter.KlidEnergy;
                        break;
                    case EffectAffectedProperty.klidRegen:
                        currentValue = currentCharacter.KlidEnergyRegen;
                        break;
                }
                float newValue = effect.GetModifiedValue(currentValue, selectedUpgrade.GetLevel() + 1);
                float difference = newValue - currentValue;

                Text effectName = effectObject.transform.FindChild("EffectName").gameObject.GetComponent<Text>();
                effectName.text = Static.GetName(effect.AffectedProperty);

                Text currentText = effectObject.transform.FindChild("CurrentValue").gameObject.GetComponent<Text>();
                currentText.text = currentValue.ToString();

                Text newText = effectObject.transform.FindChild("NewValue").gameObject.GetComponent<Text>();
                newText.text = newValue.ToString();

                Text diffText = effectObject.transform.FindChild("Difference").gameObject.GetComponent<Text>();
                diffText.text = difference.ToString();

                Text ratioText = effectObject.transform.FindChild("Ratio").gameObject.GetComponent<Text>();
                ratioText.text = "1 unit per " + (selectedUpgrade.GetPrice() / difference).ToString("0.##") + " AD";

                counter -= 125;
            }
        }
        
    }
}
