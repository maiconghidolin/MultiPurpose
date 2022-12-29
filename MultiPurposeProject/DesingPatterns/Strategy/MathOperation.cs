namespace MultiPurposeProject.DesingPatterns.Strategy
{
    public class MathOperation
    {

        private IMathStrategy _mathStrategy;

        public MathOperation()
        {
        }
        
        public MathOperation(IMathStrategy mathStrategy)
        {
            _mathStrategy = mathStrategy;
        }

        public void SetMathStrategy(IMathStrategy mathStrategy)
        {
            _mathStrategy = mathStrategy;
        }

        public decimal ExecuteMathOperation(decimal a, decimal b)
        {
            return _mathStrategy.Execute(a, b);
        }

    }
}
