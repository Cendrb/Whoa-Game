using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {

    public Slider healthSlider;
    public Slider klidSlider;

    public Text healthText;
    public Text klidText;
    public Text scoreText;

    public GameObject activeEffectPrefab;
    public GameObject activeEffectsContainer;
    public GameObject castSelfSpellButtonPrefab;
    public Color enoughKlidToCastColor;
    public Color insufficientKlidToCastColor;

    public GameObject canvasParent;

    public OnPlayerPassedExecutorScript collectiblesGenerator;

    public Canvas canvas;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
