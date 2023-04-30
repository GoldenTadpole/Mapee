using CommonUtilities.Collections.Simple;
using CommonUtilities.Collections.Synchronized;

namespace Mapper
{
    public class SynchronizedQueue : IQueue<string>
    {
        private IList<string> _queue = new SynchronizedList<string>(new SimpleList<string>(10));
        private ISet<string> _fastLookupSet = new SynchronizedSet<string>(10);

        public int Count => _queue.Count;

        public string[] TakeFirst(int count)
        {
            string[] output = new string[count > _queue.Count ? _queue.Count : count];
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
        public void ReplaceWith(ReadOnlyMemory<string> items)
        {
            _queue.Clear();
            _fastLookupSet.Clear();

            ReadOnlySpan<string> span = items.Span;
            for (int i = 0; i < items.Length; i++)
            {
                if (string.IsNullOrEmpty(span[i])) continue;
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
