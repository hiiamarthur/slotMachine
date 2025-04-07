namespace Blazesoft.SlotMachine.Domain.ValueObjects
{
   
    public class SpinResult
    {
        public int[,] Matrix { get; }
        public int Win { get; set; }
        public SpinResult(int[,] matrix)
        {
            Matrix = matrix;
        }

        static public IEnumerable<int> GetDiagonalValues(int[,] matrix, int startRow, int colLen, int rowLen, int startValue)
        {
            int currentRow = startRow;
            bool downward = true;

            for (int col = 0; col < colLen; col++)
            {
                int value = matrix[currentRow, col];
                if (value != startValue)
                    yield break;
                yield return value;

                if ((downward && currentRow == rowLen - 1) || (!downward && currentRow == 0))
                {
                    downward = !downward;
                }
                currentRow += downward ? 1 : -1;
            }
        }

        public int CalculateTotalWin() {
            var rowLen = Matrix.GetLength(0);
            var colLen = Matrix.GetLength(1);
            var totalHorizontalWin = 0;
            var totalDiagonalWin = 0;
            for (var i = 0; i < rowLen; i++) {
                var startValue = Matrix[i,0];
                var duplicateRow = Enumerable.Range(0, colLen)
                    .Select(x => Matrix[i,x])
                .TakeWhile(val => val == startValue);
                totalHorizontalWin += duplicateRow.Count() > 2 ? duplicateRow.Sum() : 0;
                var j = i;
                var duplicateDiagonal = GetDiagonalValues(Matrix, i, colLen, rowLen, startValue);
                totalDiagonalWin += duplicateDiagonal.Count() > 2 ? duplicateDiagonal.Sum() : 0;
            }
            Win = totalHorizontalWin + totalDiagonalWin;
            return Win;
        }

    }
}
