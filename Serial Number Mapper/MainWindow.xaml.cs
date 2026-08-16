using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Serial_Number_Mapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _selectedFilePath;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog selectSKU = new OpenFileDialog()
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    Title = "Select a CSV file"
                };
                if (selectSKU.ShowDialog() == true)
                {
                    _selectedFilePath = selectSKU.FileName;
                    string[] lines = File.ReadAllLines(_selectedFilePath);
                    csvContent.Text = string.Join(Environment.NewLine, lines);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async void btnProcessCSV_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedFilePath) || !File.Exists(_selectedFilePath))
            {
                MessageBox.Show("Please select a CSV file first.");
                return;
            }

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "${API_ENDPOINT}");

            request.Headers.Add("authUsername", "$({AUTH_USERNAME})");
            request.Headers.Add("authPassword", "$({AUTH_PASSWORD})");

            request.Headers.Add("authUsername", "");
            request.Headers.Add("authPassword", "");

            var content = new StreamContent(File.OpenRead(_selectedFilePath));
            request.Content = content;
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Response: {responseContent}");
            }
            else
            {
                MessageBox.Show($"Error: {response.StatusCode}");
            }
        }
    }
}
