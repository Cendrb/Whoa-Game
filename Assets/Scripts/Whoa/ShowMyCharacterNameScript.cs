﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowMyCharacterNameScript : MonoBehaviour
{

    public Text CharacterNameText;

    // Use this for initialization
    void Start()
    {
        CharacterNameText.text = WhoaPlayerProperties.Character.Name;
    }
}
