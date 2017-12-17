using System;

namespace Task_2
{
    public class DuplicateTodoItemException : Exception
    {
        public DuplicateTodoItemException()
        {
        }

        public DuplicateTodoItemException(String message) : base(message)
        { 
        }

        public DuplicateTodoItemException(String message, Exception ex) : base(message, ex)
        {
        }
    }
}