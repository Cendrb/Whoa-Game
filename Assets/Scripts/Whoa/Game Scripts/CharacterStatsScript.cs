using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterStatsScript : MonoBehaviour
{
    public Text multiplierText;
    public Text healthText;
    public Text flapStrengthText;
    public Text speedText;
    public Text gravityText;
    public Text klidEnergyText;
    public Text klidEnergyRegenText;
    public Text spellSlotCountText;
    public Text obstaclesPassedText;
    public Text moneyEarnedText;
    public Text whoaFlapsText;
    public Text priceText;

    void Start()
    {
        WhoaCharacter currentCharacter = WhoaPlayerProperties.Character;
        multiplierText.text = currentCharacter.Multiplier.ToString();
        flapStrengthText.text = currentCharacter.Flap.ToString();
        speedText.text = currentCharacter.Speed.ToString();
        gravityText.text = currentCharacter.Gravity.ToString();
        obstaclesPassedText.text = currentCharacter.Data.Statistics.ObstaclesPassed.ToString();
        moneyEarnedText.text = currentCharacter.Data.Statistics.MoneyEarned.ToString();
        whoaFlapsText.text = currentCharacter.Data.Statistics.WhoaFlaps.ToString();
        priceText.text = currentCharacter.Price.ToString();
        healthText.text = currentCharacter.Health.ToString();
        klidEnergyText.text = currentCharacter.KlidEnergy.ToString();
        klidEnergyRegenText.text = currentCharacter.KlidEnergyRegen.ToString();
        spellSlotCountText.text = currentCharacter.SelfSpellSlots.ToString();
    }
}
