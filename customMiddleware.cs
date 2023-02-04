using System.Net;
using System.Text.Json;
using chessAPI.dataAccess.interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace chessAPI;

public class customMiddleware<TC>
        where TC : struct
{
    private readonly RequestDelegate next;

    /// <summary>
    /// Constructor del middleware
    /// </summary>
    /// <param name="next"></param>
    public customMiddleware(RequestDelegate next) => this.next = next;

    /// <summary>
    /// Customización del middleware de .NetCore para inyectar un modelo de "unit of work"
    /// </summary>
    /// <param name="context"></param>
    /// <param name="rkm"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context, IRelationalContext<TC> rkm)
    {
        try
        {
            if (rkm != null)
            {
                rkm.getTransactionContext();
                await next(context).ConfigureAwait(false);
                rkm.commitTransactionContext();
            }
            else
            {
                throw new ArgumentNullException(nameof(rkm));
            }
        }
        catch (ApplicationException ex)
        {
            if (rkm != null) rkm.rollbackTransaction(ex);
            if (context != null) await handleExceptionAsync(context, ex).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            if (rkm != null)
            {
                rkm.rollbackTransaction(ex);
            }
            if (context != null) await handleExceptionAsync(context, ex).ConfigureAwait(false);
        }
        finally
        {
            if (rkm != null)
            {
                rkm.closeTransactionContext();
            }
        }
    }

    /// <summary>
    /// Manejo de expeción para no enviar al frontend un stack de error del backend
    /// </summary>
    /// <param name="context"></param>
    /// <param name="ex"></param>
    /// <returns>Información amigable y segura que puede procesar el frontend y por ende el usuario final</returns>
    private static Task handleExceptionAsync(HttpContext context, Exception ex)
    {
        var code = HttpStatusCode.InternalServerError; // 500 if unexpected
        var result = string.Empty;
        if (ex is System.ArgumentException)
        {
            code = HttpStatusCode.BadRequest;
            result = JsonSerializer.Serialize(new errorMessage(ex.Message));
        }
        else if (ex is System.NullReferenceException)
        {
            code = HttpStatusCode.NotFound;
            result = JsonSerializer.Serialize(new errorMessage("The requested resource could not be found."));
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}