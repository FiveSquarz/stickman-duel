using System.Collections;
using UnityEngine;

public class Slider : Tween {

    [SerializeField]
    Vector2 relativeEndpoint = Vector2.zero;
    [SerializeField]
    float relativeZRotation = 0f;
    
    Rigidbody2D rb;

    Vector2 originalPosition;
    float originalRotation;

    protected override IEnumerator Start() {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        originalRotation = rb.rotation;

        move = Move;
        return base.Start();
    }

    void Move() {
        float amount = GetAmount();
        if (relativeEndpoint != Vector2.zero) rb.MovePosition(Vector2.LerpUnclamped(originalPosition, originalPosition + relativeEndpoint, amount));
        if (relativeZRotation != 0f) rb.MoveRotation(Mathf.LerpAngle(originalRotation, originalRotation + relativeZRotation, amount));
    }
}
