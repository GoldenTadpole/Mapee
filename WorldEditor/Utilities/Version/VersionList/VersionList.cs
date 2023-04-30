using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace WorldEditor
{
    public class VersionList<TValue> : IList<VersionPeriod<TValue>>
    {
        private IList<VersionPeriod<TValue>> _versionPeriods;

        public int Count => _versionPeriods.Count;
        public bool IsReadOnly => _versionPeriods.IsReadOnly;

        public VersionPeriod<TValue> this[int index]
        {
            get => _versionPeriods[index];
            set => _versionPeriods[index] = value;
        }

        public VersionList(int count = 0)
        {
            _versionPeriods = new List<VersionPeriod<TValue>>(count);
        }

        public void Add(Version beginningVersion, Version endVersion, TValue value)
        {
            _versionPeriods.Add(new VersionPeriod<TValue>(beginningVersion, endVersion, value));
        }
        public bool TryRetrieveValue(Version version, [MaybeNullWhen(false)] out TValue value)
        {
            value = default;
            if (_versionPeriods.Count < 1 || version < _versionPeriods[0].BeginningVersion) return false;

            for (int i = 0; i < _versionPeriods.Count; i++)
            {
                if (version < _versionPeriods[i].BeginningVersion) continue;

                if (version <= _versionPeriods[i].EndVersion)
                {
                    value = _versionPeriods[i].Value;
                    return true;
                }
            }

            return false;
        }

        public void Add(VersionPeriod<TValue> item)
        {
            _versionPeriods.Add(item);
        }
        public void Insert(int index, VersionPeriod<TValue> item)
        {
            _versionPeriods.Insert(index, item);
        }

        public void Clear()
        {
            _versionPeriods.Clear();
        }
        public bool Remove(VersionPeriod<TValue> item)
        {
            return _versionPeriods.Remove(item);
        }
        public void RemoveAt(int index)
        {
            _versionPeriods.RemoveAt(index);
        }

        public bool Contains(VersionPeriod<TValue> item)
        {
            return _versionPeriods.Contains(item);
        }
        public void CopyTo(VersionPeriod<TValue>[] array, int arrayIndex)
        {
            _versionPeriods.CopyTo(array, arrayIndex);
        }
        public int IndexOf(VersionPeriod<TValue> item)
        {
            return _versionPeriods.IndexOf(item);
        }

        public IEnumerator<VersionPeriod<TValue>> GetEnumerator()
        {
            return _versionPeriods.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _versionPeriods.GetEnumerator();
        }

        public VersionList<TValue> Clone()
        {
            VersionList<TValue> output = new VersionList<TValue>();

            foreach (var item in _versionPeriods)
            {
                output.Add(item);
            }

            return output;
        }
    }
}
