using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Aspects.Self;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class NewSelfSpellScript : MonoBehaviour
{
    public GameObject AspectDetailsPrefab;
    public GameObject BasicInformationPrefab;
    public GameObject AvailableAspectPrefab;
    public GameObject AspectLinePrefab;

    public Text KlidCostPerCast;
    public Text ADCostForConstruction;

    public GameObject AspectDetails;
    public GameObject AspectsList;
    public GameObject AvailableAspects;

    //List<string> addedAspectsNames = new List<string>();
    Dictionary<string, SelfAspect> aspects = new Dictionary<string, SelfAspect>();
    List<GameObject> aspectsList = new List<GameObject>();
    Dictionary<string, GameObject> aspectsScreens = new Dictionary<string, GameObject>();

    GameObject basicInformation;
    GameObject lastScreen;

    InputField nameInput;
    InputField idInput;

    SelfSpell constructedSpell;

    void Start()
    {
        constructedSpell = new SelfSpell();

        basicInformation = (GameObject)Instantiate(BasicInformationPrefab);
        RectTransform infrectTransform = basicInformation.GetComponent<RectTransform>();
        infrectTransform.parent = AspectDetails.transform;
        infrectTransform.localScale = new Vector3(1, 1, 1);
        infrectTransform.anchoredPosition = new Vector2(8, -7);
        basicInformation.SetActive(false);
        nameInput = infrectTransform.FindChild("SpellName").gameObject.GetComponent<InputField>();
        idInput = infrectTransform.FindChild("SpellID").gameObject.GetComponent<InputField>();
        nameInput.onValueChange.AddListener(new UnityAction<string>((text) => constructedSpell.Name = text));
        idInput.onValueChange.AddListener(new UnityAction<string>((text) => constructedSpell.Abbreviate = text));

        int indexCounter = 0;
        int counter = 7;
        foreach (SelfAspectTemplate template in WhoaPlayerProperties.AspectsTemplates.SelfAspectsTemplates)
        {
            if (template.Data.Purchased)
            {
                GameObject templateObject = (GameObject)Instantiate(AvailableAspectPrefab);
                RectTransform rectTransform = templateObject.GetComponent<RectTransform>();
                rectTransform.parent = AvailableAspects.transform;
                rectTransform.localScale = new Vector3(1, 1, 1);
                rectTransform.anchoredPosition = new Vector3(counter, -4);

                Image image = rectTransform.FindChild("Image").GetComponent<Image>();
                image.sprite = template.Sprite;

                Button button = templateObject.GetComponent<Button>();
                SelfAspectTemplate templateCopy = template;
                button.onClick.AddListener(new UnityAction(() => OnAvailableAspectClicked(templateCopy)));

                counter += 97;
                indexCounter++;
            }
        }

        RefreshCostLabels();
    }

    private void GenerateAspectsList()
    {
        foreach (GameObject go in aspectsList)
            GameObject.Destroy(go);
        aspectsList.Clear();

        int indexCounter = 0;
        int counter = -80;
        foreach (KeyValuePair<string, SelfAspect> aspectPair in aspects)
        {
            SelfAspect aspect = aspectPair.Value;
            GameObject templateObject = (GameObject)Instantiate(AspectLinePrefab);
            RectTransform rectTransform = templateObject.GetComponent<RectTransform>();
            rectTransform.parent = AspectsList.transform;
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchoredPosition = new Vector3(0, counter);

            Image image = rectTransform.FindChild("Image").GetComponent<Image>();
            image.sprite = aspect.icon;

            Text name = rectTransform.FindChild("Name").GetComponent<Text>();
            name.text = aspect.Name;

            Text constructionCost = rectTransform.FindChild("ConstructionCost").GetComponent<Text>();
            constructionCost.text = aspect.GetPrice().FormatAD();

            Text castCost = rectTransform.FindChild("CastCost").GetComponent<Text>();
            castCost.text = aspect.GetKlidCost().FormatKlid();

            Button button = templateObject.GetComponent<Button>();
            SelfAspect aspectCopy = aspect;
            button.onClick.AddListener(new UnityAction(() => OnSelfAspectLineClicked(aspectCopy)));

            aspectsList.Add(templateObject);

            SelfAspectValueChangerScript changerScript = aspectsScreens[aspect.Name].GetComponent<SelfAspectValueChangerScript>();
            changerScript.ListADCost = constructionCost;
            changerScript.ListKlidCost = castCost;

            counter -= 80;
            indexCounter++;
        }
    }

    private void OnAvailableAspectClicked(SelfAspectTemplate template)
    {
        try
        {
            if (!aspects.ContainsKey(template.Name))
            {
                SelfAspect aspect = template.GetAspect();
                aspects.Add(template.Name, aspect);
                constructedSpell.Aspects.Add(aspect);

                if (!aspectsScreens.ContainsKey(template.Name))
                {
                    GameObject aspectScreenObject = (GameObject)Instantiate(AspectDetailsPrefab);
                    aspectScreenObject.SetActive(false);
                    RectTransform rectTransform = aspectScreenObject.GetComponent<RectTransform>();
                    rectTransform.parent = AspectDetails.transform;
                    rectTransform.localScale = new Vector3(1, 1, 1);
                    rectTransform.anchoredPosition = new Vector3(17, -16);

                    SelfAspectValueChangerScript script = aspectScreenObject.GetComponent<SelfAspectValueChangerScript>();
                    script.Aspect = aspect;
                    script.Script = this;
                    script.SetLabels();

                    Text name = rectTransform.FindChild("Text").gameObject.GetComponent<Text>();
                    name.text = template.Name;

                    Button removeButton = rectTransform.FindChild("RemoveButton").gameObject.GetComponent<Button>();
                    SelfAspect aspectCopy = aspect;
                    removeButton.onClick.AddListener(new UnityAction(() => OnRemoveButtonClick(aspectCopy)));

                    GameObject amplifier = rectTransform.FindChild("Amplifier").gameObject;
                    if (template.MinAmplifier != 0)
                    {
                        Text amplifierName = amplifier.GetComponent<Text>();
                        amplifierName.text = template.AmplifierName;

                        Slider amplifierSlider = amplifier.transform.FindChild("Slider").gameObject.GetComponent<Slider>();
                        amplifierSlider.value = template.DefaultAmplifier;
                        amplifierSlider.maxValue = template.MaxAmplifier;
                        amplifierSlider.minValue = template.MinAmplifier;
                    }
                    else
                        GameObject.Destroy(amplifier);

                    GameObject duration = rectTransform.FindChild("Duration").gameObject;
                    if (template.MinDuration != 0)
                    {
                        Slider durationSlider = duration.transform.FindChild("Slider").gameObject.GetComponent<Slider>();
                        durationSlider.value = template.DefaultDuration;
                        durationSlider.maxValue = template.MaxDuration;
                        durationSlider.minValue = template.MinDuration;
                    }
                    else
                    {
                        RectTransform transform = amplifier.GetComponent<RectTransform>();
                        transform.anchoredPosition = new Vector3(26, -66);
                        GameObject.Destroy(duration);
                    }

                    aspectsScreens[template.Name] = aspectScreenObject;
                }
                GenerateAspectsList();
                OnSelfAspectLineClicked(aspect);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void OnRemoveButtonClick(SelfAspect aspect)
    {
        SelfAspect removedAspect = aspects[aspect.Name];
        constructedSpell.Aspects.Remove(removedAspect);
        aspects.Remove(aspect.Name);
        aspectsScreens.Remove(aspect.Name);
        ClearDetailsScreen();
        GenerateAspectsList();
        RefreshCostLabels();
    }

    private void OnSelfAspectLineClicked(SelfAspect aspect)
    {
        if (lastScreen != null)
            lastScreen.SetActive(false);
        GameObject screen = aspectsScreens[aspect.Name];
        screen.SetActive(true);
        lastScreen = screen;
    }

    public void OnBasicInformationClicked()
    {
        ClearDetailsScreen();
        basicInformation.SetActive(true);
        lastScreen = basicInformation;
    }

    private void ClearDetailsScreen()
    {
        if (lastScreen != null)
            lastScreen.SetActive(false);
    }

    public void CreateSpell()
    {
        int cost = constructedSpell.GetKlidCost();
        if (cost <= WhoaPlayerProperties.Money)
        {
            WhoaPlayerProperties.Money -= cost;
            WhoaPlayerProperties.SavePrefs();
            WhoaPlayerProperties.Spells.AddSelfSpell(constructedSpell);
            WhoaPlayerProperties.Spells.SaveSpells();
            Application.LoadLevel("SelfSpells");
        }
    }

    public void RefreshCostLabels()
    {
        KlidCostPerCast.text = constructedSpell.GetKlidCost().FormatKlid() + " per cast";
        ADCostForConstruction.text = constructedSpell.GetADCost().FormatAD() + " for creating";
    }
}
