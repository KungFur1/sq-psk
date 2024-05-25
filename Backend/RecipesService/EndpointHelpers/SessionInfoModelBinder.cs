using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RecipesService.EndpointHelpers;

public class SessionInfoModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var httpContext = bindingContext.HttpContext;
        var sessionInfo = httpContext.Items["SessionInfo"] as SessionInfoDto;

        if (sessionInfo != null)
        {
            bindingContext.Result = ModelBindingResult.Success(sessionInfo);
        }
        else
        {
            bindingContext.Result = ModelBindingResult.Failed();
        }
        
        return Task.CompletedTask;
    }
}
