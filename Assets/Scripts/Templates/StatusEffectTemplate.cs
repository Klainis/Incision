using System;
using Incision;
using UnityEngine;

[Flags]
public enum StatusFlags 
{
    None = 0,
    Stunned = 1 << 0,
    Barrier = 1 << 1 
}

public enum StackMode
{
    Refresh,
    Stack,
    Ignore
}

public abstract class StatusEffectTemplate : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private float _duration = 3f;
    [SerializeField] private StackMode _stackMode = StackMode.Refresh;
    [SerializeField] private int _maxStacks = 1;
    [SerializeField] private ParticleSystem _vfx;

    public string Id => _id;
    public float Duration => _duration;
    public StackMode StackMode => _stackMode;
    public int MaxStacks => _maxStacks;
    public ParticleSystem Vfx => _vfx;

    public abstract StatusEffect CreateRuntime(GameObject source);
}

public abstract class StatusEffect
{
    public StatusEffectTemplate Template { get; }
    public float Remaining { get; set; }
    public int Stacks { get; set; } = 1;
    public virtual StatusFlags Flags => StatusFlags.None;
    protected GameObject Source { get; }

    protected StatusEffect(StatusEffectTemplate tpl, GameObject source)
    { Template = tpl; Source = source; Remaining = tpl.Duration; }

    public virtual void OnApply(StatusEffectController owner) { }
    public virtual void OnTick(StatusEffectController owner, float dt) { }
    public virtual void OnRemove(StatusEffectController owner) { }
}