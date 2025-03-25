using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Highlighters;

public class LeftRoomFlame : MonoBehaviour
{
    [SerializeField] private ParticleSystem flameParticle;

    private Highlighter highlighter;
    private Collider flameCollider;
    private float time;

    private void Awake()
    {
        highlighter = GetComponent<Highlighter>();
        flameCollider = GetComponent<Collider>();
    }

    void Start()
    {
        FlameOFF();
    }

    public void FlameOFF()
    {
        highlighter.enabled = false;
        flameCollider.enabled = false;
        flameParticle.Stop();
    }

    public void FlameReady()
    {
        highlighter.enabled = true;
        flameCollider.enabled = false;
    }

    public void FlameON()
    {
        highlighter.enabled = true;
        flameCollider.enabled = true;
        flameParticle.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            time += Time.deltaTime;

            if (time > 0.5f)
            {
                PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();
                health.Hit(10f, 0f, transform.position, Enums.CROWDCONTROL.NONE);
                time = 0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            time = 0f;
        }
    }
}
