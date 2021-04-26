using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField]
    private int length = 20;

    [SerializeField]
    private Chunk start;

    [SerializeField]
    private List<Chunk> chunks;

    private void Start()
    {
        var previous = start;
        for (var i = 0; i < length; ++i)
        {
            var chunk = chunks[Random.Range(0, chunks.Count)];
            previous = Instantiate(chunk, previous.Exit.position - chunk.Entry.position, Quaternion.identity);
        }
    }
}
