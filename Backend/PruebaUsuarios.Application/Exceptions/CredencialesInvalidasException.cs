using System;

namespace PruebaUsuarios.Application.Exceptions
{
    public class CredencialesInvalidasException : Exception
    {
        public CredencialesInvalidasException(string mensaje) : base(mensaje) { }
    }
}
