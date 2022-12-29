namespace MultiPurposeProject.DesingPatterns.Strategy
{
    public class MultiplyStrategy : IMathStrategy
    {

        public decimal Execute(decimal a, decimal b)
        {
            return a * b;
        }

    }
}
