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

    private float origin;
    private float score;
    private float maxHealth;

    private void Awake()
    {
        player = FindObjectOfType<Player>().GetComponent<Character>();
        origin = player.transform.position.x;
        maxHealth = player.Health;

        distance = transform.Find("Canvas/Distance").GetComponent<TextMeshProUGUI>();
        healthBar = transform.Find("Canvas/HealthBar").GetComponent<Slider>();
    }

    private void Update()
    {
        score = (float) Math.Max(score, Math.Floor(player.transform.position.x - origin));
        distance.text = score + " m";

        healthBar.value = player.Health / maxHealth;
    }
}
