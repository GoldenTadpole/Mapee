namespace NbtEditor
{
    public class IncorrectListElementIdException : Exception
    {
        public static IncorrectListElementIdException Default => new IncorrectListElementIdException("Value id does not match the list element id.");

        public IncorrectListElementIdException() { }
        public IncorrectListElementIdException(string message) : base(message) { }
        public IncorrectListElementIdException(string message, Exception inner) : base(message, inner) { }
    }
}
