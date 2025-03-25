using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkeletonProjectile : MonoBehaviour
{
    [SerializeField] protected Vector3 direction;
    [SerializeField] protected float speed;
    [SerializeField] protected float maxDistance;

    protected int damage;
    protected Vector3 start;

    public int Damage { get => damage; set => damage = value; }
    public Vector3 Direction { get => direction; set => direction = value; }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            health.Hit(Damage, 0f, Direction, Enums.CROWDCONTROL.NONE);
            gameObject.SetActive(false);
        }
    }
}
