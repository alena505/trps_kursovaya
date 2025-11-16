using BooleanCompletenessBack.BusinessLogic;
using BooleanCompletenessBack.Models;

namespace BooleanCompletenessBack.BuisnessLogic
{
    public class KMSolver:BaseSolver<KMResult>
    {
        public bool reduceOutput;
        public KMSolver(int paramsCount, int[] funcValues, bool reduceOutput) : base(paramsCount, funcValues) {
            this.reduceOutput = reduceOutput;
        }
        public override KMResult Solve()
        {
            var gen = new ParamsGenerator(paramsCount);
            int[][] paramAr = gen.Generate();
            List<KMStatement> pairs = new List<KMStatement>();
            bool belongs = true;
            Console.WriteLine(paramAr.Length);  
            for (int i = 0; i < paramAr.Length; ++i)
            {
                for (int j = i+1; j < paramAr.Length - 1; ++j)
                {
                    if (lessThan(paramAr[i], paramAr[j]))
                    {
                        bool lessOrEqual = funcValues[i] <= funcValues[j];
                        pairs.Add(new KMStatement
                        {
                            Sigma1 = paramAr[i],
                            Sigma2 = paramAr[j],
                            F1LessOrEqualF2 = lessOrEqual,
                        });
                        if (!lessOrEqual)
                        {
                            belongs = false;
                            if (reduceOutput)
                            {
                                break;
                            }
                        }

                    }
                }
                if (reduceOutput && !belongs)
                {
                    break;
                }
            }
            if (belongs && reduceOutput)
            {
                pairs = pairs.Take(5).ToList();
            }
            return new KMResult
            {
                BelongsToClass = belongs,
                Statements = pairs.ToArray(),
            };
        }

        private bool lessThan(int[] a, int[] b)
        {
            for(int i = 0; i < a.Length; ++i)
            {
                if (a[i] > b[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}



