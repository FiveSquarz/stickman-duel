using UnityEngine;

public class GunBullet : MonoBehaviour {

    [HideInInspector]
    public float damage, knockback;

    bool hit = false;

    void OnCollisionEnter2D(Collision2D collision) {
        if (!hit) {
            hit = true;
            collision.rigidbody.AddForce(transform.right * knockback, ForceMode2D.Impulse);

            if (collision.transform.parent != null) {
                PlayerInfo otherPlayerInfo = collision.transform.parent.GetComponent<PlayerInfo>();
                if (otherPlayerInfo != null) otherPlayerInfo.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
