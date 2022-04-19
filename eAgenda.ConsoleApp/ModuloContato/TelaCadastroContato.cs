using eAgenda.ConsoleApp.Compartilhado;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace eAgenda.ConsoleApp.ModuloContato
{
    public class TelaCadastroContato : TelaBase, ITelaCadastravel
    {
        private readonly IRepositorio<Contato> repositorioContato;
        private readonly Notificador notificador;

        public bool emailEhValido { get; private set; }

        public TelaCadastroContato(
            IRepositorio<Contato> repositorioContato,
            Notificador notificador) : base("Cadastro de Contatos")
        {
            this.repositorioContato = repositorioContato;
            this.notificador = notificador;
        }

        public void InserirRegistro()
        {
            MostrarTitulo("Inserindo nova contato");
            
            Contato novaContato = ObterContato();

            string statusValidacao = repositorioContato.Inserir(novaContato);

            if (statusValidacao == "REGISTRO_VALIDO")
                notificador.ApresentarMensagem("Contato inserida com sucesso", TipoMensagem.Sucesso);
            else
                notificador.ApresentarMensagem(statusValidacao, TipoMensagem.Erro);

        }

        public void EditarRegistro()
        {
            MostrarTitulo("Editando Contato");

            bool temContatosCadastradas = VisualizarRegistros("Pesquisando");

            if (temContatosCadastradas == false)
            {
                notificador.ApresentarMensagem("Nenhuma contato cadastrada para poder editar", TipoMensagem.Atencao);
                return;
            }

            int numeroContato = ObterNumeroContato();

            Console.WriteLine();
            

        }

        public void ExcluirRegistro()
        {
            MostrarTitulo("Excluindo Contato");

            bool temContatosCadastradas = VisualizarRegistros("Pesquisando");

            if (temContatosCadastradas == false)
            {
                notificador.ApresentarMensagem(
                    "Nenhuma contato cadastrada para poder excluir", TipoMensagem.Atencao);
                return;
            }

            int numeroContato = ObterNumeroContato();

            bool conseguiuExcluir = repositorioContato.Excluir(x => x.numero == numeroContato);

            if (!conseguiuExcluir)
                notificador.ApresentarMensagem("Não foi possível excluir.", TipoMensagem.Sucesso);
            else
                notificador.ApresentarMensagem("Contato excluída com sucesso", TipoMensagem.Sucesso);
        }

        public bool VisualizarRegistros(string tipo)
        {
            if (tipo == "Tela")
                MostrarTitulo("Visualização de Contatos");

            List<Contato> contatos = repositorioContato.SelecionarTodos();

            if (contatos.Count == 0)
            {
                notificador.ApresentarMensagem("Não há nenhuma contato disponível", TipoMensagem.Atencao);
                return false;
            }

            foreach (Contato contato in contatos)
                Console.WriteLine(contato.ToString());

            Console.ReadLine();

            return true;
        }

        #region Métodos privados
        //nome email telefone empresa cargo
        private Contato ObterContato()
        {
            

            Console.Write("Digite o nome do contato: ");
            string nome = Console.ReadLine();

            Console.Write("Digite o email do contato (exemplo@explo.com): ");
            string email = Console.ReadLine();

            while (emailEhValidoo(email) == false)
            {
                Console.WriteLine("Email inválido, digite um email válido...");
                email = Console.ReadLine();
            }

            Console.Write("Digite o NUMERO telefone da contato: ");
            int telefone = Convert.ToInt32(Console.ReadLine());

            Console.Write("Digite a empresa do contato: ");
            string empresa = Console.ReadLine();

            Console.Write("Digite o cargo do contato: ");
            string cargo = Console.ReadLine();



            Contato novaContato = new Contato(nome, email, telefone, empresa, cargo, emailEhValido);

            return novaContato;
        }

        
        
        private int ObterNumeroContato()
        {
            int numeroContato;
            bool numeroContatoEncontrado;

            do
            {
                Console.Write("Digite o número da contato que deseja selecionar: ");
                numeroContato = Convert.ToInt32(Console.ReadLine());

                numeroContatoEncontrado = repositorioContato.ExisteRegistro(x => x.numero == numeroContato);

                if (numeroContatoEncontrado == false)
                    notificador.ApresentarMensagem("Número de contato não encontrado, digite novamente", TipoMensagem.Atencao);

            } while (numeroContatoEncontrado == false);

            return numeroContato;
        }

        private static bool emailEhValidoo(string emaiou)
        {
            try
            {
                MailAddress mail = new MailAddress(emaiou);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
