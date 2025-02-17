namespace notebodia_api.Response
{
    public static class SimpleSuccess
    {
        public static object Default => new { success = true, message = "Success" };

        public static object WithMessage(string message) => new { success = true, message };
    }
}
