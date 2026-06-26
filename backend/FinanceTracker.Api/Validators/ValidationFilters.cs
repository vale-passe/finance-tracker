using FluentValidation;

namespace FinanceTracker.Api.Validators;

// endpoint filter denerico - intercetta la richiesta prima che raggiunga
// l'handler dell'endpoint, valida l'oggetto della request e restituisce 400
// con i dettagli degli errori se non è valido

// IValidator<T> viene iniettato dalla DI - registrato con AddValidatorsFromAssemblyContaining
public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
{
  private readonly IValidator<T> _validator = validator;

  public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
  {
    // nessun validatore registrato per questo tipo - fail esplicito
    // in fase di sviluppo invece di passare silenziosamente la richiesta non validata
    if (_validator is null)
    {
      throw new InvalidOperationException(
        $"No validator registered for type {typeof(T).Name}. " +
        $"Ensure a validator is defined and registered."
      );
    }

    // cerco l'oggetto della request tra gli argomenti dell'endpoint
    var argument = context.Arguments.OfType<T>().FirstOrDefault();

    if (argument is not null)
    {
      var result = await _validator.ValidateAsync(argument);

      // restituisco 400 con la lista degli errori (campo e messaggio)
      if (!result.IsValid)
      {
        return TypedResults.ValidationProblem(result.ToDictionary());
      }
    }

    // validazione passata - passo la richiesta all'handler
    return await next(context);
  }
}