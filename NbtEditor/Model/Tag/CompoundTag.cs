using CommonUtilities.Collections.Simple;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace NbtEditor 
{
    public sealed class CompoundTag : Tag, IEnumerable<KeyValueEntry<string, Tag>>
    {
        private SimpleHashedList<string, Tag> _tags;

        public int Count => _tags.Count;

        public Tag? this[string key]
        {
            get => _tags[key];
            set => _tags[key] = value;
        }

        public CompoundTag() : base(TagId.Compound) 
        {
            _tags = new SimpleHashedList<string, Tag>();
        }
        public CompoundTag(int count) : base(TagId.Compound) 
        {
            _tags = new SimpleHashedList<string, Tag>(count);
        }

        public void Add(string key, Tag value) 
        {
            _tags.Add(key, value);
        }
        public void Add(KeyValueEntry<string, Tag> item)
        {
            _tags.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _tags.Clear();
        }
        public void ClearInternalArray() 
        {
            _tags.ClearInternalArray();
        }

        public void Remove(string key) 
        {
            _tags.Remove(key);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out Tag value)
        {
            return _tags.TryGetValue(key, out value);
        }

        public Tag GetChild(string path)
        {
            return GetChild(path.Split("/"));
        }
        public Tag GetChild(params string[] path)
        {
            if (path.Length == 1) return _tags[path[0]];

            string[] childPath = new string[path.Length - 1];
            for (int i = 1; i < path.Length; i++) 
            {
                childPath[i - 1] = path[i];
            }

            return (_tags[path[0]] as CompoundTag).GetChild(childPath);
        }

        public bool TryGetChild(string path, out Tag child) 
        {
            return TryGetChild(out child, path.Split("/"));
        }
        public bool TryGetChild(out Tag child, params string[] path) 
        {
            if (path.Length == 1) return _tags.TryGetValue(path[0], out child);

            string[] childPath = new string[path.Length - 1];
            for (int i = 1; i < path.Length; i++)
            {
                childPath[i - 1] = path[i];
            }

            if (_tags.TryGetValue(path[0], out Tag tag) && tag is CompoundTag c) 
            {
                return c.TryGetChild(out child, childPath);
            } 
            else
            {
                child = null;
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _tags.GetEnumerator();
        }
        public IEnumerator<KeyValueEntry<string, Tag>> GetEnumerator() 
        {
            return _tags.GetEnumerator();
        }
    }
}
