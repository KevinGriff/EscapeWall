using System;

namespace EscapeWall
{
    class Escaper : IWallEscaper
    {
        char[,] _wall;
        int _timeAllowed;
        int _lastRow;
        int _lastColumn;
        int _startRowEren;
        int _startColEren;

        public void SetParameters(char[,] wall, int timeAllowed)
        {
            _wall = wall;
            _timeAllowed = timeAllowed;
            _lastRow = _wall.GetUpperBound(0);
            _lastColumn = _wall.GetUpperBound(1);

            if (!FindEren()) throw new ArgumentException("Eren not found");
        }

        public bool CanEscape(out int timeTaken)
        {
            var escaped = CanEscape(0, _startRowEren, _startColEren, ' ', out int totalTime);
            timeTaken = totalTime;
            return escaped;
        }
        private bool CanEscape(int t, int row, int col, char direction, out int timeTaken)
        {
            timeTaken = t;

            //debuggging
            //Console.WriteLine("time = " + t);
            //Console.WriteLine("row = " + row);
            //Console.WriteLine("col = " + col);

            //escaped
            if (Escaped(row, col)) return true;

            //out of time
            if (t == _timeAllowed) return false;

            //try next move
            t++;
            bool success = false;
            int quickest = _timeAllowed;

            //try moving up
            if ((direction != Constants.FROM_ABOVE) && (CanEnter(row, col, Constants.FROM_BELOW)))
            {
                if (CanEscape(t, row - 1, col, Constants.FROM_BELOW, out timeTaken))
                {
                    success = true;
                    quickest = timeTaken;
                }
            }
            //try moving right
            if ((direction != Constants.FROM_RIGHT) && (CanEnter(row, col, Constants.FROM_LEFT)))
            {
                if (CanEscape(t, row, col + 1, Constants.FROM_LEFT, out timeTaken))
                {
                    success = true;
                    quickest = Math.Min(timeTaken, quickest);
                }
            }

            //try moving down
            if ((direction != Constants.FROM_BELOW) && (CanEnter(row, col, Constants.FROM_ABOVE)))
            {
                if (CanEscape(t, row + 1, col, Constants.FROM_ABOVE, out timeTaken))
                {
                    success = true;
                    quickest = Math.Min(timeTaken, quickest);
                }
            }

            //try moving left
            if ((direction != Constants.FROM_LEFT) && (CanEnter(row, col, Constants.FROM_RIGHT)))
            //    if ((direction != Constants.FROM_LEFT) && (_wall[row, col - 1] == Constants.SAFE || _wall[row, col - 1] == Constants.FROM_RIGHT))
            {
                if (CanEscape(t, row, col - 1, Constants.FROM_RIGHT, out timeTaken))
                {
                    success = true;
                    quickest = Math.Min(timeTaken, quickest);
                }
            }

            if (success)
            {
                timeTaken = quickest;
                return true;
            }
            return false;
        }

        // return true if the current position is anywhere on the edge of the wall
        private bool Escaped(int row, int col)
        {
            return (row == 0 || col == 0 || row == _lastRow || col == _lastColumn);
        }

        // return true if the current position is anywhere on the edge of the wall
        private bool FindEren()
        {
            for (int rowCount = 0; rowCount < _lastRow + 1; rowCount++)
            {
                for (int colCount = 0; colCount < _lastColumn + 1; colCount++)
                {
                    if (_wall[rowCount, colCount] == Constants.EREN)
                    {
                        _startRowEren = rowCount;
                        _startColEren = colCount;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CanEnter(int row, int col, char from)
        {
            switch (from)
            {
                case Constants.FROM_BELOW:
                    row--;
                    break;
                case Constants.FROM_ABOVE:
                    row++;
                    break;
                case Constants.FROM_LEFT:
                    col++;
                    break;
                case Constants.FROM_RIGHT:
                    col--;
                    break;
            }

            return (_wall[row, col] == Constants.SAFE || _wall[row, col] == from);

        }
    }
}
