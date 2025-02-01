namespace Aplicacao.Comum.Exception.DTO;
public class ResponseErros
{
    public IList<string> Erros { get; set; } = [];

    public ResponseErros(IList<string> erros)
    {
        Erros = erros;
    }
}
