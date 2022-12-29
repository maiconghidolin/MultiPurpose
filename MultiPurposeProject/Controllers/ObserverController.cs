namespace MultiPurposeProject.Controllers;

using Microsoft.AspNetCore.Mvc;
using MultiPurposeProject.DesingPatterns.Observer;

[ApiController]
[Route("[controller]")]
public class ObserverController : ControllerBase
{

    public ObserverController()
    {
    }

    [HttpGet]
    public ActionResult Get()
    {
        Editor editor = new();

        LogSubscriber logSubscriber = new("log.txt", "Someone has opened the file:");
        editor.publisher.Subscribe("OpenFile", logSubscriber);
        
        LogSubscriber logSubscriber2 = new("log2.txt", "Someone has opened the file:");
        editor.publisher.Subscribe("OpenFile", logSubscriber2);

        EmailSubscriber emailSubscriber = new("maicon.ghidolin@gmail.com", "Someone has saved the file:");
        editor.publisher.Subscribe("SaveFile", emailSubscriber);

        editor.OpenFile("test.txt");

        editor.publisher.Unsubscribe("OpenFile", logSubscriber2);

        editor.OpenFile("file.txt");
        editor.SaveFile("file.txt");

        return Ok();
    }

}

