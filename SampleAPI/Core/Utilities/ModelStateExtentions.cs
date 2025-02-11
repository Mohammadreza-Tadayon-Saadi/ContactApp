using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Core.Utilities;

public static class ModelStateExtentions
{
    public static object GetErrorsForApiResult(this ModelStateDictionary modelState)
    {
        if(modelState.IsValid)
            return null;

        return modelState.Where(m => m.Value.Errors.Any())
                        .Select(m => new
                        {
                            m.Key,
                            ErrorMessage = m.Value.Errors
                                                .Select(e => e.ErrorMessage)
                                                .FirstOrDefault()
                        }).ToList();
    }
}