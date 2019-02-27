using App01_ConsultarCEP.Servico;
using App01_ConsultarCEP.Servico.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App01_ConsultarCEP
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Botao.Clicked += Botao_Clicked;
        }

        private void Botao_Clicked(object sender, EventArgs e)
        {
            string cep = Cep.Text.Trim();

            if(ViaCEPServico.CEPehValido(cep))
            {
                try
                {
                    Endereco end = ViaCEPServico.BuscarEnderecoViaCEP(cep);
                    Resultado.Text = $"Endereço: {end.localidade}, {end.bairro}, {end.uf}, {end.logradouro}";
                }

                catch(Exception ex)
                {
                    DisplayAlert("ERRO", ex.Message, "Ok");
                }
            }

            else
            {
                DisplayAlert("ERRO", "CEP Inválido", "Ok");
            }  
        }
    }
}
