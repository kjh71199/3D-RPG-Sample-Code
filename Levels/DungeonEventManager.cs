using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEventManager : MonoBehaviour
{
    private DungeonSpawner spawner;

    [SerializeField] private GameObject[] leftSpikes;
    [SerializeField] private GameObject[] rightSpikes;
    [SerializeField] private GameObject[] bossEntranceSpikes;
    [SerializeField] private GameObject[] bossRoomSpikes;

    [SerializeField] private LeftRoomFlame[] leftRoomFlames;
    [SerializeField] private ParticleSystem leftAltarFire;
    [SerializeField] private ParticleSystem rightAltarFire;

    [SerializeField] private GameObject bossPrefab;

    [SerializeField] private GameObject exitPortal;

    private bool isLeftRoomClear = false;
    private bool isRightRoomClear = false;

    public delegate void LeftRoomEnterDelegate();
    public static LeftRoomEnterDelegate leftRoomEnter;

    public delegate void RightRoomEnterDelegate();
    public static RightRoomEnterDelegate rightRoomEnter;

    public delegate void BossRoomEnterDelegate();
    public static BossRoomEnterDelegate bossRoomEnter;

    public delegate void BossRoomClearDelegate();
    public static BossRoomClearDelegate bossRoomClear;

    private WaitForSeconds flameWaits;
    private IEnumerator flameCoroutine;

    private void Awake()
    {
        flameWaits = new WaitForSeconds(3f);
    }

    private void Start()
    {
        spawner = GameObject.FindWithTag("Spawner").GetComponent<DungeonSpawner>();
    }

    private void OnEnable()
    {
        leftRoomEnter += OnLeftRoomEnter;
        rightRoomEnter += OnRightRoomEnter;
        bossRoomEnter += OnBossRoomEnter;
        bossRoomClear += OnBossRoomClear;
    }

    private void OnDisable()
    {
        leftRoomEnter -= OnLeftRoomEnter;
        rightRoomEnter -= OnRightRoomEnter;
        bossRoomEnter -= OnBossRoomEnter;
        bossRoomClear -= OnBossRoomClear;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            LeftRoomClear();
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            RightRoomClear();
        }
        else if (Input.GetKeyDown(KeyCode.Backslash))
        {
            bossRoomClear();
        }
    }

    private void OnLeftRoomEnter()
    {
        foreach (GameObject leftSpike in leftSpikes)
        {
            leftSpike.transform.position += new Vector3(0f, 2f, 0f);
        }
        SoundManager.Instance.PlayDungeonSound(SoundManager.DUNGEONSOUND.SPIKESUP);

        flameCoroutine = LeftRoomFlameCoroutine();
        StartCoroutine(flameCoroutine);

        StartCoroutine(LeftRoomSpawnStartCoroutine());
    }

    private void OnRightRoomEnter()
    {
        foreach (GameObject rightSpike in rightSpikes)
        {
            rightSpike.transform.position += new Vector3(0f, 2f, 0f);
        }
        SoundManager.Instance.PlayDungeonSound(SoundManager.DUNGEONSOUND.SPIKESUP);

        StartCoroutine(RightRoomSpawnStartCoroutine());
    }

    public void LeftRoomClear()
    {
        foreach (GameObject leftSpike in leftSpikes)
        {
            leftSpike.transform.position -= new Vector3(0f, 2f, 0f);
        }
        SoundManager.Instance.PlayDungeonSound(SoundManager.DUNGEONSOUND.SPIKESDOWN);

        if (flameCoroutine != null)
            StopCoroutine(flameCoroutine);

        foreach (LeftRoomFlame flame in leftRoomFlames)
        {
            flame.FlameOFF();
        }

        isLeftRoomClear = true;
        leftAltarFire.Play();

        if (isLeftRoomClear && isRightRoomClear)
            BothRoomClear();
    }

    public void RightRoomClear()
    {
        foreach (GameObject rightSpike in rightSpikes)
        {
            rightSpike.transform.position -= new Vector3(0f, 2f, 0f);
        }
        SoundManager.Instance.PlayDungeonSound(SoundManager.DUNGEONSOUND.SPIKESDOWN);

        isRightRoomClear = true;
        rightAltarFire.Play();

        if (isLeftRoomClear && isRightRoomClear)
            BothRoomClear();
    }

    private void BothRoomClear()
    {
        foreach (GameObject bossSpike in bossEntranceSpikes)
        {
            bossSpike.transform.position -= new Vector3(0f, 2f, 0f);
        }
    }

    private void OnBossRoomEnter()
    {
        foreach (GameObject bossSpike in bossRoomSpikes)
        {
            bossSpike.transform.position += new Vector3(0f, 2f, 0f);
        }
        SoundManager.Instance.PlayDungeonSound(SoundManager.DUNGEONSOUND.SPIKESUP);
        SoundManager.Instance.PlayBossSound(SoundManager.BOSSSOUND.SPAWN);
        SoundManager.Instance.PlayBGM(SoundManager.BACKGROUNDSOUND.BOSS);

        bossPrefab.gameObject.SetActive(true);
    }

    private void OnBossRoomClear()
    {
        StartCoroutine(BossClear());
    }

    private IEnumerator BossClear()
    {
        yield return new WaitForSeconds(2f);
        exitPortal.gameObject.SetActive(true);
        SoundManager.Instance.PlayDungeonSound(SoundManager.DUNGEONSOUND.PORTAL);
    }

    private IEnumerator LeftRoomFlameCoroutine()
    {
        while (true)
        {
            leftRoomFlames[0].FlameReady();
            leftRoomFlames[2].FlameReady();
            leftRoomFlames[1].FlameOFF();
            leftRoomFlames[3].FlameOFF();

            yield return flameWaits;

            leftRoomFlames[0].FlameON();
            leftRoomFlames[2].FlameON();
            leftRoomFlames[1].FlameReady();
            leftRoomFlames[3].FlameReady();
            SoundManager.Instance.PlayDungeonSound(SoundManager.DUNGEONSOUND.FLAME);

            yield return flameWaits;

            leftRoomFlames[0].FlameOFF();
            leftRoomFlames[2].FlameOFF();
            leftRoomFlames[1].FlameON();
            leftRoomFlames[3].FlameON();
            SoundManager.Instance.PlayDungeonSound(SoundManager.DUNGEONSOUND.FLAME);

            yield return flameWaits;

            leftRoomFlames[0].FlameOFF();
            leftRoomFlames[2].FlameOFF();
            leftRoomFlames[1].FlameOFF();
            leftRoomFlames[3].FlameOFF();

            yield return flameWaits;
        }
    }

    private IEnumerator LeftRoomSpawnStartCoroutine()
    {
        yield return new WaitForSeconds(3f);
        spawner.LeftRoomSpawn();
    }

    private IEnumerator RightRoomSpawnStartCoroutine()
    {
        yield return new WaitForSeconds(3f);
        spawner.RightRoomSpawn();
    }
}
