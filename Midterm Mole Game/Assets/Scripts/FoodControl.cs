using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodControl : MonoBehaviour
{
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
        NPCManager.Instance.RegisterFood(gameObject);
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
        Direction = Random.onUnitSphere;
        Direction = new Vector3(Direction.x, Direction.y, 0);
    }

    void MoveInDirection()
    {
        rb.AddForce(Direction * MovementSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) Killed();
    }

    void Killed()
    {
        ScoreController.Instance.AddPoints(PointValue);
        NPCManager.Instance.KillFood(gameObject);
        Destroy(gameObject);
    }
}
