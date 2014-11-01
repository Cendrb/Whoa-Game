using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradesScript : MonoBehaviour
{
    public ShowMoneyScript MoneyScript;

    public Text selectedUpgradeName;
    public Text selectedUpgradePrice;

    public GameObject upgradeLinePrefab;
    public GameObject upgradesParent;

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
    }

    private void selectUpgrade(int index)
    {
        selectedIndex = index;
        selectedUpgrade = currentCharacter.Upgrades[index];
        showUpgrade();
    }

    public void BuySelectedUpgrade()
    {
        switch(currentCharacter.BuyUpgrade(selectedUpgrade))
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
        selectedUpgradePrice.text = selectedUpgrade.GetPrice().ToString();
    }
}
