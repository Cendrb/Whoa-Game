using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Aspects.Self;

public class SelfAspectValueChangerScript : MonoBehaviour
{
    public Text ADCost;
    public Text KlidCost;

    public Text ListADCost { get; set; }
    public Text ListKlidCost { get; set; }
    public SelfAspect Aspect { get; set; }
    public NewSelfSpellScript Script { get; set; }

    private void Start()
    {

    }

    public void OnDurationChanged(float value)
    {
        if (Aspect != null)
        {
            Aspect.Duration = (int)value;
            SetLabels();
        }
        else
            Debug.Log("Refreshing duration failed. Aspect is null.");
    }

    public void OnAmplifierChanged(float value)
    {
        if (Aspect != null)
        {
            Aspect.Amplifier = (int)value;
            SetLabels();
        }
        else
            Debug.Log("Refreshing amplifier failed. Aspect is null.");
    }

    public void SetLabels()
    {
        ADCost.text = Aspect.GetPrice().FormatAD();
        KlidCost.text = Aspect.GetKlidCost().FormatKlid();

        if (ListADCost != null && ListKlidCost != null)
        {
            ListADCost.text = Aspect.GetPrice().FormatAD();
            ListKlidCost.text = Aspect.GetKlidCost().FormatKlid();
        }
        Script.RefreshCostLabels();
    }
}
