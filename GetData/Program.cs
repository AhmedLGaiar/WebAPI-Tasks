using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GetData
{
    class Product
    {
        public string Name { get; set; }

        public string CatName { get; set; }

        public float Price { get; set; }

        public override string ToString()
        {
            return $"{Name} :: {CatName} :: {Price}";
        }
    }

    class ApiResponse
    {
        public bool success { get; set; }

        public Product data { get; set; }
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient httpClient = new();
            Console.Write("Enter Product Id: ");
            int id = int.Parse(Console.ReadLine());
            ApiResponse response =
                await httpClient.GetFromJsonAsync<ApiResponse>(@$"http://localhost:3184/api/Product/{id}");

            Console.WriteLine(response.data);


        }
    }
}
