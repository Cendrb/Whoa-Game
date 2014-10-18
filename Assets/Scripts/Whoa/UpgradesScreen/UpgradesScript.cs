using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradesScript : MonoBehaviour
{
    public Text currentHealth;
    public Text currentKlid;
    public Text currentKlidRegen;

    public Text selectedUpgradeName;

    WhoaCharacter currentCharacter;

    private void Start()
    {
        currentCharacter = WhoaPlayerProperties.Character;

        InitializeCurrents();
    }

    private void InitializeCurrents()
    {
        currentHealth.text = currentCharacter.Health.ToString();
        currentKlid.text = currentCharacter.KlidEnergy.ToString();
        currentKlidRegen.text = currentCharacter.KlidEnergyRegen.ToString();
    }

    public void UpgradeHealth()
    {
        selectedUpgradeName.text = "Health upgrade";
    }

    public void UpgradeKlid()
    {
        selectedUpgradeName.text = "Klid upgrade";
    }
}
