using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    [SerializeField]
    private int length = 20;

    [SerializeField]
    private Chunk start;

    [Header("Chunks")]
    [SerializeField]
    private List<Chunk> easy;

    [SerializeField]
    private List<Chunk> medium;

    [SerializeField]
    private List<Chunk> hard;

    [SerializeField]
    private List<Chunk> rest;

    private Player player;
    private Chunk lastChunk;
    private readonly Queue<float> checkpoints = new Queue<float>();

    private int index;
    private int lastHard;
    private string lastChunkName;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        lastChunk = start;
    }

    private void Start()
    {
        for (var i = 0; i < length; ++i)
            AppendRandomChunk();
    }

    private void FixedUpdate()
    {
        if (player.transform.position.x > checkpoints.Peek())
        {
            checkpoints.Dequeue();
            AppendRandomChunk();
        }
    }

    private void AppendRandomChunk()
    {
        // algorithm that finds the best next chunk
        Chunk chunk;
        do
        {
            // always start with easy chunks
            if (index < 2)
                chunk = easy[Random.Range(0, easy.Count)];

            // easy-medium chunks
            else if (index - lastHard < 5)
            {
                if (Random.Range(0f, 1f) < 0.65)
                    // less rest as we progress
                    if (Random.Range(0f, 1f) > Math.Max(0.2, 0.5 - (float) index / 150))
                        chunk = easy[Random.Range(0, easy.Count)];
                    else
                        chunk = rest[Random.Range(0, rest.Count)];
                else
                    chunk = medium[Random.Range(0, medium.Count)];
            }

            // if we haven't got a hard chunk in a while
            else
            {
                var p = Random.Range(0f, 1f);
                if (p < 0.3)
                    // less rest as we progress
                    if (Random.Range(0f, 1f) > Math.Max(0.2, 0.3 - (float) index / 150))
                        chunk = easy[Random.Range(0, easy.Count)];
                    else
                        chunk = rest[Random.Range(0, rest.Count)];
                else if (p >= 0.3 && p < 0.6)
                    chunk = medium[Random.Range(0, medium.Count)];
                else
                {
                    chunk = hard[Random.Range(0, hard.Count)];
                    lastHard = index;
                }
            }
        } while (chunk.name == lastChunkName); // prevent getting the same chunk twice in a row

        index++;

        lastChunk = Instantiate(chunk, lastChunk.Exit.position - chunk.Entry.position, Quaternion.identity);
        lastChunkName = chunk.name;
        checkpoints.Enqueue(lastChunk.Entry.position.x);
    }
}
