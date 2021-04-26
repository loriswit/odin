using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField]
    private Transform entry;

    [SerializeField]
    private Transform exit;

    public Transform Entry => entry;
    public Transform Exit => exit;
}
