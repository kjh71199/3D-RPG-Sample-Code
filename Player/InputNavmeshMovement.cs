using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 타겟 터치 위치 이동 컴포넌트
public class InputNavmeshMovement : MonoBehaviour
{
    private static InputNavmeshMovement instance;

    private Animator animator;
    private NavMeshAgent agent;
    private Stats stats;
    private PlayerInputController controller;

    // 마우스 터치 포인트 위치
    [SerializeField] private Vector3 movePosition;

    public Vector3 MovePosition { get => movePosition; set => movePosition = value; }
    public NavMeshAgent Agent { get => agent; set => agent = value; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<Stats>();
        controller = GetComponent<PlayerInputController>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MovePosition = transform.position;

        Agent.angularSpeed = stats.rotateSpeed;
        Agent.speed = stats.moveSpeed;
    }

    void Update()
    {
        Agent.speed = stats.moveSpeed;

        if (controller.IsRotationLocked)
            Agent.angularSpeed = 0f;
        else
            Agent.angularSpeed = stats.rotateSpeed;

        if (controller.IsMovementLocked)
            Agent.isStopped = true;
        else
            Agent.isStopped = false;

        if (Agent.velocity.magnitude >= 0.05f)
            animator.SetFloat("Move", 1f);
        else
            animator.SetFloat("Move", 0f);
    }

    public void SetAgentDestination(Vector3 destination)
    {
        Agent.SetDestination(destination);
    }
}
