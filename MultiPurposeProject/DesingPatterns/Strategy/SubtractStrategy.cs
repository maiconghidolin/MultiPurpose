namespace MultiPurposeProject.DesingPatterns.Strategy
{
    public class SubtractStrategy : IMathStrategy
    {

        public decimal Execute(decimal a, decimal b)
        {
            return a - b;
        }

    }
}
