namespace BooleanCompletenessBack.BuisnessLogic
{
    public class ParamsGenerator
    {
        private int count;

        public ParamsGenerator(int count)
        {
            this.count = count;
        }

        public int[][] Generate()
        {
            int cnt = (int)Math.Pow(2, count);
            int[][] res = new int[cnt][];

            int[] current = new int[count];
            for(int i = 0; i < count; ++i)
            {
                current[i] = 0;
            }
            for(int i = 0; i < cnt; ++i)
            {
                res[i] = current;
                current = next(current);
            }
            return res;
        }

        private int[] next(int[] current)
        {
            int[] result = new int[current.Length];
            for(int i = 0; i < current.Length; ++i)
            {
                result[i] = current[i];
            }
            int zeroIndex = result.Length - 1;
            while (zeroIndex >= 0 && result[zeroIndex] == 1){
                zeroIndex--;
            }
            if(zeroIndex < 0)
            {
                return null;
            }
            result[zeroIndex] = 1;
            for(int i = zeroIndex + 1; i < result.Length; ++i)
            {
                result[i] = 0;
            }
            return result;
        }
    }
}
