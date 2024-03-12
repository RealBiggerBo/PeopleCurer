namespace PeopleCurer.CustomEventArgs
{
    public class IntEventArgs : EventArgs
    {
        public int IntValue { get; set; }

        public IntEventArgs(int intValue)
        {
            this.IntValue = intValue;
        }
    }
}
