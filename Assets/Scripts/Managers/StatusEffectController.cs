using Incision.Extensions;
using Incision.Interfaces;
using Incision.Structs.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

public class StatusEffectController : MonoBehaviour, IStatusReceiver
{
    public string LOG_TAG { get; } = "[StatusEffectController]".AsTag(Color.white);

    [SerializeField] private StatusFlags _immunities;

    private readonly List<StatusEffect> _effects = new();
    public StatusFlags Flags { get; private set; }
    public event Action<StatusFlags> FlagsChanged;

    [Inject] IEventBus _eventBus;

    public bool Has(StatusFlags f) => (Flags & f) != 0;

    public bool TryApplyStatus(StatusEffectTemplate tpl, GameObject source, float? duration = null)
    {
        var fresh = tpl.CreateRuntime(source);
        if ((fresh.Flags & _immunities) != 0)
        {
            _eventBus.Publish(
                new Log(
                    LOG_TAG,
                    $"Failed to apply status {tpl.Id.AsTag(Color.magenta)} from {source.name.AsTag(Color.cyan)} because enemy has {"immunity".AsTag(Color.red)}"
                    )
            );
            return false;
        }

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

            _eventBus.Publish(
                new Log(
                    LOG_TAG,
                    $"Apply status {tpl.Id.AsTag(Color.magenta)} from {source.name.AsTag(Color.cyan)}. Duration: {tpl.Duration}"
                    )
            );
            return true;
        }

        if (duration.HasValue) fresh.Remaining = duration.Value;
        _effects.Add(fresh);
        fresh.OnApply(this);
        RecalculateFlags();

        _eventBus.Publish(
            new Log(
                LOG_TAG,
                $"Apply status {tpl.Id.AsTag(Color.magenta)} from {source.name.AsTag(Color.cyan)}. Duration: {tpl.Duration}"
                )
        );
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
