using UnityEngine;

public class Deleter : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision) {
        PlayerInfo player = collision.transform.GetComponentInParent<PlayerInfo>();
        if (player) player.health = 0f;
        else Destroy(collision.gameObject);
    }
}
