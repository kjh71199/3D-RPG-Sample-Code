using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ³´ ÀÌµ¿ ÄÄÆ÷³ÍÆ®
public class SickleMovement : MonoBehaviour
{
    LineRenderer lineRenderer;

    [SerializeField] GameObject player;
    [SerializeField] float range;

    private Vector3 direction;

    public GameObject Player { get => player; set => player = value; }
    public float Range { get => range; set => range = value; }
    public Vector3 Direction { get => direction; set => direction = value; }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        if (Player != null)
            StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        float time = 0f;
        Vector3 start = Player.transform.position;
        Vector3 end = Player.transform.position + Direction * range + new Vector3(0f, 1f, 0f);
        float distance = Vector3.Distance(start, end);
        transform.position = end;
        transform.rotation = Quaternion.LookRotation(Direction, Vector3.up);
        
        lineRenderer.SetPosition(0, Player.transform.position + new Vector3(0f, 1f, 0f));
        lineRenderer.SetPosition(1, transform.position);

        while (time <= 0.3f)
        {
            time += Time.deltaTime;

            if (time > 0.25f)
            {
                if (time <= 0.3f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, start, 16f * distance * Time.deltaTime);
                    lineRenderer.SetPosition(1, transform.position);
                    yield return null;
                }
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
