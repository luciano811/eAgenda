using eAgenda.ConsoleApp.Compartilhado;
using eAgenda.ConsoleApp.ModuloContato;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace eAgenda.ConsoleApp.ModuloCompromisso
{
    public class TelaCadastroCompromisso : TelaBase
    {
        private readonly Notificador notificador;
        private readonly IRepositorio<Compromisso> repositorioCompromisso;
        private readonly IRepositorio<Contato> repositorioContato;
        private readonly TelaCadastroContato telaCadastroContato;

        public Contato contato { get; private set; }

        public TelaCadastroCompromisso(
            Notificador notificador,
            IRepositorio<Compromisso> repositorioCompromisso,
            IRepositorio<Contato> repositorioContato,
            TelaCadastroContato telaCadastroContato):base ("oliaa")
        {
            this.notificador = notificador;
            this.repositorioCompromisso = repositorioCompromisso;
            this.repositorioContato = repositorioContato;
            this.telaCadastroContato = telaCadastroContato;
        }

        public override string MostrarOpcoes()
        {
            MostrarTitulo(Titulo);

            Console.WriteLine("Digite 1 para Registrar Compromisso");
            Console.WriteLine("Digite 2 para Editar Compromisso");
            Console.WriteLine("Digite 3 para Excluir Compromisso");
            Console.WriteLine("Digite 4 para Visualizar");
            Console.WriteLine("Digite 5 para Visualizar Compromissos em Aberto");
            Console.WriteLine("Digite 6 para Devolver um compromisso");

            Console.WriteLine("Digite s para sair");

            string opcao = Console.ReadLine();

            return opcao;
        }

        public void RegistrarCompromisso()
        {
            MostrarTitulo("Inserindo novo Compromisso:");

            notificador.ApresentarMensagem("Aperte qualquer tecla para exibir contatos disponíveis para associar ao compromisso", TipoMensagem.Atencao);

            //Validação do Contato
            Contato contatoSelecionado = ObtemContato();

            if (contatoSelecionado == null)
            {
                notificador.ApresentarMensagem("Nenhum contato selecionado", TipoMensagem.Erro);
                return;
            }


            // Validação da Contato          
            if (contatoSelecionado.EstaEmCompromisso())
            {
                notificador.ApresentarMensagem("A contato selecionada já está associado a um compromisso", TipoMensagem.Erro);
                return;
            }

            Compromisso compromisso = ObtemCompromisso(contatoSelecionado);

            string statusValidacao = repositorioCompromisso.Inserir(compromisso);

            if (statusValidacao == "REGISTRO_VALIDO")
                notificador.ApresentarMensagem("Compromisso cadastrado com sucesso!", TipoMensagem.Sucesso);
            else
                notificador.ApresentarMensagem(statusValidacao, TipoMensagem.Erro);
        }

        public void RegistrarDevolucao()
        {
            MostrarTitulo("Devolvendo Compromisso");

            bool temCompromissos = VisualizarCompromissosEmAberto("Pesquisando");

            if (!temCompromissos)
            {
                notificador.ApresentarMensagem("Nenhum compromisso disponível para devolução.", TipoMensagem.Atencao);
                return;
            }

            int numeroCompromisso = ObterNumeroCompromisso();

            Compromisso compromissoParaDevolver = repositorioCompromisso.SelecionarRegistro(x => x.numero == numeroCompromisso);

            if (!compromissoParaDevolver.estaAberto)
            {
                notificador.ApresentarMensagem("O compromisso selecionado não está mais aberto.", TipoMensagem.Atencao);
                return;
            }

            (repositorioCompromisso as RepositorioCompromisso).RegistrarDevolucao(compromissoParaDevolver);

            //if (compromissoParaDevolver.contato.TemMultaEmAberto())
            //{
            //    decimal multa = compromissoParaDevolver.contato.Multa.Valor;

            //    notificador.ApresentarMensagem($"A devolução está atrasada, uma multa de R${multa} foi incluída.", TipoMensagem.Atencao);
            //}

            notificador.ApresentarMensagem("Devolução concluída com sucesso!", TipoMensagem.Sucesso);
        }

        public void EditarCompromisso()
        {
            MostrarTitulo("Editando Compromissos");

            bool temCompromissosCadastrados = VisualizarCompromissos("Pesquisando");

            if (temCompromissosCadastrados == false)
            {
                notificador.ApresentarMensagem(
                    "Nenhuma compromisso cadastrado para poder editar", TipoMensagem.Atencao);
                return;
            }
            int numeroCompromisso = ObterNumeroCompromisso();

            //Contato contatoSelecionado = ObtemContato();

            Contato contatoSelecionado = ObtemContato();

            //Compromisso compromissoAtualizado = ObtemCompromisso(contatoSelecionado, contatoSelecionado);

            //bool conseguiuEditar = repositorioCompromisso.Editar(x => x.numero == numeroCompromisso, compromissoAtualizado);

            //if (!conseguiuEditar)
            //    notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagem.Erro);
            //else
            //    notificador.ApresentarMensagem("Compromisso editado com sucesso", TipoMensagem.Sucesso);
        }

        public void ExcluirCompromisso()
        {
            MostrarTitulo("Excluindo Compromisso");

            bool temCompromissosCadastrados = VisualizarCompromissos("Pesquisando");

            if (temCompromissosCadastrados == false)
            {
                notificador.ApresentarMensagem(
                    "Nenhum compromisso cadastrado para poder excluir", TipoMensagem.Atencao);
                return;
            }

            int numeroCompromisso = ObterNumeroCompromisso();

            bool conseguiuExcluir = repositorioCompromisso.Excluir(x => x.numero == numeroCompromisso);

            if (!conseguiuExcluir)
                notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagem.Erro);
            else
                notificador.ApresentarMensagem("Contato excluída com sucesso", TipoMensagem.Sucesso);
        }

        public bool VisualizarCompromissos(string tipo)
        {
            if (tipo == "Tela")
                MostrarTitulo("Visualização de Compromissos");

            List<Compromisso> compromissos = repositorioCompromisso.SelecionarTodos();

            if (compromissos.Count == 0)
            {
                notificador.ApresentarMensagem("Não há nenhum compromisso disponível", TipoMensagem.Atencao);
                return false;
            }

            foreach (Compromisso compromisso in compromissos)
                Console.WriteLine(compromisso.ToString());

            Console.ReadLine();

            return true;
        }

        public bool VisualizarCompromissosEmAberto(string tipo)
        {
            if (tipo == "Tela")
                MostrarTitulo("Visualização de Compromissos em Aberto");

            List<Compromisso> compromissos = repositorioCompromisso.Filtrar(x => x.estaAberto);

            if (compromissos.Count == 0)
            {
                notificador.ApresentarMensagem("Nenhum compromisso em aberto disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Compromisso compromisso in compromissos)
                Console.WriteLine(compromisso.ToString());

            Console.ReadLine();

            return true;
        }

        #region Métodos privados
        private Compromisso ObtemCompromisso(Contato contato)
        {

            Console.Write("Digite o assunto do compromisso: ");
            string assunto = Console.ReadLine();

            Console.Write("Digite o local do compromisso: ");
            string local = Console.ReadLine();
            
            Console.Write("Digite a data do compromisso (dd/MM/aaaa) : ");

            string dataCompromisso = Console.ReadLine();
            DateTime dtDataCompromisso;
            
            while (!DateTime.TryParseExact(dataCompromisso, "dd/MM/yyyy", null, DateTimeStyles.None, out dtDataCompromisso))
            {
                Console.WriteLine("Invalid date, please retry");
                dataCompromisso = Console.ReadLine();
            }

            Console.Write("Digite a hora inicial do compromisso: ");
            string horaInicio = Console.ReadLine();

            Console.Write("Digite a hora final do compromisso: ");
            string horaFim = Console.ReadLine();

            bool status = true;

            Compromisso novoCompromisso = new Compromisso(assunto, local, dataCompromisso, horaInicio, horaFim, contato, status);

            return novoCompromisso;
        }

        

        private Contato ObtemContato()
        {
            bool temContatosDisponiveis = telaCadastroContato.VisualizarRegistros("Pesquisando");

            if (!temContatosDisponiveis)
            {
                notificador.ApresentarMensagem("Não há nenhuma contato disponível para cadastrar compromissos.", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o número da contato associado ao compromisso: ");
            int numeroContatoCompromisso = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            //seleciona contato na lista de contatos
            Contato contatoSelecionado = repositorioContato.SelecionarRegistro(x => x.numero == numeroContatoCompromisso);

            return contatoSelecionado;
        }

        private int ObterNumeroCompromisso()
        {
            int numeroCompromisso;
            bool numeroCompromissoEncontrado;

            do
            {
                Console.Write("Digite o número do compromisso que deseja selecionar: ");
                numeroCompromisso = Convert.ToInt32(Console.ReadLine());

                numeroCompromissoEncontrado = repositorioCompromisso.ExisteRegistro(x => x.numero == numeroCompromisso);

                if (!numeroCompromissoEncontrado)
                    notificador.ApresentarMensagem("Número de compromisso não encontrado, digite novamente", TipoMensagem.Atencao);

            } while (!numeroCompromissoEncontrado);

            return numeroCompromisso;
        }
        #endregion
    }
}
