using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 상호작용 가능 오브젝트 추상 클래스
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
