namespace PruebaUsuarios.Application.Exceptions;

public class ValidacionException : Exception
{
    public ValidacionException(string mensaje) : base(mensaje) { }
}
