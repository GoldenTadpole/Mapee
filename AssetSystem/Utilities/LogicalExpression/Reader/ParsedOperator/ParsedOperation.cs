namespace AssetSystem
{
    public abstract class ParsedOperation
    {
        public IList<Expression> Expressions { get; set; }
        public virtual bool CanBeEnded { get; protected set; } = false;
        public virtual bool HasEnded { get; protected set; } = false;

        public ParsedOperation(IList<Expression> expressions)
        {
            Expressions = expressions;
        }

        public abstract void NotifyOfChar(char c);
        public abstract int NotifyOfSpecialChar(char c);
        public abstract bool End();
    }
}
