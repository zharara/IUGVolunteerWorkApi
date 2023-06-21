namespace VolunteerWorkApi.Constants
{
    public static class ErrorMessages
    {
        public const string FieldRequired = "هذا الحقل مطلوب";
        public const string AuthError = "خطأ في المصادقة";
        public const string ErrorWhileTryingToCreateAccount = "حدث خطأ أثناء محاولة إنشاء الحساب.";
        public const string ErrorWhileTryingToAuth = "حدث خطأ أثناء محاولة مصادقة الحساب.";
        public const string ErrorUserNameOrPassword = "خطأ في اسم المستخدم أو كلمة المرور";
        public const string OldAndCurrentPasswordsMatch = "كلمات المرور القديمة والجديدة تتطابق!";
        public const string ErrorTryingChangePassword = "حدث خطأ أثناء محاولة تغيير كلمة المرور";
        public const string ErrorTryingChangeUserName = "حدث خطأ أثناء محاولة تغيير اسم المستخدم";
        public const string ErrorUserNotFound = "خطأ في إيجاد المستخدم";
        public const string ThisUserNameAlreadyTaken = "اسم المستخدم هذا مأخوذ بالفعل";
        public const string PermissionsError = "خطأ صلاحيات";
        public const string NoPermissionsForAccount = "لا يوجد صلاحيات للحساب";
        public const string AccessDenied = "غير مسموح بالوصول";
        public const string NoPermissionsToAccess = "لا يوجد صلاحيات للوصول إلى المعلومات المطلوبة";
        public const string ServerError = "خطأ في خادم الإنترنت";
        public const string InternalServerErrorOccurred = "حدث خطأ داخلي في خادم الإنترنت أثناء التنفيذ.";
        public const string NotFound = "غير موجود";
        public const string DataNotFound = "لم يتم العثور على البيانات المطلوبة";
        public const string DataError = "خطأ بيانات";
        public const string DataHandlingError = "حدث خطأ أثناء معالجة البيانات";
        public const string InputError = "خطأ في الإدخال";
        public const string InputConflict = "يوجد تعارض في البيانات المدخلة";
        public const string RequiredFieldsNotInputted = "لم يتم إدخال أحد الحقول المطلوبة";
        public const string MustStateRejectionReason = "يجب تحديد سبب رفض الطلب.";
        public const string MustUploadAtLeastOneFile = "يجب رفع ملف واحد على الأقل";
        public const string OnlyOneFileAllowedToUpload = "مسموح برفع ملف واحد فقط";
        public const string AFileUploadedIsCorrupt = "تم رفع ملف معطوب";
        public const string MakeSureToFulfillApplicationRequirements = "تأكد من تلبية متطلبات التقديم";
        public const string ApplicationIsAlreadyApproved = "الطلب مقبول بالفعل";
        public const string ApplicationIsAlreadyRejected = "الطلب مرفوض بالفعل";
    }
}
