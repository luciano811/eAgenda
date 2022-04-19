using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace eAgenda.ConsoleApp.Compartilhado
{
    public class RepositorioJson<T> : ISerializavel<T>, IRepositorio<T> where T : EntidadeBase
    {
        public string Diretorio { get; init; }
        public string Arquivo { get; init; }

        public RepositorioJson()
        {
            Diretorio = Environment.CurrentDirectory + "\\Dados";

            Arquivo = Diretorio + $"\\{typeof(T).Name}.json";

            if (!Directory.Exists(Diretorio))
                Directory.CreateDirectory(Diretorio);

            if (!File.Exists(Arquivo))
                File.Create(Arquivo).Close();
        }

        public string Inserir(T entidade)
        {
            ResultadoValidacao validacao = entidade.Validar();

            if (validacao.Status == StatusValidacao.Erro)
                return validacao.ToString();

            Serializar(entidade);

            return "REGISTRO_VALIDO";
        }

        public bool Editar(Predicate<T> condicao, T novaEntidade)
        {
            List<T> registros = Deserializar();

            foreach (T entidade in registros)
            {
                if (condicao(entidade))
                {
                    novaEntidade.numero = entidade.numero;

                    int posicaoParaEditar = registros.IndexOf(entidade);
                    registros[posicaoParaEditar] = novaEntidade;

                    SerializarLista(registros);
                    return true;
                }
            }

            return false;
        }

        public bool Excluir(Predicate<T> condicao)
        {
            List<T> registros = Deserializar();

            foreach (T entidade in registros)
            {
                if (condicao(entidade))
                {
                    registros.Remove(entidade);

                    SerializarLista(registros);
                    return true;
                }
            }

            return false;
        }

        public bool ExisteRegistro(Predicate<T> condicao)
        {
            List<T> registros = Deserializar();

            foreach (T entidade in registros)
                if (condicao(entidade))
                    return true;

            return false;
        }

        public List<T> Filtrar(Predicate<T> condicao)
        {
            List<T> registros = Deserializar();

            List<T> registrosFiltrados = new List<T>();

            foreach (T entidade in registros)
                if (condicao(entidade))
                    registrosFiltrados.Add(entidade);

            return registrosFiltrados;
        }

        public T SelecionarRegistro(Predicate<T> condicao)
        {
            List<T> registros = Deserializar();

            foreach (T entidade in registros)
            {
                if (condicao(entidade))
                    return entidade;
            }

            return null;
        }

        public List<T> SelecionarTodos()
        {
            return Deserializar();
        }

        public void Serializar(T entidade)
        {
            List<T> registros = Deserializar();

            if (registros.Count > 0)
                entidade.numero = registros.OrderBy(x => x.numero).Last().numero + 1;
            else
                entidade.numero = 1;

            registros.Add(entidade);

            string jsonRegistrosAtualizados 
                = JsonConvert.SerializeObject(registros, Formatting.Indented,
                     new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore } );

            File.WriteAllText(Arquivo, jsonRegistrosAtualizados);
        }

        public void SerializarLista(List<T> registros)
        {
            string jsonRegistrosAtualizados = 
                JsonConvert.SerializeObject(registros, Formatting.Indented,
                    new JsonSerializerSettings { ReferenceLoopHandling =  ReferenceLoopHandling.Ignore} );

            File.WriteAllText(Arquivo, jsonRegistrosAtualizados);
        }

        public List<T> Deserializar()
        {
            string jsonString = File.ReadAllText(Arquivo);

            if (string.IsNullOrEmpty(jsonString))
                return new List<T>();

            return JsonConvert.DeserializeObject<List<T>>(jsonString);
        }
    }
}
