using System.Collections;
using UnityEngine;

public class CardAnimationExplodeShoot : CardAnimation
{
    public ParticleSystem shootFxPrefab;
    public Vector3 shootAngle = new Vector3(0f, 0f, 135f);
    public Vector3 shootFxOffset = new Vector3(0f, 1f, 0f);
    public float shootScreenShake = 1f;
    public Vector3 recoilOffset = new Vector3(1f, -1f, 0f);
    public AnimationCurve recoilCurve;
    public float recoilDuration = 1f;

    public override IEnumerator Routine(object data, float startDelay = 0f)
    {
        if (data is Entity entity)
        {
            ParticleSystem shootFx = Object.Instantiate(
                shootFxPrefab,
                entity.transform.position + shootFxOffset,
                Quaternion.Euler(shootAngle)
            );
            Events.InvokeScreenShake(shootScreenShake, shootAngle.z + 180f);
            entity.curveAnimator?.Move(recoilOffset, recoilCurve, 1f, 1f);
            yield return new WaitUntil(() => !shootFx);
        }
    }
}
