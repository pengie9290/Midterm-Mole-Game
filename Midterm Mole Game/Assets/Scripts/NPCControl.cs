using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControl : MonoBehaviour
{
    public bool isFood = true;

    public bool CanDig = true;
    public NPCDigger digger;
    public float DigDelayTime = 0.2f;
    public float DigTimer = 0;

    public float MoveDelayTime = 2f;
    public float MoveTimer = 0;
    public float MaxMoveTime = 5f;
    public float MinMoveTime = 1f;

    public float MovementSpeed = 1f;
    public Vector3 Direction = Vector3.zero;
    Rigidbody2D rb;

    public float PointValue = 5;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        DigTimer = DigDelayTime;
        ChooseDirection();
        SetMoveTimer();
        if (isFood) NPCManager.Instance.RegisterFood(gameObject);
        else NPCManager.Instance.RegisterEnemy(gameObject);
    }

    void SetMoveTimer()
    {
        MoveDelayTime = Random.Range(MinMoveTime, MaxMoveTime);
        MoveTimer = MoveDelayTime;
    }

    void Update()
    {
        if (DigTimer > 0)
        {
            DigTimer -= Time.deltaTime;
            if (DigTimer <= 0) digger.CanDig = CanDig;
        }
        if (MoveTimer > 0)
        {
            MoveInDirection();
            FaceInDirection();
            MoveTimer -= Time.deltaTime;
            if (MoveTimer <= 0)
            {
                ChooseDirection();
                SetMoveTimer();
            }
        }
        else
        {
            ChooseDirection();
            SetMoveTimer();
        }
    }

    void ChooseDirection()
    {
        if (!CanSeePlayer)
        {
            Direction = Random.onUnitSphere;
            Direction = new Vector3(Direction.x, Direction.y, 0);
        }
    }

    bool CanSeePlayer
    {
        get
        {
            Vector3 PlayerDirection = (PlayerControl.Instance.gameObject.transform.position - transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, PlayerDirection);
            if (hit)
            {
                GameObject found = hit.transform.gameObject;
                print(found.name);
                if (found.CompareTag("Player")) return true;
            }
            return false;
        }
    }

    void MoveInDirection()
    {
        if (CanSeePlayer)
        {
            Vector3 PerfectDirection = PlayerControl.Instance.transform.position - transform.position;
            PerfectDirection.Normalize();
            Direction = Vector3.Lerp(Direction, PerfectDirection, 0.1f);
        }
        rb.AddForce(Direction * MovementSpeed * Time.deltaTime);
    }

    void FaceInDirection()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, Direction);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PlayerControl.Instance.GameState == PlayerControl.GameStates.Playing && collision.gameObject.CompareTag("Player")) Killed();
    }

    void Killed()
    {
        ScoreController.Instance.AddPoints(PointValue);
        if (isFood) NPCManager.Instance.KillFood(gameObject);
        else NPCManager.Instance.KillEnemy(gameObject);
        Destroy(gameObject);
    }
}
