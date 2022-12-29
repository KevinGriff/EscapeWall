namespace EscapeWall
{
    interface IWallEscaper
    {
        void SetParameters(char[,] wall, int timeAllowed);

        bool CanEscape(out int timeTaken);
    }
}
