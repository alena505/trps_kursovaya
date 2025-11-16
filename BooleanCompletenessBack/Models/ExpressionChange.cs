namespace BooleanCompletenessBack.Models
{

    //Описание изменения выражения
    public class ExpressionChange
    {
        //Полученное выражение
        public string Expression { get; set; }

        //Причина изменения выражения
        public string Reason { get; set; }

        public ExpressionChange(string reason, string expression)
        {
            Reason = reason;
            Expression = expression;
        }
    }
}
