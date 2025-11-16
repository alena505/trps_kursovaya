namespace BooleanCompletenessBack.Models
{

    public class ExpressionChange
    {
        public string Expression { get; set; }
        
        public string Reason { get; set; }

        public ExpressionChange(string reason, string expression)
        {
            Reason = reason;
            Expression = expression;
        }
    }
}
