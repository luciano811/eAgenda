using eAgenda.ConsoleApp.ModuloCompromisso;
using eAgenda.ConsoleApp.ModuloContato;
using eAgenda.ConsoleApp.ModuloTarefa;

using System;

namespace eAgenda.ConsoleApp.Compartilhado
{
    public class TelaMenuPrincipal
    {
        private string opcaoSelecionada;

        // Declaração de Tarefa
        private IRepositorio<Tarefa> repositorioTarefa;

        private TelaCadastroTarefa telaCadastroTarefa;




        // Declaração de Contatos
        private IRepositorio<Contato> repositorioContato;

        private TelaCadastroContato telaCadastroContato;


        // Declaração de Compromissos
        private IRepositorio<Compromisso> repositorioCompromisso;

        private TelaCadastroCompromisso telaCadastroCompromisso;


        public TelaMenuPrincipal(Notificador notificador)
        {
            repositorioTarefa = new RepositorioTarefa();
            repositorioContato = new RepositorioContato();
            repositorioCompromisso = new RepositorioCompromisso();

            telaCadastroContato = new TelaCadastroContato(
                repositorioContato,
                notificador
            );

            telaCadastroTarefa = new TelaCadastroTarefa(repositorioTarefa, notificador);


            telaCadastroCompromisso = new TelaCadastroCompromisso(
                notificador,
                repositorioCompromisso,
                repositorioContato,
                telaCadastroContato
            );

        }

        public string MostrarOpcoes()
        {
            Console.Clear();

            Console.WriteLine("eAgenda 1.0");


            Console.WriteLine();

            Console.WriteLine("Digite 1 para Cadastrar Contatos");
            Console.WriteLine("Digite 2 para Gerenciar Compromissos");
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nDigite 3 para Gerenciar Tarefas\n");
            Console.ResetColor();


            Console.WriteLine("Digite s para sair");

            opcaoSelecionada = Console.ReadLine();

            return opcaoSelecionada;
        }

        public TelaBase ObterTela()
        {
            string opcao = MostrarOpcoes();

            TelaBase tela = null;

            if (opcao == "1")
                tela = telaCadastroContato;


            else if (opcao == "2")
                tela = telaCadastroCompromisso;

            else if (opcao == "3")
                tela = telaCadastroTarefa;


            return tela;
        }
    
    }
}