using System;

namespace Task_1
{
    public class TodoAccessDeniedException : Exception
    {
        public TodoAccessDeniedException(string message) : base(message)
        {
        }

        public TodoAccessDeniedException(string message, Exception ex) : base(message, ex)
        { 
        }
    }
}