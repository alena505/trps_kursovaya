using BooleanCompletenessBack.Models;

namespace BooleanCompletenessBack.BusinessLogic
{
    public abstract class BaseSolver<TResult>
    {
        protected int paramsCount;
        protected int[] funcValues;
        public BaseSolver(int paramsCount, int[] funcValues)
        {
            if (funcValues.Length < 1)
            {
                throw new Exception($"Функций должно быть >= 1");
            }

            if ((int)Math.Pow(2, paramsCount) != funcValues.Length)
            {
                throw new Exception($"2**paramsCount должно быть квадратом funcValues.Length");
            }
            this.funcValues = funcValues;
            this.paramsCount = paramsCount;
            this.funcValues = funcValues;
        }
        public abstract TResult Solve();
    }
}
