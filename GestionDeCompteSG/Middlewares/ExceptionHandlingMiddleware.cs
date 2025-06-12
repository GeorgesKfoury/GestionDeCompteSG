using GestionDeCompteSG.Contracts.Responses;

namespace GestionDeCompteSG.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(ArgumentException e)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                //context.Response.ContentType = "application/json";
                var response = new ExceptionResponse() { ErrorMessage = e.Message };
                await context.Response.WriteAsJsonAsync(response);
            }
           
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                Console.WriteLine($"Exception occurred: {ex.Message}");
                //context.Response.ContentType = "application/json";
                var response = new ExceptionResponse() { ErrorMessage = "Internal Server problem, please try again later" };
                await context.Response.WriteAsJsonAsync(response);
            }
        
        }
    }
}
