namespace BooleanCompletenessBack.Models
{
    public class KLResultAnalyticalMethod: BaseKResult
    {

        public string SourceExpression { get; set; }

        //1)Расркрываем все операции кроме И ИЛИ НЕ
        public List<ExpressionChange> RevealingComplexOperators { get; set; }

        //2)Переводим в ДНФ
        public List<ExpressionChange> DnfRetrieving { get; set; }

        //3)Преобразуем ДНФ в СДНФ

        public List<ExpressionChange> PdnfRetrieving { get; set; }

        //4)Преобраование СДНФ в АНФ(Полином Жегалкина)
        public List<ExpressionChange> AnfRetrieving { get; set; }

    }
}
