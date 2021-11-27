namespace Atlas.Commands.Entities
{
    public struct ConverterValue
    {
        public object Value { get; }
        public float Score { get; }

        public ConverterValue(object value, float score)
        {
            Value = value;
            Score = score;
        }

        public override string ToString() => Value?.ToString();
    }
}
