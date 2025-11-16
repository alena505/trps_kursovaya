namespace BooleanCompletenessBack.BusinessLogic.SyntaxTree
{
    public abstract class ExpressionNode
    {
        public abstract string ToString(Dictionary<string, int> precedence); // Для отображения с учетом приоритетов
        public abstract List<string> GetVariables(); // Для сбора переменных

        // Можно добавить Evaluate, но поскольку у вас уже есть RPN, это опционально
        public abstract ExpressionNode Clone();

    }
}
