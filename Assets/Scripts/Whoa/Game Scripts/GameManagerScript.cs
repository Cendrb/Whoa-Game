using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public PlayerScript.CollectiblesPrefabs collectibles;

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

    public Transform followers;

    public ObstacleGeneratorScript obstacleGenerator;

    GameObject player;
    PlayerScript playerScript;

    private void Awake()
    {
        player = (GameObject)Instantiate(WhoaPlayerProperties.Character.Prefab);
        player.tag = "Player";
        playerScript = player.GetComponent<PlayerScript>();
        playerScript.ActiveEffectPrefab = activeEffectPrefab;
        playerScript.ActiveEffectsContainer = activeEffectsContainer;
        playerScript.CanvasParent = canvasParent;
        playerScript.CastSelfSpellButtonPrefab = castSelfSpellButtonPrefab;
        playerScript.Collectibles = collectibles;
        playerScript.CollectiblesGenerator = collectiblesGenerator;
        playerScript.EnoughKlidToCastColor = enoughKlidToCastColor;
        playerScript.Follower = followers;
        playerScript.HealthSlider = healthSlider;
        playerScript.HealthText = healthText;
        playerScript.InsufficientKlidToCastColor = insufficientKlidToCastColor;
        playerScript.KlidSlider = klidSlider;
        playerScript.KlidText = klidText;
        playerScript.ScoreText = scoreText;

        obstacleGenerator.playerScript = playerScript;
        obstacleGenerator.playerTransform = player.transform;
    }

    private void Update()
    {

    }
}
