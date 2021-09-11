using UnityEngine;

public class DeathBarrier : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision) {
        PlayerInfo playerInfo = collision.gameObject.GetComponentInParent<PlayerInfo>();
        if (playerInfo) playerInfo.TakeDamage(Mathf.Infinity);
    }
}
