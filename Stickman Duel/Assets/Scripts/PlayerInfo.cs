using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfo : MonoBehaviour {

    [SerializeField]
    float maxHealth = 0f;
    [HideInInspector]
    public float health;

    Transform healthBar;

    Rigidbody2D rightHand;

    int player;

    [HideInInspector]
    public Weapon weapon;

    void Start() {
        player = gameObject.name == "Player0" ? 0 : 1;

        healthBar = transform.Find("HealthBar");
        rightHand = transform.Find("RightHand").GetComponent<Rigidbody2D>();

        health = maxHealth;

        foreach (Collider2D collider in GetComponentsInChildren<Collider2D>()) collider.gameObject.layer = player + 8;

        if (player == 1 && Customization.instance.aiLevel != 0) gameObject.AddComponent<AI>();

        if (SceneManager.GetActiveScene().name != "Menu") {
            GetComponent<PlayerController>().forcePoint = UsefulMethods.FindTransform(EquipWeapon(Customization.instance.GetWeapon(player)).transform, "ForcePoint");
        }

        Color(Customization.instance.GetColor(player));
    }

    public void Color(Color32 toColor) {
        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>()) {
            if (!renderer.transform.IsChildOf(rightHand.transform)) {
                renderer.color = toColor;
                if (renderer.name == "HealthSprite") {
                    Color spriteColor = renderer.color;
                    spriteColor.a = 0.5f;
                    renderer.color = spriteColor;
                }
            }
        }
    }

    public GameObject EquipWeapon(GameObject weaponToEquip) {
        RemoveWeapon();
        if (player == 1) rightHand.transform.localScale = new Vector3(-1f, 1f, 1f);
        GameObject equipped = Instantiate(weaponToEquip, rightHand.transform);
        weapon = equipped.GetComponent<Weapon>();
        weapon.originalWeapon = weaponToEquip;
        equipped.GetComponent<FixedJoint2D>().connectedBody = rightHand;
        foreach (Transform item in equipped.GetComponentsInChildren<Transform>()) {
            item.gameObject.layer = player + 10;
            if (item.name == "ForcePoint") {
                if (SceneManager.GetActiveScene().name != "Menu") GetComponent<PlayerController>().forcePoint = item;
                if (player == 1) item.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
        return equipped;
    }

    public void RemoveWeapon() {
        weapon = null;
        foreach (Transform child in rightHand.transform) Destroy(child.gameObject);
    }

    public void TakeDamage(float amount) {
        if (RoundHandler.bothAlive) {
            health -= amount;
            healthBar.localScale = new Vector3(Mathf.Clamp(health, 0f, maxHealth) / maxHealth, 1f, 1f);
        }
    }
}