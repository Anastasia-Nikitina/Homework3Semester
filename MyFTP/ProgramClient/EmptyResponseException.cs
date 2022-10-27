namespace ProgramClient;

using System;

public class EmptyResponseException : Exception
{
    public EmptyResponseException(string message) 
        : base(message) { }
}