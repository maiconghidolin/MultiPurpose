namespace MultiPurposeProject.Controllers;

using Microsoft.AspNetCore.Mvc;
using MultiPurposeProject.DesingPatterns.Strategy;

[ApiController]
[Route("[controller]")]
public class StrategyController : ControllerBase
{

    public StrategyController()
    {
    }

    [HttpGet("add")]
    public ActionResult<decimal> Add(decimal a, decimal b)
    {
        MathOperation mathOperation = new(new AddStrategy());

        var result = mathOperation.ExecuteMathOperation(a, b);

        return Ok(result);
    }

    [HttpGet("subtract")]
    public ActionResult<decimal> Subtract(decimal a, decimal b)
    {
        MathOperation mathOperation = new(new SubtractStrategy());

        var result = mathOperation.ExecuteMathOperation(a, b);

        return Ok(result);
    }

    [HttpGet("multiply")]
    public ActionResult<decimal> Multiply(decimal a, decimal b)
    {
        MathOperation mathOperation = new(new MultiplyStrategy());

        var result = mathOperation.ExecuteMathOperation(a, b);

        return Ok(result);
    }

}

