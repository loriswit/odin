using System.Collections.Generic;
using System.Linq;
using Characters;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private List<Character> defenders;

    [Header("Opening")]
    [SerializeField]
    private Transform destination;

    [SerializeField]
    private float openingSpeed = 2;

    private Transform door;

    private bool opening;
    private bool open;

    private void Awake()
    {
        door = transform.Find("Door").transform;
    }

    private void Update()
    {
        if (open) return;

        // check if all defenders are dead
        if (!opening && defenders.All(defender => defender.Health <= 0))
            opening = true;

        if (opening)
        {
            var step = openingSpeed * Time.deltaTime;
            door.position = Vector3.MoveTowards(door.position, destination.position, step);
            // if (Vector2.Distance(transform.position, destination.position) <= step)
            // open = true;
        }
    }
}
