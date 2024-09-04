using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;
using Random = UnityEngine.Random;
public class Centipede : MonoBehaviour
{
    [HideInInspector]
    public List<CentipedeSection> sections = new List<CentipedeSection>();
    public GameObject HudCanvas;
    public GameManager GameManager;

    public CentipedeSection SectionPrefab;
    public Sprite HeadSprite;
    public Sprite BodySprite;
    public int PedeLength = 10;
    public float CentiSpeed = 2f;

    public LayerMask CollisionMask;
    public BoxCollider2D HomeBound;
    public Barrier BarrierPrefab;
    public event Action OnTargetDestroy;
    private bool NextLevel = false;
    public AudioSource soundSectionDestroyed;
    public Shake screenShake;
    private void Start()
    {
        Respawn();
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.ReturnScore() > 0 && NextLevel == false)
        {
            NextLevel = true;
            StartCoroutine(HudCanvas.GetComponent<HudController>().NextLevel());
        }
    }

    public void Respawn()
    {
        foreach (CentipedeSection section in sections)
        {
            Destroy(section.gameObject);
        }

        sections.Clear();

        for (int i = 0; i < PedeLength; i++)
        {
            Vector2 pos = GridPosition(transform.position) + (Vector2.left * i);
            CentipedeSection section = Instantiate(SectionPrefab, pos, Quaternion.identity);

            if(i == 0)
                section.SRenderer.sprite = HeadSprite;
            else
                section.SRenderer.sprite = BodySprite;
            section.Centipede = this;
            sections.Add(section);

            section.gameObject.GetComponent<Animator>().SetBool("Head", section.Ahead == null);
        }

        for (int i = 0; i < sections.Count; i++) 
        { 
            CentipedeSection section = sections[i];
            section.Ahead = GetSectionAt(i - 1);
            section.Behind = GetSectionAt(i + 1);
        }
    }

    public void RemoveSectionAndSpawn(CentipedeSection section)
    {
        Vector3 pos = GridPosition(section.transform.position);
        Instantiate(BarrierPrefab, pos, Quaternion.identity);

        if (section.Ahead != null)
        {
            section.Ahead.Behind = null;
        }
        if(section.Behind != null)
        {
            section.Behind.Ahead = null;
            section.Behind.SRenderer.sprite = HeadSprite;
            section.Behind.GetComponent<Animator>().SetBool("Head", section.Ahead == null);
            section.Behind.UpdateHeadSection();
        }

        RemoveSection(section);
    }

    /// <summary>
    /// Remove Sections only, won't spawn any barriers.
    /// </summary>
    /// <param name="section">A Centipede Section class.</param>
    public void RemoveSection(CentipedeSection section) 
    {
        sections.Remove(section);
        if (section.isLockedOn)     // Clear the target marker if this is the target
        {
            OnTargetDestroy?.Invoke();
            section.isLockedOn = false;

        }
        section.gameObject.GetComponent<Collider2D>().enabled = false;
        section.gameObject.GetComponent<Animator>().SetTrigger("Die");
        section.Dying = true;

        if (sections.Count <= 0)
            HudCanvas.GetComponent<HudController>().Lose();
        soundSectionDestroyed.Play();
        StartCoroutine(screenShake.Shaking());
    }
    private CentipedeSection GetSectionAt(int index)
    {
        if(index >= 0 && index < sections.Count)
            return sections[index];
        else
            return null;
    }

    private Vector2 GridPosition(Vector2 position)
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        return position;
    }

    /// <summary>
    /// Return a random section from centipede(temporary).
    /// </summary>
    public CentipedeSection GetRandomSection() {
        int length = sections.Count;
        return GetSectionAt(Random.Range(0, length));
    }

}