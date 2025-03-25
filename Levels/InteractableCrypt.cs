using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 묘지 입장 상호작용 컴포넌트
public class InteractableCrypt : InteractableObject
{
    public override void Interaction()
    {
        LoadingSceneManager.LoadNextScene("DungeonScene", new Vector3(2.5f, 0f, -2.5f), new Vector3(1f, 0f, 1f));
    }
}
