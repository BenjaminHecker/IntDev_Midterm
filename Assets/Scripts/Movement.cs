using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static Vector2 origin;

    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float drag = 3f;

    public static Vector2 velocity = Vector2.zero;
    public static bool run = true;

    public static void Startup()
    {
        run = true;
        velocity = Vector2.zero;
    }

    public static void Stop()
    {
        run = false;
        velocity = Vector2.zero;
    }

    void Update()
    {
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (run && move != Vector2.zero)
            velocity += move.normalized * acceleration * Time.deltaTime;
        else
            velocity *= (1 - drag * Time.deltaTime);

        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

        origin += velocity * Time.deltaTime;
    }
}
