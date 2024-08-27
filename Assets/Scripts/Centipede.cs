using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede : MonoBehaviour
{
    private List<CentipedeSection> sections = new List<CentipedeSection>();

    public CentipedeSection SectionPrefab;
    public Sprite HeadSprite;
    public Sprite BodySprite;
    public int PedeLength = 10;
    public float CentiSpeed = 2f;

    public LayerMask CollisionMask;
    public BoxCollider2D HomeBound;
    public Barrier BarrierPrefab;

    private void Start()
    {
        Respawn();
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
        }

        for (int i = 0; i < sections.Count; i++) 
        { 
            CentipedeSection section = sections[i];
            section.Ahead = GetSectionAt(i - 1);
            section.Behind = GetSectionAt(i + 1);
        }
    }

    public void RemoveSection(CentipedeSection section)
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
            section.Behind.UpdateHeadSection();
        }

        sections.Remove(section);
        Destroy(section.gameObject);
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
}