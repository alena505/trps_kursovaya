namespace BooleanCompletenessBack.Models
{
    public class SolveInput
    {

        public int ParamsCount { get; set; }

        public int[][] Fs { get; set; }
        public List<string>? FsFormulas { get; set; }

        public bool ReduceOutput { get; set; }

        public void Validate()
        {

            if(FsFormulas != null && FsFormulas.Count != Fs.Length)
            {
                throw new Exception("Число формул (если задано) должно быть равно числу функций (заданных таблицей истинности)");
            }

            if (Fs.Length < 2)
            {
                throw new Exception("Число функций должно быть 2 или более");
            }
            if(ParamsCount < 1 || ParamsCount > 10)
            {
                throw new Exception($"Число параметров должно быть от 1 до 10. У нас {ParamsCount}");
            }
            int linesCount = (int) Math.Pow(2, ParamsCount);
            for(int i = 0; i < Fs.Length; ++i)
            {
                if (Fs[i].Length != linesCount)
                {
                    throw new Exception($"В каждой функции должно быть {linesCount} значений," + $"Для {ParamsCount} параметров, но это не так для функции №{ i + 1}");
                }
                for(int j = 0; j < Fs[i].Length; ++j)
                {
                    int v = Fs[i][j];
                    if(v < 0 || v > 1)
                    {
                        throw new Exception($"В качестве значений функций допустимы 0 и 1, но " + $"в функции №{i + 1} испольуется значение {v} {j + 1}-ым по счёту");
                    }
                }
            }
        }



    }
}
