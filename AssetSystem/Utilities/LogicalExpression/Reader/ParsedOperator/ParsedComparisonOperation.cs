using System.Text;

namespace AssetSystem
{
    public class ParsedComparisonOperation : ParsedOperation
    {
        public override bool CanBeEnded => _name.Length > 0 && (_value.Length > 0 || _values.Count > 0) && !_inArray;
        public bool MustInclideAllProperties { get; set; }

        private bool _insideName = false, _insideValue = false, _inArray = false;
        private StringBuilder _name = new StringBuilder();
        private ComparisonOperand _comparisonOperand = ComparisonOperand.Equals;

        private int _type = 0;
        private StringBuilder _value = new StringBuilder();
        private List<string> _values = new List<string>();

        public ParsedComparisonOperation(IList<Expression> expressions, bool mustInclideAllProperties) : base(expressions)
        {
            MustInclideAllProperties = mustInclideAllProperties;
        }

        public override void NotifyOfChar(char c)
        {
            switch (c)
            {
                case '[':
                    _inArray = true; return;
                case ']':
                    _inArray = false;
                    End(); return;
                case ',':
                    if (_value.Length > 0)
                    {
                        _values.Add(_value.ToString());
                        _value.Clear();
                    }
                    return;
            }

            if (_insideValue) _value.Append(c);
            else if (_insideName) _name.Append(c);
            else
            {
                _insideName = true;
                _name.Append(c);
            }
        }
        public override int NotifyOfSpecialChar(char c)
        {
            _insideName = false; _insideValue = true;

            switch (c)
            {
                case '=':
                    if (_type == 2) _comparisonOperand = ComparisonOperand.ArrayOrEquals;
                    else _comparisonOperand = ComparisonOperand.Equals;
                    return 1;
                case '!':
                    if (_type == 1) _comparisonOperand = ComparisonOperand.ArrayAndNotEquals;
                    else _comparisonOperand = ComparisonOperand.NotEquals;
                    return 1;
                case '&':
                    _type = 1; return 1;
                case '|':
                    _type = 2; return 1;
                case ' ':
                    if (_value.Length > 0)
                    {
                        _values.Add(_value.ToString());
                        _value.Clear();
                    }
                    break;

            }
            return 0;
        }
        public override bool End()
        {
            if (HasEnded) return false;
            HasEnded = true;

            if (_value.Length > 0) _values.Add(_value.ToString());

            ComparisonOperation output = new ComparisonOperation(_name.ToString(), _values.ToArray())
            {
                Operand = _comparisonOperand,
                MustIncludeAllProperties = MustInclideAllProperties
            };

            Expressions.Last().Operations.Add(output);

            return true;
        }
    }
}
