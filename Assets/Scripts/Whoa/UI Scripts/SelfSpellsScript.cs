using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using System;
using Aspects.Self;

public class SelfSpellsScript : MonoBehaviour
{
    public GameObject spellLinesParent;
    public GameObject selfSpellAspectRowsParent;
    public GameObject spellPrefab;
    public GameObject selfSpellAspectRowPrefab;
    public GameObject spellDetailsParent;
    public Button deleteButton;
    public Text spellName;
    public Text spellAbbreviate;

    public Slots SlotButtonsManager;
    public Spells SpellButtonManager;

    public ScrollRect scrollRect;

    List<GameObject> selfSpellAspectRows;
    int selectedIndex;
    SelfSpell selectedSpell;
    GameObject infoRow;

    void Start()
    {
        selfSpellAspectRows = new List<GameObject>();

        SlotButtonsManager.Start();
        SpellButtonManager.Start(spellPrefab.GetComponent<Button>().image.color);

        GenerateSpellLinesGameObjects();

        infoRow = (GameObject)Instantiate(selfSpellAspectRowPrefab);
        RectTransform rectTransform = infoRow.GetComponent<RectTransform>();
        rectTransform.SetParent(selfSpellAspectRowsParent.transform);
        rectTransform.localScale = new Vector3(1, 1, 1);
        rectTransform.anchoredPosition = new Vector3(13, -60);

        Text aspectName = infoRow.transform.FindChild("AspectName").gameObject.GetComponent<Text>();
        aspectName.fontStyle = FontStyle.Bold;
        aspectName.text = "Name";

        Text aspectDuration = infoRow.transform.FindChild("AspectDuration").gameObject.GetComponent<Text>();
        aspectDuration.fontStyle = FontStyle.Bold;
        aspectDuration.text = "Duration";

        Text aspectAmplifier = infoRow.transform.FindChild("AspectAmplifier").gameObject.GetComponent<Text>();
        aspectAmplifier.fontStyle = FontStyle.Bold;
        aspectAmplifier.text = "Amplifier";

        if (WhoaPlayerProperties.Spells.SelfSpells.Count > 0)
        {
            selectedSpell = WhoaPlayerProperties.Spells.SelfSpells.First().Value;
            RefreshSpellAspectRowsGameObjects();
        }

        if (WhoaPlayerProperties.Character.SelfSpellSlots > 0)
            SelectSlot(0);
    }

    private void RefreshSpellAspectRowsGameObjects()
    {
        foreach (GameObject row in selfSpellAspectRows)
            GameObject.Destroy(row);
        infoRow.SetActive(false);

        float counter = -105;
        foreach (SelfAspect aspect in selectedSpell.Aspects)
        {
            infoRow.SetActive(true);
            GameObject row = (GameObject)Instantiate(selfSpellAspectRowPrefab);
            RectTransform rectTransform = row.GetComponent<RectTransform>();
            rectTransform.SetParent(selfSpellAspectRowsParent.transform);
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchoredPosition = new Vector3(16, counter);

            Text aspectName = row.transform.FindChild("AspectName").gameObject.GetComponent<Text>();
            aspectName.text = aspect.Name;

            Text aspectDuration = row.transform.FindChild("AspectDuration").gameObject.GetComponent<Text>();
            if (aspect.ExpensesMultiplierPerDuration == 1)
                aspectDuration.text = "None";
            else
                aspectDuration.text = aspect.Duration.ToString() + " s";

            Text aspectAmplifier = row.transform.FindChild("AspectAmplifier").gameObject.GetComponent<Text>();
            if (aspect.ExpensesMultiplierPerAmplifier == 1)
                aspectAmplifier.text = "None";
            else
                aspectAmplifier.text = aspect.Amplifier.ToString() + " (" + aspect.AmplifierName + ")";

            selfSpellAspectRows.Add(row);

            counter -= 45;
        }
    }

    private void GenerateSpellLinesGameObjects()
    {
        foreach (KeyValuePair<int, Button> button in SpellButtonManager.spellButtons)
            GameObject.Destroy(button.Value.gameObject);
        SpellButtonManager.spellButtons.Clear();

        float counter = 0;
        foreach (KeyValuePair<int, SelfSpell> spell in WhoaPlayerProperties.Spells.SelfSpells)
        {
            GameObject spellObject = (GameObject)Instantiate(spellPrefab);
            RectTransform rectTransform = spellObject.GetComponent<RectTransform>();
            rectTransform.SetParent(spellLinesParent.transform);
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchoredPosition = new Vector3(0, counter);

            Button button = spellObject.GetComponent<Button>();
            int index = spell.Key;
            button.onClick.AddListener(new UnityAction(() => SelectSpell(index)));
            SpellButtonManager.spellButtons.Add(spell.Key, button);

            Text text = spellObject.transform.FindChild("Name").gameObject.GetComponent<Text>();
            text.text = spell.Value.Name;

            Text klidText = spellObject.transform.FindChild("KlidCost").gameObject.GetComponent<Text>();
            klidText.text = spell.Value.GetKlidCost().FormatKlid();

            Button setButton = spellObject.transform.FindChild("SetButton").gameObject.GetComponent<Button>();
            if (WhoaPlayerProperties.Character.SelfSpellSlots == 0)
                setButton.interactable = false;
            else
                setButton.onClick.AddListener(new UnityAction(() => SetSpellToSelectedSlot(index)));

            counter -= 80;
        }

        RectTransform rekt = spellLinesParent.GetComponent<RectTransform>();
        rekt.sizeDelta = new Vector2(0, -counter);

        scrollRect.normalizedPosition = new Vector2(0, 1);
    }

    public void ClearActiveSlot()
    {
        SlotButtonsManager.ClearActiveSlot(this);
    }

    private void SelectSpell(int index)
    {
        selectedIndex = index;
        deleteButton.interactable = true;
        selectedSpell = WhoaPlayerProperties.Spells.SelfSpells[index];
        spellName.text = selectedSpell.Name;
        spellAbbreviate.text = selectedSpell.Abbreviate;
        RefreshSpellAspectRowsGameObjects();
        SpellButtonManager.SelectSpell(index, this);
    }

    private void SetSpellToSelectedSlot(int index)
    {
        try
        {
            SelectSpell(index);
            WhoaPlayerProperties.Character.Data.SelectedSelfSpellsIds[SlotButtonsManager.selectedIndex] = index;
            WhoaPlayerProperties.Character.Save();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

    }

    public void SelectSlot(int index)
    {
        SlotButtonsManager.SelectSlot(index, this);
    }

    private void DeselectSpell()
    {
        deleteButton.interactable = false;
        spellName.text = "Select spell to view";
        spellAbbreviate.text = "";
        selectedSpell = null;
        SpellButtonManager.DeselectSpell();
        RefreshSpellAspectRowsGameObjects();
    }

    public void DeleteSelectedSpell()
    {
        foreach (WhoaCharacter character in WhoaPlayerProperties.Characters.characters)
            if (character.Data.SelectedRangedSpellsIds.ContainsValue(selectedIndex))
                character.Data.SelectedSelfSpellsIds.Remove(character.Data.SelectedSelfSpellsIds.FirstOrDefault(x => x.Value == selectedIndex).Key);
        WhoaPlayerProperties.Spells.SelfSpells.Remove(selectedIndex);
        GenerateSpellLinesGameObjects();
        DeselectSpell();
        WhoaPlayerProperties.Spells.SaveSpells();
    }

    [Serializable]
    public class Spells
    {
        public Dictionary<int, Button> spellButtons { get; private set; }
        Color normalColor;
        public Color selectedColor;
        Button lastSelectedButton;

        public void Start(Color defaultColor)
        {
            normalColor = defaultColor;
            spellButtons = new Dictionary<int, Button>();
        }

        public void SelectSpell(int index, SelfSpellsScript script)
        {
            try
            {
                SelectButton(spellButtons[index]);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void DeselectSpell()
        {
            if (lastSelectedButton != null)
                DeselectButton(lastSelectedButton);
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
    }

    [Serializable]
    public class Slots
    {
        public List<Button> slotButtons;
        Color normalColor;
        public Color selectedColor;
        Button lastSelectedButton;

        public int selectedIndex { get; private set; }

        public void Start()
        {
            for (int spellSlotCount = WhoaPlayerProperties.Character.SelfSpellSlots; spellSlotCount > 0; spellSlotCount--)
                slotButtons[spellSlotCount - 1].interactable = true;
            normalColor = slotButtons.First().GetComponent<Button>().image.color;
        }


        public void SelectSlot(int index, SelfSpellsScript script)
        {
            try
            {
                selectedIndex = index;
                SelectButton(slotButtons[index]);
                if (WhoaPlayerProperties.Character.Data.SelectedSelfSpellsIds.ContainsKey(index))
                    script.SelectSpell(WhoaPlayerProperties.Character.Data.SelectedSelfSpellsIds[index]);
                else
                    script.DeselectSpell();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void ClearActiveSlot(SelfSpellsScript script)
        {
            script.DeselectSpell();
            WhoaPlayerProperties.Character.Data.SelectedSelfSpellsIds.Remove(selectedIndex);
            WhoaPlayerProperties.Character.Save();
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
    }
}
