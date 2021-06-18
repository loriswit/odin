using System.Collections.Generic;
using Characters;
using UnityEngine;

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
        var chunk = easy[Random.Range(0, easy.Count)];
        lastChunk = Instantiate(chunk, lastChunk.Exit.position - chunk.Entry.position, Quaternion.identity);
        checkpoints.Enqueue(lastChunk.Entry.position.x);
    }
}
