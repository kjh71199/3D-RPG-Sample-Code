using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ż�� ��Ż ��ȣ�ۿ� ������Ʈ
public class InteractablePortal : InteractableObject
{
    public override void Interaction()
    {
        LoadingSceneManager.LoadNextScene("OutsideScene", new Vector3(21f, 0f, 21f), new Vector3(-1f, 0f, 1f));
    }

}
