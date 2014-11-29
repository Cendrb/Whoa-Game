using UnityEngine;
using System.Collections;

public class DataManagerScript : MonoBehaviour
{
    private void Start()
    {
        Debug.Log(WhoaPlayerProperties.Money);
    }

    public void WipeCharactersData()
    {
        WhoaPlayerProperties.Characters.WipeCharactersData();
    }

    public void ReloadFromDrive()
    {
        WhoaPlayerProperties.ReloadFromDrive();
    }
}
