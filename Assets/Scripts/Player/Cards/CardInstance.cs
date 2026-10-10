using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInstance
{
    public CardData Data { get; }
    public int Ammo { get; private set; }
    public float NextShotTime { get; set; }

    public bool IsEmpty => Ammo <= 0;

    public CardInstance(CardData data)
    {
        Data = data;
        Ammo = data.MaxAmmo;
    }

    public bool TrySpendAmmo(int amount)
    {
        if (Ammo < amount) return false;

        Ammo -= amount;

        return true;
    }

    public void Reload()
    {
        Ammo = Data.MaxAmmo;
        NextShotTime = 0f;
    }

    public float RollDamage(out bool isCrit)
    {
        isCrit = Random.value < Data.CritChance;

        return isCrit ? Data.Damage * Data.CritDamageMultiplier : Data.Damage;
    }
}
