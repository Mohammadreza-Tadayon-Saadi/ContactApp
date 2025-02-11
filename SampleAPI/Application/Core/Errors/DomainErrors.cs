namespace Application.Core.Errors;

public static class DomainErrors
{
    /// <summary>
    /// Contains the Common errors.
    /// </summary>
    public static class Common
    {
        public static Error NullOrEmpty(ErrorType errorType = ErrorType.PropertyValidation, string propertyName = "") 
            => new Error(propertyName.HasValue() ? propertyName : $"Inside{errorType}", "لطفا مقدار را وارد کنید.");

        public static Error NotValid(ErrorType errorType = ErrorType.PropertyValidation, string propertyName = "") 
            => new Error(propertyName.HasValue() ? propertyName : $"Inside{errorType}", "مقدار وارد شده صحیح نمیباشد.");

        public static Error Concurrency(ErrorType errorType = ErrorType.PropertyValidation, string propertyName = "")
        {
           var e =  new Error(propertyName.HasValue() ? propertyName : $"Inside{errorType}", ContextTransactionStatus.Concurrency.ToDisplay());
            return e;
        }
    }
    
    /// <summary>
    /// Contains the Price errors.
    /// </summary>
    public static class Price
    {
        public static Error NotValid(int minPrice, ErrorType errorType = ErrorType.PropertyValidation, string propertyName = "") 
            => new Error(propertyName.HasValue() ? propertyName : $"Inside{errorType}", $"قیمت نمیتواند کمتر از {minPrice} تومان باشد.");
    }

    /// <summary>
    /// Contains the Price errors.
    /// </summary>
    public static class Weight
    {
        public static Error NotValid(int minWeight, ErrorType errorType = ErrorType.PropertyValidation, string propertyName = "") 
            => new Error(propertyName.HasValue() ? propertyName : $"Inside{errorType}", $"وزن نمیتواند کمتر از {minWeight} گرم باشد.");
    }

    public static class Entity
    {
        public static Error NotExists(string propertyName = "", string message = "")
            => new Error(propertyName.HasValue() ? propertyName : "Entity",
                                      message.HasValue() ? message : ContextTransactionStatus.NotExists.ToDisplay(),
                                      ErrorType.Entity);
        public static Error AlreadyExists(string propertyName = "", string message = "")
            => new Error(propertyName.HasValue() ? propertyName : "Entity",
                                      message.HasValue() ? message : ContextTransactionStatus.AlreadyExists.ToDisplay(),
                                      ErrorType.Entity);
        public static Error Concurrency(string propertyName = "", string message = "")
            => new Error(propertyName.HasValue() ? propertyName : "Entity",
                                      message.HasValue() ? message : ContextTransactionStatus.Concurrency.ToDisplay(),
                                      ErrorType.Entity);
        public static Error Timeout(string propertyName = "", string message = "")
            => new Error(propertyName.HasValue() ? propertyName : "Entity",
                                      message.HasValue() ? message : ContextTransactionStatus.Timeout.ToDisplay(),
                                      ErrorType.Entity);
        public static Error ForeignKey(string propertyName = "", string message = "")
            => new Error(propertyName.HasValue() ? propertyName : "Entity",
                                      message.HasValue() ? message : ContextTransactionStatus.ForeignKey.ToDisplay(),
                                      ErrorType.Entity);
        public static Error DeleteRelation(string propertyName = "", string message = "")
            => new Error(propertyName.HasValue() ? propertyName : "Entity",
                                      message.HasValue() ? message : ContextTransactionStatus.DeleteRelation.ToDisplay(),
                                      ErrorType.Entity);
        public static Error ServerError(string propertyName = "", string message = "")
            => new Error(propertyName.HasValue() ? propertyName : "Entity",
                                      message.HasValue() ? message : ContextTransactionStatus.ServerError.ToDisplay(),
                                      ErrorType.Entity);
        public static Error Unknown(string propertyName = "", string message = "")
            => new Error(propertyName.HasValue() ? propertyName : "Entity",
                                      message.HasValue() ? message : ContextTransactionStatus.Failed.ToDisplay(),
                                      ErrorType.Entity);
    }
}
