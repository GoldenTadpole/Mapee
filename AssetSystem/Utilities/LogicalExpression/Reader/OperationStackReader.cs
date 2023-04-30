namespace AssetSystem
{
    public class OperationStackReader
    {
        public bool MustIncludeAllProperties { get; set; }

        public OperationStackReader(bool mustIncludeAllProperties)
        {
            MustIncludeAllProperties = mustIncludeAllProperties;
        }

        public IOperation[] Read(string text)
        {
            return ReadExpression(new Buffer<char>(text.ToArray()), 0).Operations.ToArray();
        }
        private Expression ReadExpression(Buffer<char> buffer, int level)
        {
            List<Expression> output = new List<Expression>() { new Expression(level) };

            ParsedOperation? operation = null;
            while (buffer.HasNext())
            {
                switch (buffer.Next())
                {
                    case ')': operation?.End(); return new Expression(output, level);
                    case '(':
                        EndIfNeeded();
                        output.Add(ReadExpression(buffer, level + 1));
                        output.Add(new Expression(level)); break;
                    case ' ':
                        if (!EndIfNeeded()) buffer.Index += operation?.NotifyOfSpecialChar(buffer.Current) ?? 0;
                        break;
                    case '=':
                    case '!':
                    case '&':
                    case '|':
                    case '^':
                        if (EndIfNeeded()) operation = new ParsedLogicalOperation(output);
                        buffer.Index += operation?.NotifyOfSpecialChar(buffer.Current) ?? 0; break;
                    default:
                        if (HasEnded()) operation = new ParsedComparisonOperation(output, MustIncludeAllProperties);
                        operation?.NotifyOfChar(buffer.Current); break;
                }

                if (!buffer.HasNext() && !HasEnded()) operation?.End();
            }

            bool EndIfNeeded() => HasEnded() ? true : (operation?.CanBeEnded ?? false) ? operation.End() : false;
            bool HasEnded() => operation == null || operation.HasEnded;

            return new Expression(output, level);
        }
    }
}
