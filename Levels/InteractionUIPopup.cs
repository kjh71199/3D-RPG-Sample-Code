using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUIPopup : MonoBehaviour
{
    [SerializeField] private Collider playerCollider;
    [SerializeField] private Image interactImage;

    private void Start()
    {
        StartCoroutine(UIPopupCoroutine());
    }

    private IEnumerator UIPopupCoroutine()
    {
        while (true)
        {
            if (playerCollider != null)
            {
                interactImage.enabled = true;
            }
            else
            {
                interactImage.enabled = false;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            playerCollider = other.GetComponent<Collider>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            playerCollider = null;
        }
    }
}
