using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ImagesService.EndpointHelpers;

public class SessionInfoModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        if (context.Metadata.ModelType == typeof(SessionInfoDto))
            return new SessionInfoModelBinder();

        return null;
    }
}
