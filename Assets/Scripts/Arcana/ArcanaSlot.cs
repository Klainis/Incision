namespace Incision.Arcana
{
    public readonly struct ArcanaSlot
    {
        public ArcanaSlot(ArcanaDefinition definition, int level)
        {
            Definition = definition;
            Level = level;
        }

        public ArcanaDefinition Definition { get; }
        public int Level { get; }
    }
}
