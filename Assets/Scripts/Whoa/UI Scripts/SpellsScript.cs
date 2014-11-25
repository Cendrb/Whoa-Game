using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class SpellsScript : MonoBehaviour
{
    public GameObject spellsParent;
    public GameObject spellPrefab;
    public GameObject spellDetailsParent;
    public Button deleteButton;
    public Text spellName;
    public Text spellAbbreviate;

    int selectedIndex;
    SelfSpell selectedSpell;

    void Start()
    {
        float counter = 0;
        foreach (KeyValuePair<int, SelfSpell> spell in WhoaPlayerProperties.Spells.SelfSpells)
        {
            GameObject characterObject = (GameObject)Instantiate(spellPrefab);
            RectTransform rectTransform = characterObject.GetComponent<RectTransform>();
            rectTransform.parent = spellsParent.transform;
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchoredPosition = new Vector3(0, counter);

            Text text = characterObject.transform.FindChild("Name").gameObject.GetComponent<Text>();
            text.text = spell.Value.Name;

            Text klidText = characterObject.transform.FindChild("KlidCost").gameObject.GetComponent<Text>();
            klidText.text = spell.Value.GetKlidCost(true).ToString();

            Button button = characterObject.transform.FindChild("Button").gameObject.GetComponent<Button>();
            int index = spell.Key;
            button.onClick.AddListener(new UnityAction(() => SelectCharacter(index)));

            counter -= 80;
        }

        RectTransform rekt = spellsParent.GetComponent<RectTransform>();
        rekt.sizeDelta = new Vector2(0, -counter);
    }

    private void SelectCharacter(int index)
    {
        selectedIndex = index;
        deleteButton.enabled = true;
        SelfSpell selectedSpell = WhoaPlayerProperties.Spells.SelfSpells[index];
        spellName.text = selectedSpell.Name;
        spellAbbreviate.text = selectedSpell.Abbreviate;
    }

    public void DeleteSelectedSpell()
    {
        WhoaPlayerProperties.Spells.SelfSpells.Remove(selectedIndex);
        deleteButton.enabled = false;
        spellName.text = "Select spell to view";
        spellAbbreviate.text = "";
    }
        
}
