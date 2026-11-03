using System;
using System.Collections.Generic;
using Incision;
using Incision.Interfaces;
using UnityEngine;
using VContainer;

public class RicochetChain
{
    public int BouncesLeft;
    public readonly HashSet<IDamageable> AlreadyHit = new HashSet<IDamageable>();
    public Func<Vector3, Vector3, Collider, ProjectileFactory, bool> FinalBounce;

    public CardProjectile Spawn(ProjectileFactory factory, CardData data, float damage, bool isCrit, Vector3 origin, Vector3 dir)
    {
        CardProjectile p = factory.CreateCardProjectile(
            data, damage, isCrit, origin, Quaternion.LookRotation(dir));

        p.Rigidbody.AddForce(dir * data.ProjectileSpeed, ForceMode.Impulse);
        return p;
    }
}