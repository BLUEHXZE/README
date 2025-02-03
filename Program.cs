using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

var builder = WebApplication.CreateBuilder(args);

//ToDo... 
//Code for Service and Controller
//Take into account MemoryDB storage lacks persistency when registering the service.


var app = builder.Build();
//ToDo...
//Code for Controller

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
  //ToDo..
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

public interface IExamService{

	public Task<bool> Insert(MultipleChoiceQuestion q);
	public Task<bool> Update(MultipleChoiceQuestion q);
	public Task<List<MultipleChoiceQuestion>> ReadBatch(List<int> ids);
	public Task<bool> Delete(int id);
}

//ToDo: Most of the code should be here:

// ToDo: Complete ExamController

public class ExamController : Controller { 

}

// ToDo: Implement ExamService
public class ExamService : IExamService
{
    public Task<bool> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Insert(MultipleChoiceQuestion q)
    {
        throw new NotImplementedException();
    }

    public Task<List<MultipleChoiceQuestion>> ReadBatch(List<int> ids)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(MultipleChoiceQuestion q)
    {
        throw new NotImplementedException();
    }
}
//ToDo: Implement Filter here 
public class PowerUser : Attribute, IAsyncActionFilter
{
  public async Task OnActionExecutionAsync(
      ActionExecutingContext actionContext, ActionExecutionDelegate next)
  {
    //ToDo...

  } 
}