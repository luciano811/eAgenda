using eAgenda.ConsoleApp.Compartilhado;
using System;
using System.Collections.Generic;

namespace eAgenda.ConsoleApp.ModuloTarefa
{
    public class Tarefa : EntidadeBase
    {
        private readonly string titulo;
        private int prioridade;
        private int quantidadeItensFeitos;
        private int quantidadeItens;
        private double percentual;

        private string dataAbertura;
        private string dataFechamento;
        private bool estaConcluida;


        public string Titulo => titulo;
        public int Prioridade => prioridade;
        public bool EstaConcluida => estaConcluida;
        public int QuantidadeItensFeitos => quantidadeItensFeitos;
        public int QuantidadeItens => quantidadeItens;




        public Tarefa(string titulo, int prioridade, string dataAbertura, string dataFechamento, int quantidadeItensFeitos, int quantidadeItens, double percentual, bool estaConcluida)
        {
            this.titulo = titulo;
            this.prioridade = prioridade;
            this.dataAbertura = dataAbertura;
            this.dataFechamento = dataFechamento;
            this.quantidadeItensFeitos = quantidadeItensFeitos;
            this.quantidadeItens = quantidadeItens;
            this.percentual = percentual;
            this.estaConcluida = false;

        }

        public override string ToString()
        {
            return "Número: " + numero + Environment.NewLine +
            "Prioridade: " + prioridade + Environment.NewLine +
            "Titulo: " + titulo + Environment.NewLine +
            "Quantidade de itens realizados: " + quantidadeItensFeitos + Environment.NewLine +
            "Quantidade Total de itens: " + quantidadeItens + Environment.NewLine +
            "Percentual: " + percentual +"%"+ Environment.NewLine +
            "dataAbertura: " + dataAbertura + Environment.NewLine +
            "dataFechamento: " + dataFechamento + Environment.NewLine;

        }

        public override ResultadoValidacao Validar()
        {
            List<string> erros = new List<string>();

            if (string.IsNullOrEmpty(titulo))
                erros.Add("É necessário inserir uma titulo para as tarefas!");


            return new ResultadoValidacao(erros);
        }
    }
}
