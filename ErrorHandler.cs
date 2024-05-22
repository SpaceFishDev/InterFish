namespace InterFish
{
    public static class ErrorHandler
    {
        public static List<string> Errors = new List<string>();
        public static void HandleError(string Code, int Line, int Column)
        {
            Dictionary<string, string> errors = new();
            errors.Add("STR0", "String Not Terminated.");
            if (!errors.ContainsKey(Code))
            {
                Console.WriteLine($"No Error With Code '{Code}'\n");
                return;
            }
            Errors.Add(errors[Code] + " LN: " + Line + " COL: " + Column);
        }
    }
}