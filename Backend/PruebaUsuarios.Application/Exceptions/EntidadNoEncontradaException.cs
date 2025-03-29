namespace PruebaUsuarios.Application.Exceptions;

public class EntidadNoEncontradaException : Exception
{
    public EntidadNoEncontradaException(string mensaje) : base(mensaje) { }
}
