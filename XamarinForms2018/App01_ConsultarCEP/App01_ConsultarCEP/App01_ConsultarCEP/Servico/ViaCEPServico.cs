using App01_ConsultarCEP.Servico.Modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace App01_ConsultarCEP.Servico
{
    public class ViaCEPServico
    {
        private static string EnderecoURL = "http://viacep.com.br/ws/{0}/json";

        public static Endereco BuscarEnderecoViaCEP(string cep)
        {
            string url = string.Format(EnderecoURL, cep);

            WebClient wc = new WebClient();
            string conteudo = wc.DownloadString(url);

            Endereco endereco = JsonConvert.DeserializeObject<Endereco>(conteudo);

            if (endereco.cep == null)
                throw new Exception($"Endereço não encontrado para CEP {cep}");

            return endereco;
        }

        public static bool CEPehValido(string cep)
        {
            if (cep.Length != 8)
                return false;
            else if (int.TryParse(cep, out int result) == false)
                return false;

            return true;
        }
    }
}
