using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InteractableObject : MonoBehaviour
{
    private Collider interactableCollider;
    [SerializeField] protected Image image;

    protected virtual void Awake()
    {
        interactableCollider = GetComponent<Collider>();
    }

    public abstract void Interaction();


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            image.gameObject.SetActive(true);
            PlayerInputController controller = other.GetComponent<PlayerInputController>();
            controller.InteractCollider = interactableCollider;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            image.gameObject.SetActive(false);
            PlayerInputController controller = other.GetComponent<PlayerInputController>();
            controller.InteractCollider = null;
        }
    }
}
