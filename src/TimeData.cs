namespace FluentBehaviourTree
{
    /// <summary>
    /// Represents time. Used to pass time values to behaviour tree nodes.
    /// </summary>
    public struct TimeData
    {
        public TimeData(float deltaTime)
        {
            this.deltaTime = deltaTime;
        }

        public float deltaTime;
    }
}
