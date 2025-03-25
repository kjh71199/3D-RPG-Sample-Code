using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// 던전 방 입장 시 몬스터 소환 컴포넌트
public class DungeonSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] monsterPrefabs;

    [SerializeField] Transform[] leftSpawnTransforms;
    [SerializeField] Transform[] rightSpawnTransforms;

    [SerializeField] List<GameObject> monsterList;

    private DungeonEventManager eventManager;
    private WaitForSeconds spawnWaits;
    private WaitForSeconds checkWaits;

    private Coroutine leftCheckCoroutine;
    private Coroutine rightCheckCoroutine;

    private void Awake()
    {
        eventManager = GetComponentInParent<DungeonEventManager>();
        spawnWaits = new WaitForSeconds(5f);
        checkWaits = new WaitForSeconds(3f);
    }

    public void LeftRoomSpawn()
    {
        StartCoroutine(LeftRoomSpawnCoroutine());
    }

    public void RightRoomSpawn()
    {
        StartCoroutine(RightRoomSpawnCoroutine());
    }

    private IEnumerator LeftRoomSpawnCoroutine()
    {
        for (int i = 0; i < 5; i++)
        {
            int transform = Random.Range(0, leftSpawnTransforms.Length);
            GameObject spawn = Instantiate(monsterPrefabs[0], leftSpawnTransforms[transform]);
            spawn.GetComponent<MonsterFSMInfo>().DetectDistance = 50f;
            spawn.GetComponent<MonsterDeathState>().IsSpawned = true;
            monsterList.Add(spawn);
        }

        yield return spawnWaits;

        for (int i = 0; i < 5; i++)
        {
            int transform = Random.Range(0, leftSpawnTransforms.Length);
            int monster = Random.Range(0, 2);

            if (i < 2) monster = 0;

            GameObject spawn = Instantiate(monsterPrefabs[monster], leftSpawnTransforms[transform]);
            spawn.GetComponent<MonsterFSMInfo>().DetectDistance = 50f;
            spawn.GetComponent<MonsterDeathState>().IsSpawned = true;
            monsterList.Add(spawn);
        }

        yield return spawnWaits;

        for (int i = 0; i < 5; i++)
        {
            int transform = Random.Range(0, leftSpawnTransforms.Length);
            int monster = Random.Range(0, 2);

            if (i < 2) monster = 0;

            GameObject spawn = Instantiate(monsterPrefabs[monster], leftSpawnTransforms[transform]);
            spawn.GetComponent<MonsterFSMInfo>().DetectDistance = 50f;
            spawn.GetComponent<MonsterDeathState>().IsSpawned = true;
            monsterList.Add(spawn);
        }

        yield return spawnWaits;

        for (int i = 0; i < 5; i++)
        {
            int transform = Random.Range(0, leftSpawnTransforms.Length);
            int monster = Random.Range(0, 3);

            if (i < 2) monster = 0;

            GameObject spawn = Instantiate(monsterPrefabs[monster], leftSpawnTransforms[transform]);
            spawn.GetComponent<MonsterFSMInfo>().DetectDistance = 50f;
            spawn.GetComponent<MonsterDeathState>().IsSpawned = true;
            monsterList.Add(spawn);
        }

        yield return spawnWaits;

        for (int i = 0; i < 5; i++)
        {
            int transform = Random.Range(0, leftSpawnTransforms.Length);
            int monster = Random.Range(0, 3);

            if (i < 2) monster = 0;

            GameObject spawn = Instantiate(monsterPrefabs[monster], leftSpawnTransforms[transform]);
            spawn.GetComponent<MonsterFSMInfo>().DetectDistance = 50f;
            spawn.GetComponent<MonsterDeathState>().IsSpawned = true;
            monsterList.Add(spawn);
        }

        leftCheckCoroutine = StartCoroutine(LeftMonsterCheckCoroutine());
    }

    private IEnumerator RightRoomSpawnCoroutine()
    {
        for (int i = 0; i < 5; i++)
        {
            int transform = Random.Range(0, rightSpawnTransforms.Length);
            int monster = Random.Range(0, 2);
            GameObject spawn = Instantiate(monsterPrefabs[0], rightSpawnTransforms[transform]);
            spawn.GetComponent<MonsterFSMInfo>().DetectDistance = 50f;
            spawn.GetComponent<MonsterDeathState>().IsSpawned = true;
            monsterList.Add(spawn);
        }

        yield return spawnWaits;

        for (int i = 0; i < 5; i++)
        {
            int transform = Random.Range(0, rightSpawnTransforms.Length);
            int monster = Random.Range(0, 3);

            if (i < 2) monster = 0;

            GameObject spawn = Instantiate(monsterPrefabs[monster], rightSpawnTransforms[transform]);
            spawn.GetComponent<MonsterFSMInfo>().DetectDistance = 50f;
            spawn.GetComponent<MonsterDeathState>().IsSpawned = true;
            monsterList.Add(spawn);
        }

        yield return spawnWaits;

        for (int i = 0; i < 5; i++)
        {
            int transform = Random.Range(0, rightSpawnTransforms.Length);
            int monster = Random.Range(0, 3);

            if (i < 2) monster = 0;

            GameObject spawn = Instantiate(monsterPrefabs[monster], rightSpawnTransforms[transform]);
            spawn.GetComponent<MonsterFSMInfo>().DetectDistance = 50f;
            spawn.GetComponent<MonsterDeathState>().IsSpawned = true;
            monsterList.Add(spawn);
        }

        yield return spawnWaits;

        for (int i = 0; i < 5; i++)
        {
            int transform = Random.Range(0, rightSpawnTransforms.Length);
            int monster = Random.Range(0, 4);

            if (i < 2) monster = 3;

            GameObject spawn = Instantiate(monsterPrefabs[monster], rightSpawnTransforms[transform]);
            spawn.GetComponent<MonsterFSMInfo>().DetectDistance = 50f;
            spawn.GetComponent<MonsterDeathState>().IsSpawned = true;
            monsterList.Add(spawn);
        }

        yield return spawnWaits;

        for (int i = 0; i < 5; i++)
        {
            int transform = Random.Range(0, rightSpawnTransforms.Length);
            int monster = Random.Range(0, 4);

            if (i < 2) monster = 3;

            GameObject spawn = Instantiate(monsterPrefabs[monster], rightSpawnTransforms[transform]);
            spawn.GetComponent<MonsterFSMInfo>().DetectDistance = 50f;
            spawn.GetComponent<MonsterDeathState>().IsSpawned = true;
            monsterList.Add(spawn);
        }

        rightCheckCoroutine = StartCoroutine(RightMonsterCheckCoroutine());
    }

    public void MonsterDeathCount(GameObject gameObject)
    {
        monsterList.Remove(gameObject);
    }

    private IEnumerator LeftMonsterCheckCoroutine()
    {
        while (true)
        {
            if (monsterList.Count == 0)
            {
                eventManager.LeftRoomClear();
                StopCoroutine(leftCheckCoroutine);
            }
            Debug.Log("LeftCheck");
            yield return checkWaits;
        }
    }

    private IEnumerator RightMonsterCheckCoroutine()
    {
        while (true)
        {
            if (monsterList.Count == 0)
            {
                eventManager.RightRoomClear();
                StopCoroutine(rightCheckCoroutine);
            }
            Debug.Log("RightCheck");
            yield return checkWaits;
        }
    }
}
