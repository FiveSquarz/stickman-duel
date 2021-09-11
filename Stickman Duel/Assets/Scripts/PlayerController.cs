using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    Rigidbody2D rightHand = null,
        torsoRb = null;
    Rigidbody2D[] allRigidbodies;

    public float handForce;

    [SerializeField]
    float animationSpeed = 0f,
        reduceUprightForceMultiplier = 0f,
        jumpCooldown = 0f,
        jumpForce = 0f;

    [HideInInspector]
    public Vector2 direction;

    [SerializeField]
    Collider2D jumpDetector = null;

    public LayerMask jumpLayers = 0;
    float lastJump = 0f;

    [SerializeField]
    Upright UL = null,
        LL = null,
        UR = null,
        LR = null,
        torsoUpright = null;

    float amount = 0f;

    [HideInInspector]
    public Transform forcePoint;

    [HideInInspector]
    public bool canJump;

    void Start() {
        allRigidbodies = torsoRb.transform.parent.GetComponentsInChildren<Rigidbody2D>();
    }

    void Update() {

        float absX = Mathf.Abs(direction.x);
        if (absX >= 0.3f) {
            amount += animationSpeed * Time.deltaTime;
            UL.rotation = LL.rotation = UL.absOriginalRotation * Mathf.Sin(amount) * absX;
            UR.rotation = LR.rotation =  UR.absOriginalRotation * Mathf.Sin(amount - Mathf.PI) * absX;
        } else {
            UL.rotation = Mathf.Sign(UL.rotation) * UL.absOriginalRotation;
            LL.rotation = UL.rotation / 2f;
            UR.rotation = -UL.rotation;
            LR.rotation = UR.rotation / 2f;
        }

        UL.force = LL.force = UR.force = LR.force = torsoUpright.force = Mathf.Clamp(UL.originalForce + direction.y * UL.originalForce * reduceUprightForceMultiplier, 0f, UL.originalForce);
    }

    void FixedUpdate() {
        if (jumpDetector.IsTouchingLayers(jumpLayers) && Time.time >= lastJump + jumpCooldown) canJump = true;
        if (direction.y >= 0.75f && canJump) {
            canJump = false;
            lastJump = Time.time;
            foreach (Rigidbody2D rb in allRigidbodies) rb.velocity = new Vector2(rb.velocity.x, 0f);
            torsoRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        rightHand.AddForceAtPosition(direction * handForce * Time.fixedDeltaTime, forcePoint.position, ForceMode2D.Impulse);
    } 
}