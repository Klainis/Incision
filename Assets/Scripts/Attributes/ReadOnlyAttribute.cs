using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ReadOnlyAttribute : UnityEngine.PropertyAttribute { }