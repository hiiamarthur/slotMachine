namespace Blazesoft.SlotMachine.Common
{
    public static class Extensions
    {
        public static int[][] ToJagged(this int[,] twoDimArray)
        {
            int rows = twoDimArray.GetLength(0);
            int cols = twoDimArray.GetLength(1);

            int[][] jagged = new int[rows][];
            for (int i = 0; i < rows; i++)
            {
                jagged[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    jagged[i][j] = twoDimArray[i, j];
                }
            }
            return jagged;
        }
    }
}
