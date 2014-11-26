﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using System;

public class SpellsScript : MonoBehaviour
{
    public GameObject spellsParent;
    public GameObject spellPrefab;
    public GameObject spellDetailsParent;
    public Button deleteButton;
    public Text spellName;
    public Text spellAbbreviate;

    public Slots SlotButtonsManager;
    public Spells SpellButtonManager;


    int selectedIndex;
    SelfSpell selectedSpell;

    void Start()
    {
        SlotButtonsManager.Start();
        SpellButtonManager.Start();

        Debug.Log(WhoaPlayerProperties.Spells.SelfSpellIdCounter);

        GenerateSpellGameobjects();

        if (WhoaPlayerProperties.Character.SelfSpellSlots > 0)
            SelectSlot(0);
    }

    private void GenerateSpellGameobjects()
    {
        foreach (KeyValuePair<int, Button> button in SpellButtonManager.spellButtons)
            GameObject.Destroy(button.Value.gameObject.transform.parent.gameObject);
        SpellButtonManager.spellButtons.Clear();

        float counter = 0;
        foreach (KeyValuePair<int, SelfSpell> spell in WhoaPlayerProperties.Spells.SelfSpells)
        {
            GameObject spellObject = (GameObject)Instantiate(spellPrefab);
            RectTransform rectTransform = spellObject.GetComponent<RectTransform>();
            rectTransform.parent = spellsParent.transform;
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchoredPosition = new Vector3(0, counter);

            Button button = spellObject.GetComponent<Button>();
            int index = spell.Key;
            button.onClick.AddListener(new UnityAction(() => SelectSpellButton(index)));
            SpellButtonManager.spellButtons.Add(spell.Key, button);

            Text text = spellObject.transform.FindChild("Name").gameObject.GetComponent<Text>();
            text.text = spell.Value.Name;

            Text klidText = spellObject.transform.FindChild("KlidCost").gameObject.GetComponent<Text>();
            klidText.text = spell.Value.GetKlidCost(true).ToString();

            Button setButton = spellObject.transform.FindChild("SetButton").gameObject.GetComponent<Button>();
            if (WhoaPlayerProperties.Character.SelfSpellSlots == 0)
                setButton.interactable = false;
            else
                setButton.onClick.AddListener(new UnityAction(() => SetSpellToSelectedSlot(spell.Key)));

            counter -= 80;
        }

        RectTransform rekt = spellsParent.GetComponent<RectTransform>();
        rekt.sizeDelta = new Vector2(0, -counter);
        rekt.anchoredPosition = new Vector2(0, 0);
    }

    private void SelectSpell(int index)
    {
        selectedIndex = index;
        deleteButton.interactable = true;
        SelfSpell selectedSpell = WhoaPlayerProperties.Spells.SelfSpells[index];
        spellName.text = selectedSpell.Name;
        spellAbbreviate.text = selectedSpell.Abbreviate;
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

    public void SelectSpellButton(int index)
    {
        SpellButtonManager.SelectSpell(index, this);
    }

    private void DeselectSpell()
    {
        deleteButton.interactable = false;
        spellName.text = "Select spell to view";
        spellAbbreviate.text = "";
    }

    public void CreateNewSpell()
    {
        SelfSpell spell = new SelfSpell();
        spell.Abbreviate = "BUM";
        spell.Name = "Zeman";
        spell.Aspects.Add(WhoaPlayerProperties.AspectsTemplates.SelfAspectsTemplates[0].GetAspect());
        spell.Aspects.Add(WhoaPlayerProperties.AspectsTemplates.SelfAspectsTemplates[2].GetAspect());
        spell.GenerateEffects();
        WhoaPlayerProperties.Spells.AddSelfSpell(spell);
        WhoaPlayerProperties.Spells.SaveSpells();
        GenerateSpellGameobjects();
    }

    public void DeleteSelectedSpell()
    {
        WhoaPlayerProperties.Spells.SelfSpells.Remove(selectedIndex);
        GenerateSpellGameobjects();
        DeselectSpell();
        WhoaPlayerProperties.Spells.SaveSpells();
    }

    [Serializable]
    public class Spells
    {
        public Dictionary<int, Button> spellButtons { get; private set; }
        public Color normalColor;
        public Color selectedColor;
        Button lastSelectedButton;

        public void Start()
        {
            spellButtons = new Dictionary<int, Button>();
        }

        public void SelectSpell(int index, SpellsScript script)
        {
            try
            {
                script.SelectSpell(index);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
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


        public void SelectSlot(int index, SpellsScript script)
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
