using eAgenda.ConsoleApp.Compartilhado;

namespace eAgenda.ConsoleApp.ModuloCompromisso
{
    public class RepositorioCompromisso : RepositorioBase<Compromisso>
    {
        public bool RegistrarDevolucao(Compromisso compromisso)
        {
            compromisso.Fechar();

            return true;
        }
    }
}
