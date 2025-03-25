using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableCrypt : InteractableObject
{
    public override void Interaction()
    {
        LoadingSceneManager.LoadNextScene("DungeonScene", new Vector3(2.5f, 0f, -2.5f), new Vector3(1f, 0f, 1f));
    }
}
