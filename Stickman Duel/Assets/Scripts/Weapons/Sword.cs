using System.Collections;
using UnityEngine;

public class Sword : Weapon {

    [SerializeField]
    float extraSize = 0f,
        aiAbilityConstantDistance = 0f,
        aiAbilityVariableDistance = 0f;

    ParticleSystem particle;

    protected override void Start() {
        base.Start();

        particle = transform.Find("Particle System").GetComponent<ParticleSystem>();
        ParticleSystem.MainModule settings = particle.main;
        settings.startColor = new ParticleSystem.MinMaxGradient(Customization.instance.GetColor(player));
    }

    protected override IEnumerator Ability() {
        if (ready) {
            ready = false;
            GetComponent<SpriteRenderer>().color = originalWeapon.GetComponent<SpriteRenderer>().color;

            particle.Play();
            transform.localScale += new Vector3(extraSize, extraSize, extraSize);

            yield return new WaitForSeconds(duration);

            particle.Stop();
            transform.localScale -= new Vector3(extraSize, extraSize, extraSize);

            yield return new WaitForSeconds(cooldown);
            LookReady();
        }
    }

    public override void AiCondition() {
        if (ready && (transform.position - targetTorso.position).magnitude <= aiAbilityVariableDistance * ai.aiPercent + aiAbilityConstantDistance) StartCoroutine(Ability());
    }

    public override void LookReady() {
        GetComponent<SpriteRenderer>().color = Customization.instance.GetColor(player);
        base.LookReady();
    }
}
