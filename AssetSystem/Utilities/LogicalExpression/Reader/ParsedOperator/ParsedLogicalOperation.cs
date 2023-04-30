namespace AssetSystem
{
    public class ParsedLogicalOperation : ParsedOperation
    {
        private LogicalOperand _logicalOperand = LogicalOperand.Or;

        public ParsedLogicalOperation(IList<Expression> operations) : base(operations) { }

        public override void NotifyOfChar(char c) { }
        public override int NotifyOfSpecialChar(char c)
        {
            CanBeEnded = true;

            switch (c)
            {
                case '!':
                    _logicalOperand = LogicalOperand.Negate;
                    End();
                    return 0;
                case '&':
                    _logicalOperand = LogicalOperand.And;
                    End();
                    return 1;
                case '|':
                    _logicalOperand = LogicalOperand.Or;
                    End();
                    return 1;
                case '^':
                    _logicalOperand = LogicalOperand.Xor;
                    End();
                    return 0;
            }

            return 0;
        }
        public override bool End()
        {
            if (HasEnded) return false;
            HasEnded = true;

            LogicalOperation output = new LogicalOperation()
            {
                Operand = _logicalOperand
            };

            switch (_logicalOperand)
            {
                case LogicalOperand.Negate:
                    Expressions.Last().Operations.Add(output);
                    break;
                case LogicalOperand.And:
                    int[] insertAt = FindInsertsForAndOperator();

                    if (insertAt[0] == -1 || insertAt[1] == -1) break;
                    Expressions[insertAt[0]].Operations.Insert(insertAt[1], output);

                    break;
                case LogicalOperand.Or:
                case LogicalOperand.Xor:
                    Expressions.First().Operations.Insert(0, output);
                    break;
            }

            return true;
        }

        private int[] FindInsertsForAndOperator()
        {
            int[] insertAt = new int[] { -1, -1 }, prevInsertAt = new int[] { -1, -1 };

            int i = Expressions.Count;
            while (--i >= 0)
            {
                if (Expressions[i].Level != Expressions.Last().Level)
                {
                    SetInserts(0);
                    continue;
                }

                int j = Expressions[i].Operations.Count;
                if (j == 0) SetInserts(j);

                while (--j >= 0)
                {
                    IOperation operation = Expressions[i].Operations[j];
                    if (operation is LogicalOperation l)
                    {
                        switch (l.Operand)
                        {
                            case LogicalOperand.Or:
                            case LogicalOperand.Xor:
                                return prevInsertAt;
                        }
                    }

                    SetInserts(j);
                }
            }

            void SetInserts(int j)
            {
                prevInsertAt = new int[] { insertAt[0], insertAt[1] };
                insertAt[0] = i; insertAt[1] = j;
            }

            return insertAt;
        }
    }
}
