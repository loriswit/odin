using System;
using Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private Character player;
    private TextMeshProUGUI distance;
    private Slider healthBar;
    private GameObject gameOverScreen;
    private TextMeshProUGUI gameOverText;

    private float origin;
    private int score;
    private float maxHealth;
    private bool gameOver;

    private void Awake()
    {
        player = FindObjectOfType<Player>().GetComponent<Character>();
        origin = player.transform.position.x;
        maxHealth = player.Health;

        distance = transform.Find("Canvas/Distance").GetComponent<TextMeshProUGUI>();
        healthBar = transform.Find("Canvas/HealthBar").GetComponent<Slider>();
        gameOverScreen = transform.Find("Canvas/GameOver").gameObject;
        gameOverText = gameOverScreen.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        score = (int) Math.Max(score, Math.Floor(player.transform.position.x - origin));
        distance.text = score + " m";

        healthBar.value = Math.Max(0, player.Health / maxHealth);

        if (!gameOver && player.Health <= 0)
        {
            gameOver = true;
            gameOverText.text += " " + score + " m";
            gameOverScreen.SetActive(true);
            distance.gameObject.SetActive(false);

            // slow game down
            Time.timeScale = 0.2f;
            // decrease fixed timestep to keep physics smooth
            Time.fixedDeltaTime *= Time.timeScale;
        }
    }
}
