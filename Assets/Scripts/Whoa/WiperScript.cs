using UnityEngine;
using System.Collections;

public class WiperScript : MonoBehaviour
{
    public void WipeCharacters()
    {
        WhoaPlayerProperties.Load();
        WhoaPlayerProperties.Characters.SetupCharacters();
        WhoaPlayerProperties.Save();
        WhoaPlayerProperties.Load();
    }
}
