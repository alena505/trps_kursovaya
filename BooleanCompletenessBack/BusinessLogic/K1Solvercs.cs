using BooleanCompletenessBack.BusinessLogic;
using BooleanCompletenessBack.Models;

namespace BooleanCompletenessBack.BuisnessLogic
{
    public class K1Solver:BaseSolver<K1Result>
    {
        public K1Solver(int paramsCount, int[] funcValues) : base(paramsCount, funcValues){}
        public override K1Result Solve()
        {
            var valueOnOnes = funcValues[funcValues.Length - 1];
            return new K1Result
            {
                BelongsToClass = valueOnOnes == 1,
                ValueOnOnes = valueOnOnes,
            };
        }
    }


}
