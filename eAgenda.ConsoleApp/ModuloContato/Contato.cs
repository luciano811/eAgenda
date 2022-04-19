using eAgenda.ConsoleApp.Compartilhado;
using eAgenda.ConsoleApp.ModuloCompromisso;
using System;
using System.Collections.Generic;

namespace eAgenda.ConsoleApp.ModuloContato
{
    public class Contato : EntidadeBase
    {
        //nome email telefone empresa cargo
        private readonly string nome;
        private readonly string email;
        private readonly bool emailEhValido;
        private readonly int telefone;
        private readonly string empresa;
        private readonly string cargo;



        public List<Compromisso> historicoCompromissos = new List<Compromisso>();

        public string Nome => nome;

        public int Edicao => telefone;


        public Contato(string nome, string email, int telefone, string empresa, string cargo, bool emailEhValido)
        {
            this.nome = nome;
            this.email = email;
            this.telefone = telefone;
            this.empresa = empresa; 
            this.cargo = cargo;
            this.emailEhValido = emailEhValido;

        }

        public override string ToString()
        {
            return "Número: " + numero + Environment.NewLine +
                "Email: " + email + Environment.NewLine +
                "Telefone: " + telefone + Environment.NewLine +
                "Empresa: " + empresa + Environment.NewLine +
                "Cargo: " + cargo + Environment.NewLine;
        }

        public override ResultadoValidacao Validar()
        {
            List<string> erros = new List<string>();

            if (string.IsNullOrEmpty(Nome))
                erros.Add("É necessário incluir uma coleção!");

            if (Edicao < 0)
                erros.Add("A edição de uma contato não pode ser menor que zero!");

            //if (Ano < 0 || Ano > DateTime.Now.Year)
            //    erros.Add("O ano da contato precisa ser válido!");

            return new ResultadoValidacao(erros);
        }

        
        public void RegistrarCompromisso(Compromisso compromisso)
        {
            historicoCompromissos.Add(compromisso);
        }

        //public bool EstaReservada()
        //{
        //    bool temReservaEmAberto = false;

        //    foreach (Reserva reserva in historicoReservas)
        //    {
        //        if (reserva != null && reserva.EstaAberta)
        //        {
        //            temReservaEmAberto = true;
        //            break;
        //        }
        //    }

        //    return temReservaEmAberto;
        //}

        public bool EstaEmCompromisso()
        {
            bool temCompromissoEmAberto = false;

            foreach (Compromisso compromisso in historicoCompromissos)
            {
                if (compromisso != null && compromisso.estaAberto)
                {
                    temCompromissoEmAberto = true;
                    break;
                }
            }
            return temCompromissoEmAberto;
        }
    }
}