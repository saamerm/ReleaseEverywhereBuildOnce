﻿using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using SampleApp.Config;
using Xamarin.Forms;

namespace CauserException.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            HttpClientInstance = new HttpClient();
            GetInfoCommand = new Command(() => GetInfo());
        }

        private readonly HttpClient HttpClientInstance;

        private string _environmentConfig;
        public string EnvironmentConfig
        {
            get { return _environmentConfig; }
            set { SetProperty(ref _environmentConfig, value); }
        }

        public ICommand GetInfoCommand { get; }

        private async Task GetInfo()
        {
            var response = await HttpClientInstance.GetAsync(Constants.EndPoint)
                                                                   .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                string serialized = await response.Content.ReadAsStringAsync();
                Environment result = await Task.Run(() => JsonConvert.DeserializeObject<Environment>(serialized))
                                           .ConfigureAwait(false);

                EnvironmentConfig = result.Name;
                return;
            }

            EnvironmentConfig = "Failed";
        }
    }

    public class Environment
    {
        public string Name { get; set; }
    }
}