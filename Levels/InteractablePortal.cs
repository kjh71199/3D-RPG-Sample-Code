using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Å»Ãâ Æ÷Å» »óÈ£ÀÛ¿ë ÄÄÆ÷³ÍÆ®
public class InteractablePortal : InteractableObject
{
    public override void Interaction()
    {
        LoadingSceneManager.LoadNextScene("OutsideScene", new Vector3(21f, 0f, 21f), new Vector3(-1f, 0f, 1f));
    }

}
