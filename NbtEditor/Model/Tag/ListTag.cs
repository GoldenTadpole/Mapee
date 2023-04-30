using CommonUtilities.Collections.Simple;
using System.Collections;

namespace NbtEditor
{
    public sealed class ListTag : Tag, IList<Tag>
    {
        private SimpleList<Tag> _tags;

        public TagId ElementId { get; set; }

        public int Count => _tags.Count;
        public bool IsReadOnly => false;

        public Tag this[int index]
        {
            get => _tags[index];
            set 
            {
                if (value.Id != ElementId) throw IncorrectListElementIdException.Default;
                _tags[index] = value;
            }
        }

        public ListTag(TagId elementId) : base(TagId.List)
        {
            ElementId = elementId;
            _tags = new SimpleList<Tag>();
        }
        public ListTag(TagId elementId, int count) : base(TagId.List) 
        {
            ElementId = elementId;
            _tags = new SimpleList<Tag>(count);
        }

        public void Add(Tag item) 
        {
            if (item.Id != ElementId) throw IncorrectListElementIdException.Default;
            _tags.Add(item);
        }
        public void Insert(int index, Tag item) 
        {
            if (item.Id != ElementId) throw IncorrectListElementIdException.Default;
            _tags.Insert(index, item);
        }

        public bool Remove(Tag item)
        {
            return _tags.Remove(item);
        }
        public void RemoveAt(int index) 
        {
            _tags.RemoveAt(index);
        }
        public void Clear() 
        {
            _tags.Clear();
        }

        public int IndexOf(Tag item) 
        {
            return _tags.IndexOf(item);
        }
        public bool Contains(Tag item)
        {
            return _tags.Contains(item);
        }
        public void CopyTo(Tag[] array, int arrayIndex)
        {
            for (int i = 0; i < array.Length - arrayIndex; i++) 
            {
                array[arrayIndex + i] = _tags[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _tags.GetEnumerator();
        }
        public IEnumerator<Tag> GetEnumerator()
        {
            return _tags.GetEnumerator();
        }
    }
}
