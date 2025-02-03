using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

var builder = WebApplication.CreateBuilder(args);

//ToDo... 
//Code for Service and Controller
//Take into account MemoryDB storage lacks persistency when registering the service.
builder.Services.AddSingleton<MemoryDB>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddControllers();
builder.Services.AddScoped<PowerUser>();

var app = builder.Build();
//ToDo...
//Code for Controller
app.MapControllers();

app.UseStaticFiles();

app.MapGet("/", () => Results.Extensions.Html(@"
<html lang='en'>

<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Task Management</title>
</head>

<body>
    <div id='root'></div>
    <script src='./js/main.js'></script>
</body>

</html>
"));

//_______________________________________FRONT-END____ENDPOINTS:________________________________________

var questions = new List<Question> {
    // Open Questions
    new OpenQuestion { Id = 0, Statement = "______FETCH BEGIN!!!_____", Answer = ""  },
    new OpenQuestion { Id = 1, Statement = "FROM BACK END Explain the benefits of client-side rendering in web applications.", Answer = "" },
    new OpenQuestion { Id = 2, Statement = "FROM BACK END What is the purpose of Dependency Injection in software design?", Answer = "" },
    new OpenQuestion { Id = 3, Statement = "FROM BACK END Describe the difference between REST and GraphQL APIs.", Answer = ""  },
    // Multiple Choice Questions
    new MultipleChoiceQuestion { Id = 4, Statement = "FROM BACK END Which of the following is a React Hook?",
        Choices = new List<string> { "useFetch", "useState", "useClass", "useHandler" }, CorrectAnswer = 1 },
    new MultipleChoiceQuestion { Id = 5, Statement = "FROM BACK END What is the default port for HTTP?",
        Choices = new List<string> { "80", "443", "8080", "3000" }, CorrectAnswer = 0 },
    new MultipleChoiceQuestion { Id = 6, Statement = "FROM BACK END Which HTTP method is idempotent?",
        Choices = new List<string> { "POST", "DELETE", "PUT", "OPTIONS" }, CorrectAnswer = 2 },
    // Extra Open Questions
    new OpenQuestion { Id = 7, Statement = "FROM BACK END Describe the difference between Singleton, Scoped and Transient?", Answer = ""  },
    new OpenQuestion { Id = 8, Statement = "______FETCH END!!!_____", Answer = ""  },
};

app.MapGet("/api/questions", (string? kind) =>
{
    if (kind != null)
    {
        if (kind=="mcq") return Results.Ok(questions.OfType<MultipleChoiceQuestion>().ToList());
        if (kind=="open") return Results.Ok(questions.OfType<OpenQuestion>().ToList());
        return Results.BadRequest(kind);
    }
    return Results.Ok(questions.ToList<object>());
});

//______________________________________________________________________________________________________


//Middleware

app.Use(async (context, next) =>
{
    await next.Invoke();

    var path = context.Request.Path;
    if (!path.StartsWithSegments("/dummy"))
    {
        var logEntry = $"{DateTime.Now}: {context.Request.Method} {context.Request.Path} responded {context.Response.StatusCode}{Environment.NewLine}";
        await System.IO.File.AppendAllTextAsync("exam.log", logEntry);
    }
});

app.Run("http://localhost:5005");


static class ResultsExtensions
{
    public static IResult Html(this IResultExtensions resultExtensions, string html)
    {
        ArgumentNullException.ThrowIfNull(resultExtensions);

        return new HtmlResult(html);
    }
}

class HtmlResult : IResult
{
    private readonly string _html;

    public HtmlResult(string html)
    {
        _html = html;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_html);
        return httpContext.Response.WriteAsync(_html);
    }
}

//Model 

public abstract class Question {
    public int Id { get; set; }
    public string Statement { get; set; } = null!;
}

public class OpenQuestion : Question {
    public string? Answer { get; set; } // User's answer
}

public class MultipleChoiceQuestion : Question {
    public List<string> Choices { get; set; } = null!; 
    public int CorrectAnswer { get; set; } // Index of the correct answer
}

//Storage
public class MemoryDB {
    public Dictionary<int, MultipleChoiceQuestion> Exam;

    public MemoryDB() {
        Exam = new Dictionary<int, MultipleChoiceQuestion>();    
    }

    public void Seed()
    {
        int id = 0;
        Exam.Add(++id,new MultipleChoiceQuestion { Id = id, Statement = "Which of the following is a React Hook?",
        Choices = new List<string> { "useFetch", "useState", "useClass", "useHandler" }, CorrectAnswer = 1 });
        Exam.Add(++id,new MultipleChoiceQuestion { Id = id, Statement = "What is the default port for HTTP?",
        Choices = new List<string> { "80", "443", "8080", "3000" }, CorrectAnswer = 0 });
        Exam.Add(++id, new MultipleChoiceQuestion { Id = id, Statement = "Which HTTP method is idempotent?",
        Choices = new List<string> { "POST", "DELETE", "PUT", "OPTIONS" }, CorrectAnswer = 2 });

    }
}

public interface IExamService
{
    // Define the methods that need to be implemented
    void Insert(MultipleChoiceQuestion question);
    void Update(MultipleChoiceQuestion question);
    IEnumerable<MultipleChoiceQuestion> ReadBatch(IEnumerable<int> indices);
    void Delete(int id);
    void Seed();
}

//ToDo: Most of the code should be here:

// ToDo: Complete ExamController

public class ExamController : Controller
{
    private readonly IExamService _examService;

    public ExamController(IExamService examService)
    {
        _examService = examService;
    }

    [HttpGet("dummy/{id}/url")]
    public IActionResult GetDummy(int id, [FromQuery] int url)
    {
        if (id != url)
        {
            return BadRequest("ID and URL do not match.");
        }
        return Ok(new { id, url });
    }

    [HttpPost("dummy")]
    public IActionResult PostDummy([FromBody] object body)
    {
        if (body == null)
        {
            return BadRequest("Invalid data");
        }
        return Ok(body);
    }

    [HttpPost("insert")]
    [ServiceFilter(typeof(PowerUser))]
    public async Task<IActionResult> Insert([FromBody] MultipleChoiceQuestion question)
    {
        if (await _examService.Insert(question))
        {
            return Ok();
        }
        return BadRequest();
    }

    [HttpPost("update")]
    [ServiceFilter(typeof(PowerUser))]
    public async Task<IActionResult> Update([FromBody] MultipleChoiceQuestion question)
    {
        if (await _examService.Update(question))
        {
            return Ok();
        }
        return BadRequest();
    }

    [HttpGet("readbatch")]
    public async Task<IActionResult> ReadBatch([FromQuery] List<int> ids)
    {
        var result = await _examService.ReadBatch(ids);
        if (result != null)
        {
            return Ok(result);
        }
        return BadRequest();
    }

    [HttpDelete("delete/{id}")]
    [ServiceFilter(typeof(PowerUser))]
    public async Task<IActionResult> Delete(int id)
    {
        if (await _examService.Delete(id))
        {
            return Ok();
        }
        return BadRequest();
    }
}

// ToDo: Implement ExamService
public class ExamService : IExamService
{
    private readonly List<MultipleChoiceQuestion> _memoryDb;

    public ExamService()
    {
        _memoryDb = new List<MultipleChoiceQuestion>();
        Seed();
    }

    public void Insert(MultipleChoiceQuestion question)
    {
        _memoryDb.Add(question);
    }

    public void Update(MultipleChoiceQuestion question)
    {
        var index = _memoryDb.FindIndex(q => q.Id == question.Id);
        if (index != -1)
        {
            _memoryDb[index] = question;
        }
    }

    public IEnumerable<MultipleChoiceQuestion> ReadBatch(IEnumerable<int> indices)
    {
        return _memoryDb.Where(q => indices.Contains(q.Id));
    }

    public void Delete(int id)
    {
        var question = _memoryDb.FirstOrDefault(q => q.Id == id);
        if (question != null)
        {
            _memoryDb.Remove(question);
        }
    }

    public void Seed()
    {
        // Seed the memory database with initial data
        _memoryDb.AddRange(new List<MultipleChoiceQuestion>
        {
            new MultipleChoiceQuestion { Id = 1, Statement = "Sample Question 1", Choices = new List<string> { "A", "B", "C" }, CorrectAnswer = 0 },
            new MultipleChoiceQuestion { Id = 2, Statement = "Sample Question 2", Choices = new List<string> { "D", "E", "F" }, CorrectAnswer = 1 }
        });
    }
}

//ToDo: Implement Filter here 
public class PowerUser : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token) ||
            !token.ToString().Contains("PowerUser"))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }
}
