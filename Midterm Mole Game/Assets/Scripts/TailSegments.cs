using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailSegments : MonoBehaviour
{
    public GameObject Head;
    public GameObject PreviousSegment;
    public Vector3 Direction = Vector3.zero;
    public float MovementSpeed = 1f;
    Rigidbody2D rb;

    private float _startingDistance;




    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (Head != null) Head.SendMessage("RegisterBossTailSegment", this.gameObject);
    }

    void Start()
    {
        _startingDistance = Vector3.Distance(transform.position, PreviousSegment.transform.position);
        CalculateDirection();
    }

    void Update()
    {
        CalculateDirection();
        FaceInDirection();
        MoveInDirection();
    }

    void CalculateDirection()
    {
        Direction = PreviousSegment.transform.position - transform.position;
    }

    void FaceInDirection()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, Direction);
    }

    void MoveInDirection()
    {
        if (_startingDistance < Vector3.Distance(transform.position, PreviousSegment.transform.position))
        {
            rb.AddForce(Direction.normalized * MovementSpeed * Time.deltaTime);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Head != null) Head.SendMessage("BossHurt");
        }
    }
}
