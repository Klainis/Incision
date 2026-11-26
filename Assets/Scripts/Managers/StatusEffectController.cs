using Incision.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectController : MonoBehaviour, IStatusReceiver
{
    [SerializeField] private StatusFlags _immunities;

    private readonly List<StatusEffect> _effects = new();
    public StatusFlags Flags { get; private set; }
    public event Action<StatusFlags> FlagsChanged;

    public bool Has(StatusFlags f) => (Flags & f) != 0;

    public bool TryApply(StatusEffectTemplate tpl, GameObject source, float? duration = null)
    {
        var fresh = tpl.CreateRuntime(source);
        if ((fresh.Flags & _immunities) != 0) return false;

        var existing = _effects.Find(e => e.Template == tpl);
        if (existing != null)
        {
            switch (tpl.StackMode)
            {
                case StackMode.Ignore: return false;
                case StackMode.Refresh: existing.Remaining = duration ?? tpl.Duration; break;
                case StackMode.Stack:
                    existing.Stacks = Mathf.Min(existing.Stacks + 1, tpl.MaxStacks);
                    existing.Remaining = duration ?? tpl.Duration; break;
            }
            return true;
        }

        if (duration.HasValue) fresh.Remaining = duration.Value;
        _effects.Add(fresh);
        fresh.OnApply(this);
        RecalculateFlags();
        return true;
    }

    private void Update()
    {
        var dt = Time.deltaTime;
        for (int i = _effects.Count - 1; i >= 0; i--)
        {
            var e = _effects[i];
            e.OnTick(this, dt);
            e.Remaining -= dt;
            if (e.Remaining <= 0f)
            {
                e.OnRemove(this);
                _effects.RemoveAt(i);
                RecalculateFlags();
            }
        }
    }

    public void ClearAll()
    {
        foreach (var e in _effects) e.OnRemove(this);
        _effects.Clear();
        RecalculateFlags();
    }

    private void RecalculateFlags()
    {
        var f = StatusFlags.None;
        foreach (var e in _effects) f |= e.Flags;
        if (f == Flags) return;
        Flags = f;
        FlagsChanged?.Invoke(Flags);
    }
}
