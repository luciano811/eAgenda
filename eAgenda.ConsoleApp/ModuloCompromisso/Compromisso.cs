using eAgenda.ConsoleApp.Compartilhado;
using eAgenda.ConsoleApp.ModuloContato;
using System;
using System.Collections.Generic;

namespace eAgenda.ConsoleApp.ModuloCompromisso
{
    public class Compromisso : EntidadeBase
    {
        public Contato contato;
        private string assunto;
        private string local;
        private string dataCompromisso;
        private string horaInicio;
        private string horaFim;
        private bool status = false;

        
        public Compromisso(string assunto, string local, string dataCompromisso, string horaInicio, string horaFim, Contato contato, bool status)
        {
            this.assunto = assunto;
            this.local = local;
            this.dataCompromisso = dataCompromisso;
            this.horaInicio = horaInicio;
            this.horaFim = horaFim;
            this.contato = contato;
            this.status = status;

        }

        public string Assunto { get => assunto; set => assunto = value; }
        public string Local { get => local; set => local = value; }
        public string DataCompromisso { get => dataCompromisso; set => dataCompromisso = value; }
        public string HoraInicio { get => horaInicio; set => horaInicio = value; }
        public string HoraFim { get => horaFim; set => horaFim = value; }
        public Contato Contato { get => contato; set => contato = value; }
        public bool Status { get => status; set => status = value; }
        public bool estaAberto { get; internal set; }

        public override string ToString()
        {
            return "Número: " + numero + Environment.NewLine +
                "Contato do Compromisso: " + contato.Nome + Environment.NewLine +
                "Assunto compromisso: " + assunto + Environment.NewLine +
                "Local do compromisso: " + local + Environment.NewLine+
                "Data do compromisso: " + dataCompromisso + Environment.NewLine +
                "Hora inicial do compromisso: " + horaInicio + Environment.NewLine +
                "Hora final do compromisso: " + horaFim + Environment.NewLine +
                "Status do compromisso: " + status + Environment.NewLine;
        }

        public void Abrir()
        {
            //if (!estaAberto)
            //{
            //    estaAberto = true;
            //    dataCompromisso = DateTime.Today;
            //    //dataDevolucao = dataCompromisso.AddDays();

            //    contato.RegistrarCompromisso(this);
            //}
        }

        public void Fechar()
        {
            //if (estaAberto)
            //{
            //    estaAberto = false;

            //    DateTime dataRealDevolucao = DateTime.Today;

            //    bool devolucaoAtrasada = dataRealDevolucao > horaFim;

            //    if (devolucaoAtrasada)
            //    {
            //        int diasAtrasados = (dataRealDevolucao - horaFim).Days;

            //        decimal valorMulta = 10 * diasAtrasados;

            //    }
            //}
        }

        public override ResultadoValidacao Validar()
        {
            return new ResultadoValidacao(new List<string>());
        }
    }
}
