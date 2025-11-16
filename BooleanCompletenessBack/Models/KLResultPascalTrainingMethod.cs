namespace BooleanCompletenessBack.Models
{
    public class KLResultPascalTrainingMethod : BaseKResult
    {
        public int[,] Triangle { get; set; }
        public Monomial[] ZhegalkinPolynomial { get; set; }
    }
}