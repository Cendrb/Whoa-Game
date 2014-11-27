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

    List<SelfAspectTemplate> templates = new List<SelfAspectTemplate>();
    List<SelfAspect> aspects = new List<SelfAspect>();
    List<GameObject> aspectsList = new List<GameObject>();

    GameObject detailsWindowContent;
    GameObject basicInformations;

    void Start()
    {
        basicInformations = (GameObject)Instantiate(BasicInformationPrefab);
        RectTransform infrectTransform = basicInformations.GetComponent<RectTransform>();
        infrectTransform.parent = AspectDetails.transform;
        infrectTransform.localScale = new Vector3(1, 1, 1);
        infrectTransform.anchoredPosition = new Vector2(8, -7);
        basicInformations.SetActive(false);

        int indexCounter = 0;
        int counter = 7;
        foreach (SelfAspectTemplate template in WhoaPlayerProperties.AspectsTemplates.SelfAspectsTemplates)
        {
            if (true || template.Data.Bought)
            {
                GameObject templateObject = (GameObject)Instantiate(AvailableAspectPrefab);
                RectTransform rectTransform = templateObject.GetComponent<RectTransform>();
                rectTransform.parent = AvailableAspects.transform;
                rectTransform.localScale = new Vector3(1, 1, 1);
                rectTransform.anchoredPosition = new Vector3(counter, -4);

                Image image = rectTransform.FindChild("Image").GetComponent<Image>();
                image.sprite = template.Sprite;

                Button button = templateObject.GetComponent<Button>();
                int index = indexCounter;
                button.onClick.AddListener(new UnityAction(() => AvailableAspectClicked(index)));

                counter += 97;
                indexCounter++;
            }
        }
    }

    private void GenerateAspectsList()
    {
        foreach (GameObject go in aspectsList)
            GameObject.Destroy(go);
        aspectsList.Clear();

        int counter = -80;
        foreach (SelfAspect aspect in aspects)
        {
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
            constructionCost.text = aspect.GetPrice().ToString() + " AD";

            Text castCost = rectTransform.FindChild("CastCost").GetComponent<Text>();
            castCost.text = aspect.GetKlidCost().ToString() + " K";

            aspectsList.Add(templateObject);

            counter -= 80;
        }
    }

    private void AvailableAspectClicked(int indexCounter)
    {
        try
        {
            templates.Add(WhoaPlayerProperties.AspectsTemplates.SelfAspectsTemplates[indexCounter]);
            aspects.Add(WhoaPlayerProperties.AspectsTemplates.SelfAspectsTemplates[indexCounter].GetAspect());
            GenerateAspectsList();
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void OnBasicInformationClicked()
    {
        basicInformations.SetActive(true);
    }
}
