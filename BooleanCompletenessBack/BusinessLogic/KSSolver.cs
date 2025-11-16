using BooleanCompletenessBack.BusinessLogic;
using BooleanCompletenessBack.Models;

namespace BooleanCompletenessBack.BuisnessLogic
{
    public class KSSolver:BaseSolver<KSResult>
    {
        public bool reduceOutput;
        public KSSolver(int paramsCount, int[] funcValues, bool reduceOutput) : base(paramsCount, funcValues) {
            this.reduceOutput = reduceOutput;
        }
        public override KSResult Solve()
        {
            var gen = new ParamsGenerator(paramsCount);
            int[][] paramAr = gen.Generate();
            List<KSStatement> pairs = new List<KSStatement>();
            bool belongs = true;
            for (int i = 0; i < paramAr.Length / 2; ++i)
            {
                int j = paramAr.Length - i - 1;

                bool opposite = funcValues[i] != funcValues[j];

                pairs.Add(new KSStatement
                {
                    Sigma1 = paramAr[i],
                    Sigma2 = paramAr[j],
                    F1IsOppositeToF2 = opposite,
                });

                if (!opposite)
                {
                    belongs = false;
                    if (reduceOutput)
                    {
                        break;
                    }
                }


            }
            if(belongs && reduceOutput)
            {
                pairs = pairs.Take(5).ToList();
            }
            return new KSResult
            {
                BelongsToClass = belongs,
                Statements = pairs.ToArray(),
            };
        }

    }
}



