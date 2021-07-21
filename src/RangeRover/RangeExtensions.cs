using System;
using System.Collections.Generic;
using System.Linq;

namespace RangeRover
{
    public static class RangeExtensions
    {
        public static IEnumerable<Range<T>> Combine<T>(this IEnumerable<Range<T>> ranges)
            where T : IComparable
        {
            var queue = new Queue<Range<T>>(ranges);

            while (queue.Count > 0)
            {
                Range<T> source = queue.Dequeue();

                for (int i = queue.Count; i > 0; i--)
                {
                    Range<T> next = queue.Dequeue();

                    if (next.From.CompareTo(source.From) >= 0 && next.From.CompareTo(source.To) <= 0)
                    {
                        T to = source.To.CompareTo(next.To) >= 0 ? source.To : next.To;
                        source = new Range<T>(source.From, to);
                    }
                    else if (next.To.CompareTo(source.From) >= 0 && next.To.CompareTo(source.To) <= 0)
                    {
                        T from = source.From.CompareTo(next.From) <= 0 ? source.From : next.From;
                        source = new Range<T>(from, source.To);
                    }
                    else if (next.From.CompareTo(source.From) < 0 && next.To.CompareTo(source.To) > 0)
                    {
                        source = new Range<T>(next.From, next.To);
                    }
                    else
                    {
                        queue.Enqueue(next);
                    }
                }

                yield return source;
            }
        }

        public static IEnumerable<Range<T>> Intersect<T>(this IEnumerable<Range<T>> first, IEnumerable<Range<T>> second)
            where T : IComparable
            => IntersectInternal(first, second).Combine();

        private static IEnumerable<Range<T>> IntersectInternal<T>(IEnumerable<Range<T>> first, IEnumerable<Range<T>> second)
            where T : IComparable
        {
            Range<T>[] secondArray = second.ToArray();

            foreach (var source in first.Combine())
            {
                foreach (var other in secondArray)
                {
                    if (other.From.CompareTo(source.From) >= 0 && other.From.CompareTo(source.To) < 0)
                    {
                        T to = source.To.CompareTo(other.To) < 0 ? source.To : other.To;
                        yield return new Range<T>(other.From, to);
                    }
                    else if (other.To.CompareTo(source.From) > 0 && other.To.CompareTo(source.To) <= 0)
                    {
                        T from = source.From.CompareTo(other.From) > 0 ? source.From : other.From;
                        yield return new Range<T>(from, other.To);
                    }
                    else if (other.From.CompareTo(source.From) < 0 && other.To.CompareTo(source.To) > 0)
                    {
                        yield return new Range<T>(source.From, source.To);
                    }
                }
            }
        }

        public static IEnumerable<Range<T>> Subtract<T>(this IEnumerable<Range<T>> first, IEnumerable<Range<T>> second)
            where T : IComparable
            => SubtractInternal(first, second).Combine();

        private static IEnumerable<Range<T>> SubtractInternal<T>(IEnumerable<Range<T>> minuends, IEnumerable<Range<T>> subtrahends)
            where T : IComparable
        {
            var queue = new Queue<Range<T>>(minuends.Combine());

            foreach (var subtrahend in subtrahends)
            {
                for (int i = queue.Count; i > 0; i--)
                {
                    Range<T> minuend = queue.Dequeue();

                    if (subtrahend.From.CompareTo(minuend.From) > 0 && subtrahend.To.CompareTo(minuend.To) >= 0)
                    {
                        T to = subtrahend.From.CompareTo(minuend.To) < 0 ? subtrahend.From : minuend.To;
                        queue.Enqueue(new Range<T>(minuend.From, to));
                    }
                    else if (subtrahend.To.CompareTo(minuend.To) < 0 && subtrahend.From.CompareTo(minuend.From) <= 0)
                    {
                        T from = subtrahend.To.CompareTo(minuend.From) > 0 ? subtrahend.To : minuend.From;
                        queue.Enqueue(new Range<T>(from, minuend.To));
                    }
                    else if (subtrahend.From.CompareTo(minuend.From) > 0 && subtrahend.To.CompareTo(minuend.To) < 0)
                    {
                        queue.Enqueue(new Range<T>(minuend.From, subtrahend.From));
                        queue.Enqueue(new Range<T>(subtrahend.To, minuend.To));
                    }
                }
            }

            return queue;
        }
    }
}
