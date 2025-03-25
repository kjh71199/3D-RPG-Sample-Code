using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGolemFSMController : MonoBehaviour
{
    public enum STATE { IDLE, DETECT, ATTACK, SPECIALATTACK, SUMMON, STUN, DEATH }

    [SerializeField] private SkeletonGolemState currentState;

    [SerializeField] private SkeletonGolemState[] monsterStates;

    private GameObject player;

    public GameObject Player { get => player; set => player = value; }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        TransactionToState(STATE.IDLE);
    }

    private void Update()
    {
        currentState?.UpdateState();
    }

    public void TransactionToState(STATE state, object data = null)
    {
        if (currentState == monsterStates[(int)STATE.DEATH]) return;

        currentState?.ExitState();
        currentState = monsterStates[(int)state];
        currentState?.EnterState(state, data);
    }

    public float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, Player.transform.position);
    }
}
