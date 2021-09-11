using UnityEngine;

public class Upright : MonoBehaviour {

    Rigidbody2D rb;
    public float rotation, force;
    [HideInInspector]
    public float originalRotation, originalForce, absOriginalRotation;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        originalRotation = rotation;
        originalForce = force;
        absOriginalRotation = Mathf.Abs(originalRotation);
    }

    // Update is called once per frame
    void FixedUpdate() {
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotation, force * Time.fixedDeltaTime));
    }
}
