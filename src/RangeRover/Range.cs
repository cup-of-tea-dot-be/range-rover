namespace RangeRover
{
    public readonly struct Range<T>
    {
        public Range(T from, T to)
        {
            From = from;
            To = to;
        }

        public T From { get; }

        public T To { get; }
    }
}
