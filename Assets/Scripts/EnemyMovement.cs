using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize speed if needed, but serialized values in Unity Editor take priority.
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    // Handles enemy movement logic
    void Move()
    {
        if (transform.position.x < -17.5f)
        {
            // Rotate and move right when the enemy goes out of bounds on the left
            transform.Rotate(0, 180, 0);
            transform.position += Vector3.right * Time.deltaTime * speed;
        }
        else if (transform.position.x > 11.5f)
        {
            // Rotate and move left when the enemy goes out of bounds on the right
            transform.Rotate(0, 180, 0);
            transform.position += Vector3.left * Time.deltaTime * speed;
        }
        else
        {
            // Move forward within bounds
            transform.position += transform.forward * speed * Time.deltaTime;

            // Ensure the enemy stays grounded by setting Y position to 0
            Vector3 pos = transform.position;
            pos.y = 0;
            transform.position = pos;
        }
    }
}
