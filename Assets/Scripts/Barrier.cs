using UnityEngine;

public class Barrier : MonoBehaviour
{
    // For this to look correct the most damaged sprite comes first and full health sprite comes last
    public Sprite[] Sprites = { };
    private SpriteRenderer sRenderer;
    private int hp = 0;
    public AudioSource soundBarrierDestroyed;
    private void Awake()
    {

        sRenderer = GetComponent<SpriteRenderer>();
        sRenderer.sprite = Sprites[Sprites.Length - 1]; // Set the sprite at the end of the array as default
        hp = Sprites.Length;
    }

    private void Damage(int damage)
    {
        hp -= damage;
        if(hp > 0)
        {
            soundBarrierDestroyed.Play();
            sRenderer.sprite = Sprites[hp - 1];
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            Damage(1);
        }
    }
}
