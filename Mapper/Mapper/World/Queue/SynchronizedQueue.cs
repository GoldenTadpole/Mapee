using CommonUtilities.Collections.Simple;
using CommonUtilities.Collections.Synchronized;
using System.Collections.ObjectModel;
using WorldEditor;

namespace Mapper
{
    public class SynchronizedQueue : IQueue<Coords>
    {
        private readonly IList<Coords> _queue = new SynchronizedList<Coords>(new SimpleList<Coords>(10));
        private readonly ISet<Coords> _fastLookupSet = new SynchronizedSet<Coords>(10);

        public int Count => _queue.Count;

        public Coords[] TakeFirst(int count)
        {
            Coords[] output = new Coords[count > _queue.Count ? _queue.Count : count];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = _queue[i];
            }

            RemoveFirst(count);

            return output;
        }

        public void RemoveFirst(int count)
        {
            count = count > _queue.Count ? _queue.Count : count;

            for (int i = 0; i < count; i++)
            {
                _fastLookupSet.Remove(_queue[0]);
                _queue.RemoveAt(0);
            }
        }
        public void ReplaceWith(ReadOnlyMemory<Coords> items)
        {
            _queue.Clear();
            _fastLookupSet.Clear();

            ReadOnlySpan<Coords> span = items.Span;
            for (int i = 0; i < items.Length; i++)
            {
                _queue.Add(span[i]);
            }

            _fastLookupSet.UnionWith(ToEnumerable(items));
        }

        public static IEnumerable<T> ToEnumerable<T>(ReadOnlyMemory<T> memory)
        {
            for (int i = 0; i < memory.Length; i++)
            {
                yield return memory.Span[i];
            }
        }
    }
}
