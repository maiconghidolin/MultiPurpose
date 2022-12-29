namespace MultiPurposeProject.DesingPatterns.Strategy
{
    public class AddStrategy : IMathStrategy
    {

        public decimal Execute(decimal a, decimal b)
        {
            return a + b;
        }

    }
}
