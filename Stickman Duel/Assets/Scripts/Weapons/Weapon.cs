using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    [SerializeField]
    protected float baseDamage = 0f,
        knockback = 0f,
        cooldown = 0f,
        duration = 0f,
        aiAbilityMaxWait = 0f;

    protected bool ready;

    protected PlayerInfo playerInfo;
    protected PlayerController controller;
    protected int player;

    protected Transform forcePoint;

    protected AI ai;
    protected Transform targetTorso;

    public GameObject originalWeapon;

    protected virtual void Start() {
        playerInfo = GetComponentInParent<PlayerInfo>();
        controller = playerInfo.GetComponent<PlayerController>();
        player = playerInfo.gameObject.name == "Player0" ? 0 : 1;
        forcePoint = UsefulMethods.FindTransform(transform, "ForcePoint");

        InputHandler.abilities[player] = CheckAbility;

        ai = GetComponentInParent<AI>();
        targetTorso = GameObject.Find("Player0").transform.Find("Torso");

        LookReady();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        collision.rigidbody.AddForce((collision.GetContact(0).point - (Vector2)transform.position).normalized * knockback, ForceMode2D.Impulse);

        if (collision.transform.parent != null) {
            PlayerInfo otherPlayerInfo = collision.transform.parent.GetComponent<PlayerInfo>();
            if (otherPlayerInfo != null) otherPlayerInfo.TakeDamage(baseDamage);
        }
    }

    protected abstract IEnumerator Ability();
    void CheckAbility() {
        if (ready) StartCoroutine(Ability());
    }
    public abstract void AiCondition();
    public virtual void LookReady() {
        if (ai) Invoke(nameof(Ready), Random.Range(0f, ai.aiPercent * aiAbilityMaxWait));
        else Ready();
    }
    void Ready() {
        ready = true;
    }
}
