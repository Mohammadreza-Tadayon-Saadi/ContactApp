using System.ComponentModel.DataAnnotations;

namespace Core.Statuses;

public enum ApiResultStatusCode
{
    [Display(Name = "عملیات با موفقیت انجام شد.")]
    Success = 0,

    [Display(Name = "متاسفانه خطایی سمت سرور رخ داده است.لطفا در صورت تکرار به مدیر سیستم اطلاع دهید.")]
    ServerError = 1,

    [Display(Name = "پارامتر های ورودی نامعتبر هستند.")]
    BadRequest = 2,

    [Display(Name = "یافت نشد!")]
    NotFound = 3,

    [Display(Name = "لیست خالی است!")]
    ListEmpty = 4,

    [Display(Name = "خطایی در پردازش رخ داده است!")]
    LogicException = 5,

    [Display(Name = "دسترسی به این قسمت برای شما مجاز نیست.")]
    UnAuthorized = 6,

    [Display(Name = "شما احراز هویت نشده اید!")]
    UnAuthenticated = 7
}
