using System.ComponentModel.DataAnnotations;
namespace Core.Statuses;

public enum ContextTransactionStatus
{
	[Display(Name = "عملیات با موفقیت انجام شد.")]
	Success = 0,

	[Display(Name = "متاسفانه خطایی سمت سرور رخ داده است، لطفا صفحه را مجددا بارگذاری کنید و در صورت تکرار به مدیر سیستم اطلاع دهید.")]
	ServerError = 1,

	[Display(Name = "متاسفانه داده های فرستاده شده با داده های اصلی مغایرت دارد، لطفا دوباره امتحان کنید.")]
	Concurrency = 2,

	[Display(Name = "متاسفانه به دلیل وابستگی داشتن داده به داده های دیگر، امکان حذف داده وجود ندارد.")]
	DeleteRelation = 3,

	[Display(Name = "متاسفانه نوع رابطه قابل ثبت نمیباشد، لطفا صفحه را مجدد بارگذاری کرده و دوباره امتحان  کنید.")]
	ForeignKey = 4,

	[Display(Name = "متاسفانه به دلیل طول کشیدن بیش از حد، عملیات مورد نظر لغو شد.")]
	Timeout = 5,

	[Display(Name = "متاسفانه درخواست مورد نظر انجام نشد، لطفا اتصالتان را بررسی کنید و در صورت تکرار به مدیر سیستم اطلاع دهید.")]
	Failed = 6,

    [Display(Name = "متاسفانه داده ای پیدا نشد.")]
    NotExists = 7,

    [Display(Name = "چنین داده ای قبلا ثبت شده است و امکان ثبت مجدد وجود ندارد.")]
    AlreadyExists = 8,
}