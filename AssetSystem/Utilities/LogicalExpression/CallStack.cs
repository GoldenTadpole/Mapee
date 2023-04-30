namespace AssetSystem
{
    public struct CallStack
    {
        private IOperation[] _operations;
        private int _index = 0;

        public CallStack(IOperation[] operations)
        {
            _operations = operations;
            _index = 0;
        }

        public IOperation Next()
        {
            return _operations[_index];
        }
        public void Advance()
        {
            _index++;
        }

        public bool Run(PropertyValueProvider propertyValueProvider)
        {
            return Next().Execute(this, propertyValueProvider);
        }
    }
}
