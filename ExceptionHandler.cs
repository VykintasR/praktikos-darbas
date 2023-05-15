public static class ExceptionHandler
{
    public static void Handle(Exception ex)
    {
        if (ex is AssertionException assertionEx)
        {
            Console.WriteLine("Test failed: " + assertionEx.Message);
        }
        else
        {
            Console.WriteLine("An unexpected error occurred: " + ex.Message);
        }
    }
}