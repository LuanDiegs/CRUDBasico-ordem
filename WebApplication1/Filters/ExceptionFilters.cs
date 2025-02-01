using Aplicacao.Comum.Exception.DTO;
using Infra.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Journey.Api.Filters
{
    public class ExceptionFilters: IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ExceptionBase exception)
            {
                context.HttpContext.Response.StatusCode = (int)exception.GetStatusCode();

                var responseJson = new ResponseErros(exception.GetErrorMessage());

                context.Result = new ObjectResult(responseJson);
            }
            else
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                var responseJson = new ResponseErros(["Registro não encontrado"]);

                context.Result = new ObjectResult(responseJson);
            }

        }
    }
}
