namespace Day06
{
    internal class Race
    {
        public Int64 Time { get; }
        public Int64 Distance { get; }
        public Race(Int64 time, Int64 distance)
        {
            Time = time;
            Distance = distance;
        }
    }
}