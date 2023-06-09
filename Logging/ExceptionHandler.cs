﻿namespace Bezdzione.Logging
{
    public static class ExceptionHandler
    {
        public static void Handle(Exception ex)
        {
            switch (ex)
            {
                case AssertionException assertionEx:
                    ConsoleLogger.TestFail(assertionEx.Message);
                    break;
                case ArgumentException argumentEx:
                    ConsoleLogger.Exception(argumentEx.Message);
                    break;
                default:
                    ConsoleLogger.Error(ex.Message);
                    break;
            }
        }
    }
}