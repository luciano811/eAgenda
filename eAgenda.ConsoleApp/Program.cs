using eAgenda.ConsoleApp.Compartilhado;
using eAgenda.ConsoleApp.ModuloCompromisso;
using eAgenda.ConsoleApp.ModuloTarefa;

namespace eAgenda.ConsoleApp
{
    internal class Program
    {
        static Notificador notificador = new Notificador();
        static TelaMenuPrincipal menuPrincipal = new TelaMenuPrincipal(notificador);

        static void Main(string[] args)
        {
            while (true)
            {
                TelaBase telaSelecionada = menuPrincipal.ObterTela();

                if (telaSelecionada is null)
                    return;

                string opcaoSelecionada = telaSelecionada.MostrarOpcoes();

                if (telaSelecionada is ITelaCadastravel)
                    GerenciarCadastroBasico(telaSelecionada, opcaoSelecionada);

                else if (telaSelecionada is TelaCadastroCompromisso)
                    GerenciarCadastroCompromissos(telaSelecionada, opcaoSelecionada);

                else if (telaSelecionada is TelaCadastroTarefa)
                    GerenciarCadastroTarefas(telaSelecionada, opcaoSelecionada);

            }
        }

       

        private static void GerenciarCadastroCompromissos(TelaBase telaSelecionada, string opcaoSelecionada)
        {
            TelaCadastroCompromisso telaCadastroCompromisso = telaSelecionada as TelaCadastroCompromisso;

            if (telaCadastroCompromisso is null)
                return;

            if (opcaoSelecionada == "1")
                telaCadastroCompromisso.RegistrarCompromisso();

            else if (opcaoSelecionada == "2")
                telaCadastroCompromisso.EditarCompromisso();

            else if (opcaoSelecionada == "3")
                telaCadastroCompromisso.ExcluirCompromisso();

            else if (opcaoSelecionada == "4")
                telaCadastroCompromisso.VisualizarCompromissos("Tela");

            else if (opcaoSelecionada == "5")
                telaCadastroCompromisso.VisualizarCompromissosEmAberto("Tela");

            else if (opcaoSelecionada == "6")
                telaCadastroCompromisso.RegistrarDevolucao();
        }

        private static void GerenciarCadastroTarefas(TelaBase telaSelecionada, string opcaoSelecionada)
        {
            TelaCadastroTarefa telaCadastroTarefa = telaSelecionada as TelaCadastroTarefa;

            if (telaCadastroTarefa is null)
                return;

            if (opcaoSelecionada == "1")
                telaCadastroTarefa.InserirRegistro();

            else if (opcaoSelecionada == "2")
                telaCadastroTarefa.EditarRegistro();

            else if (opcaoSelecionada == "3")
                telaCadastroTarefa.ExcluirRegistro();

            else if (opcaoSelecionada == "4")
                telaCadastroTarefa.VisualizarRegistros("Tela");

            else if (opcaoSelecionada == "5")
                telaCadastroTarefa.VisualizarTarefasConcluidas("Tela");

            
        }

        public static void GerenciarCadastroBasico(TelaBase telaSelecionada, string opcaoSelecionada)
        {
            ITelaCadastravel telaCadastroBasico = telaSelecionada as ITelaCadastravel;

            if (telaCadastroBasico is null)
                return;

            if (opcaoSelecionada == "1")
                telaCadastroBasico.InserirRegistro();

            else if (opcaoSelecionada == "2")
                telaCadastroBasico.EditarRegistro();

            else if (opcaoSelecionada == "3")
                telaCadastroBasico.ExcluirRegistro();

            else if (opcaoSelecionada == "4")
                telaCadastroBasico.VisualizarRegistros("Tela");

        }
    }
}
