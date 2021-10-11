namespace Demo.Domain
{
    public class GuitarString : EntityBase
    {
        public GuitarString(int number, string gauge, string tuning)
        {
            Number = number;
            Gauge = gauge;
            Tuning = tuning;
            Created = DateTime.UtcNow;
        }

        public int Number { get; private set; }

        public string Gauge { get; private set; }

        public string Tuning { get; private set; }

        public DateTime Created {  get; private set; }

        public DateTime? DateRestrung {  get; private set; }

        public void ReString(string gauge)
        {
            Gauge = gauge;
            DateRestrung = DateTime.UtcNow;
        }

        public void Tune(string tuning)
        {
            Tuning = tuning;
        }
    }
}