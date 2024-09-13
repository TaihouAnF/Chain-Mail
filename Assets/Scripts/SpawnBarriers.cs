using System.CodeDom.Compiler;
using UnityEngine;

public class SpawnBarriers : MonoBehaviour
{
    private BoxCollider2D spawnArea;
    public Barrier B;
    public int SpawnNum = 50;

    private void Awake()
    {
        spawnArea = GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    private void Generate()
    {
        Bounds bounds = spawnArea.bounds;
        for (int i = 0; i < SpawnNum; ++i)
        {
            Vector2 pos = Vector2.zero;

            pos.x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
            pos.y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

            Instantiate(B, pos, Quaternion.identity, transform);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
