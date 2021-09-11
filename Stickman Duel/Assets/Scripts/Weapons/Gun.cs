using System.Collections;
using UnityEngine;

public class Gun : Weapon {

    [SerializeField]
    GameObject bullet = null;
    [SerializeField]
    float bulletDamage = 0f,
        bulletKnockback = 0f,
        fireCooldown = 0f,
        velocity = 0f,
        //maxLifetime = 0f,
        abilityDamage = 0f,
        abilityKnockback = 0f,
        spread = 0f,
        velocitySpread = 0f,
        aiMaxAngle = 0f;
    [SerializeField]
    int abilityAmount = 0;

    float lastFire = 0f;

    protected override void Start() {
        base.Start();
    }

    void Update() {
        if (Time.time >= lastFire + fireCooldown && controller != null && controller.direction.normalized.sqrMagnitude != 0f) {
            lastFire = Time.time;
            StartCoroutine(FireBullet(false, 0f));
        } 
    }

    IEnumerator FireBullet(bool abilityMode, float angle) {
        yield return new WaitForFixedUpdate();
        GameObject bulletClone = Instantiate(bullet, forcePoint.position, Quaternion.identity);
        bulletClone.GetComponent<SpriteRenderer>().color = Customization.instance.GetColor(player);
        bulletClone.layer = 10 + player;
        GunBullet gunbullet = bulletClone.GetComponent<GunBullet>();
        if (abilityMode) {
            float thisVelocitySpread = Random.Range(1 / velocitySpread, velocitySpread);
            bulletClone.GetComponent<Rigidbody2D>().velocity = gunbullet.transform.right = UsefulMethods.RotateVector2(forcePoint.right * velocity * thisVelocitySpread, angle);
            gunbullet.damage = abilityDamage;
            gunbullet.knockback = abilityKnockback;
            //Destroy(bulletClone, maxLifetime / thisVelocitySpread);
        } else {
            bulletClone.GetComponent<Rigidbody2D>().velocity = gunbullet.transform.right = controller.direction.normalized * velocity;
            gunbullet.damage = bulletDamage;
            gunbullet.knockback = bulletKnockback;
            //Destroy(bulletClone, maxLifetime);
        }
    }

    protected override IEnumerator Ability() {
        if (ready) {
            ready = false;
            GetComponent<SpriteRenderer>().color = originalWeapon.GetComponent<SpriteRenderer>().color;

            for (int i = 0; i < abilityAmount; i++) StartCoroutine(FireBullet(true, Random.Range(-spread, spread)));

            yield return new WaitForSeconds(cooldown);
            LookReady();
        }
    }

    public override void AiCondition() {
        if (ready && UsefulMethods.AngleBetween(forcePoint.right, targetTorso.position - forcePoint.position) <= ai.aiPercent * aiMaxAngle + aiMaxAngle * 0.1f) StartCoroutine(Ability());
    }

    public override void LookReady() {
        GetComponent<SpriteRenderer>().color = Customization.instance.GetColor(player);
        base.LookReady();
    }
}
