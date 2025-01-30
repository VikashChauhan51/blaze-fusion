namespace BlazeFusion;
 
internal static class BlazeConsts
{
    public static class RequestHeaders
    {
        public const string Boosted = "Blaze-Boosted";
        public const string Blaze = "Blaze-Request";
        public const string OperationId = "Blaze-Operation-Id";
        public const string Payload = "Blaze-Payload";
    }

    public static class ResponseHeaders
    {
        public const string Trigger = "Blaze-Trigger";
        public const string LocationTarget = "Blaze-Location-Target";
        public const string LocationTitle = "Blaze-Location-Title";
        public const string OperationId = "Blaze-Operation-Id";
        public const string SkipOutput = "Blaze-Skip-Output";
        public const string RefreshToken = "Refresh-Antiforgery-Token";
        public const string Scripts = "Blaze-Js";
    }

    public static class ContextItems
    {
        public const string RenderedComponentIds = "blaze-all-ids";
        public const string EventName = "blaze-event";
        public const string MethodName = "blaze-method";
        public const string BaseModel = "blaze-base-model";
        public const string RequestForm = "blaze-request-form";
        public const string Parameters = "blaze-parameters";
        public const string EventData = "blaze-event-model";
        public const string EventSubject = "blaze-event-subject";
        public const string IsRootRendered = "blaze-root-rendered";
    }

    public static class Component
    {
        public const string ParentComponentId = "ParentId";
        public const string EventMethodName = "event";
    }
}

