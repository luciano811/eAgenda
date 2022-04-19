using eAgenda.ConsoleApp.Compartilhado;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace eAgenda.ConsoleApp.ModuloTarefa
{
    public class TelaCadastroTarefa : TelaBase, ITelaCadastravel
    {
        private readonly Notificador notificador;
        private readonly IRepositorio<Tarefa> repositorioTarefa;

        //VER ISSOAQUI
        public bool estaAberta { get; private set; }

        public TelaCadastroTarefa(IRepositorio<Tarefa> repositorioTarefa, Notificador notificador)
            : base("Cadastro de Tarefas")
        {
            this.repositorioTarefa = repositorioTarefa;
            this.notificador = notificador;
        }

        public override string MostrarOpcoes()
        {
            MostrarTitulo(Titulo);

            Console.WriteLine("Digite 1 para Registrar");
            Console.WriteLine("Digite 2 para Editar (essa opção permite que a quantidade de itens da tarefa seja ajustada)");
            Console.WriteLine("Digite 3 para Excluir");
            Console.WriteLine("Digite 4 para Visualizar todas as Tarefas");
            Console.WriteLine("Digite 5 para Visualizar Tarefas Concluídas");

            Console.WriteLine("Digite s para sair");

            string opcao = Console.ReadLine();

            return opcao;
        }

        public void InserirRegistro()
        {
            MostrarTitulo("Inserindo nova Tarefa");
                       
            Tarefa novaTarefa = ObterTarefa();            

            string statusValidacao = repositorioTarefa.Inserir(novaTarefa);

            if (statusValidacao == "REGISTRO_VALIDO")
                notificador.ApresentarMensagem("Tarefa cadastrada com sucesso!", TipoMensagem.Sucesso);
            else
                notificador.ApresentarMensagem(statusValidacao, TipoMensagem.Erro);

            estaAberta = true;

        }

        public void EditarRegistro()
        {
            MostrarTitulo("Editando Tarefa");

            bool temFuncionariosCadastrados = VisualizarRegistros("Pesquisando");

            if (temFuncionariosCadastrados == false)
            {
                notificador.ApresentarMensagem("Nenhum funcionario cadastrado para poder editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroTarefa = ObterNumeroTarefa();

            Tarefa tarefaAtualizada = ObterTarefa();

            bool conseguiuEditar = repositorioTarefa.Editar(x => x.numero == numeroTarefa, tarefaAtualizada);

            if (!conseguiuEditar)
                notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Tarefa editada com sucesso", TipoMensagem.Sucesso);
        }

        public int ObterNumeroTarefa()
        {
            int numeroTarefa;
            bool numeroTarefaEncontrado;

            do
            {
                Console.Write("Digite o número da tarefa que deseja editar: ");
                numeroTarefa = Convert.ToInt32(Console.ReadLine());

                numeroTarefaEncontrado = repositorioTarefa.ExisteRegistro(x => x.numero == numeroTarefa);

                if (numeroTarefaEncontrado == false)
                    notificador.ApresentarMensagem("Número de tarefa não encontrado, digite novamente", TipoMensagem.Atencao);

            } while (numeroTarefaEncontrado == false);
            return numeroTarefa;
        }

        public void ExcluirRegistro()
        {
            MostrarTitulo("Excluindo Tarefa");

            bool temTarefasCadastradas = VisualizarRegistros("Pesquisando");

            if (temTarefasCadastradas == false)
            {
                notificador.ApresentarMensagem(
                    "Nenhuma tarefa cadastrada para poder excluir", TipoMensagem.Atencao);
                return;
            }

            int numeroTarefa = ObterNumeroTarefa();

            repositorioTarefa.Excluir(x => x.numero == numeroTarefa);

            notificador.ApresentarMensagem("Tarefa excluída com sucesso", TipoMensagem.Sucesso);
        }

        public bool VisualizarRegistros(string tipo)
        {
            if (tipo == "Tela")
                MostrarTitulo("Visualização de Tarefas POR PRIORIDADE");

            List<Tarefa> tarefas = repositorioTarefa.SelecionarTodos();

            if (tarefas.Count == 0)
            {
                notificador.ApresentarMensagem("Não há nenhuma tarefa disponível.", TipoMensagem.Atencao);
                return false;
            }

            //novo recurso para ordenar, linq
            List<Tarefa> tarefasPriorizadas = tarefas.OrderBy(o => o.Prioridade).ToList();

            foreach (Tarefa tarefa in tarefasPriorizadas)
                Console.WriteLine(tarefa.ToString());

            Console.ReadLine();

            return true;
        }
        
        public Tarefa ObterTarefa()
        {
            Console.Write("Digite a titulo: ");
            string titulo = Console.ReadLine();

            Console.Write("Digite a prioridade (1 - Alta, 2 - Média, 3 - Baixa): ");
            int prioridade = Convert.ToInt32(Console.ReadLine());

            Console.Write("Digite a quantidade de itens já realizados da tarefa: ");
            int quantidadeItensFeitos = Convert.ToInt32(Console.ReadLine());

            Console.Write("Digite a quantidade de itens total da tarefa: ");
            int quantidadeItens = Convert.ToInt32(Console.ReadLine());

            Console.Write("Digite a data de abertura (dd/MM/aaaa): ");
            string dataAbertura = Console.ReadLine();
            dataAbertura = FormatarEmData(dataAbertura);

            Console.Write("Digite a data de fechamento (dd/MM/aaaa): ");
            string dataFechamento = Console.ReadLine();
            dataFechamento = FormatarEmData(dataFechamento);

            //percentual
            double percentual = (Convert.ToDouble(quantidadeItensFeitos) / Convert.ToDouble(quantidadeItens)) * 100;
            Console.WriteLine($"\nA atividade está {Math.Round(percentual, 0)}% concluída. ");


            bool estaConcluida = false;

            bool tituloJaUtilizado;

            do
            {
                tituloJaUtilizado = repositorioTarefa.ExisteRegistro(x => x.Titulo == titulo);

                if (tituloJaUtilizado)
                {
                    notificador.ApresentarMensagem("Titulo já utilizado, por gentileza informe outro", TipoMensagem.Erro);

                    Console.Write("Digite o novo título: ");
                    titulo = Console.ReadLine();
                    break;
                }

            } while (tituloJaUtilizado);

            Tarefa tarefa = new Tarefa(titulo, prioridade, dataAbertura, dataFechamento, quantidadeItensFeitos, quantidadeItens,percentual, estaConcluida);

            return tarefa;
        }

        public bool VisualizarTarefasConcluidas(string tipo)
        {
            if (tipo == "Tela")
                MostrarTitulo("Visualização de Tarefas em Concluídas");

            List<Tarefa> tarefas = repositorioTarefa.Filtrar(x => x.EstaConcluida);

            if (tarefas.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhum Tarefa em aberto disponível.", TipoMensagem.Atencao);
                return false;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            foreach (Tarefa tarefa in tarefas)
            {
                if (tarefa.EstaConcluida == true)
                {
                    Console.WriteLine("ID: " + tarefa.numero);
                    Console.WriteLine("Título: " + tarefa.Titulo);
                    Console.WriteLine("Prioridade: " + tarefa.Prioridade);
                    Console.WriteLine("Ítens: \n" + tarefa.QuantidadeItens);
                }
            }
            Console.ResetColor();

            Console.ReadLine();

            return true;
        }
    }
}
