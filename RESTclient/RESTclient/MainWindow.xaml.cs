using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RESTclient.Models;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Net;

namespace RESTclient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        static HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("http://localhost:61091/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        

        public void bAdd_Click(object sender, RoutedEventArgs e)
        {
            Persons person = new Persons();
            person = SetPersonsValue();

            var url = CreateProductAsync(person);
            tMessage.Text = $"Created at {url}";
        }

        static async Task<Uri> CreateProductAsync(Persons person)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/listitems/", person);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task<List<Persons>> GetAllPersonsAsync()
        {
            List<Persons> persons = null;

            HttpResponseMessage response = await client.GetAsync("api/listitems/");
            if (response.IsSuccessStatusCode)
            {
                persons = await response.Content.ReadAsAsync<List<Persons>>();
            }
            return persons;
        }

        public Persons SetPersonsValue()
        {
            Persons persons = new Persons();
            Pets pet = new Pets();
            Cars car = new Cars();

            persons.name = tName.Text;
            
            persons.surname = tSurname.Text;

            persons.city = tCity.Text;


            if (tYear.Text != "")
            {
                persons.year = int.Parse(tYear.Text);
            }
            else
            {
                persons.year = 0;
            }


            car.theCarBrand = tBrand.Text;
            car.color = tColor.Text;
            if (tCarYer.Text != "")
            {
                car.year = int.Parse(tCarYer.Text);
            }
            else
                car.year = 0;

            if (tMileage.Text != "")
            {
                car.mileage = int.Parse(tMileage.Text);
            }
            else
                car.mileage = 0;

            pet.animal = tAnimal.Text;
            pet.name = tAnimalName.Text;
            if (tAnimalYear.Text != "")
            {
                pet.year = int.Parse(tAnimalYear.Text);
            }
            else
                pet.year = 0;

            persons.car = car;
            persons.pet = pet;


            return persons;
        }

        static async Task<Persons> GetPersonAsync(string path)
        {
            Persons product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Persons>();
            }
            return product;
        }

        static async Task<Persons> UpdatePersonAsync(Persons person)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/listitems/{person.id}", person);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            person = await response.Content.ReadAsAsync<Persons>();
            return person;
        }

        static async Task<HttpStatusCode> DeletePersonAsync(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/listitems/{id}");
            return response.StatusCode;
        }


        public async void bGetAll_Click(object sender, RoutedEventArgs e)
        {

            List<Persons> persons = await GetAllPersonsAsync();
            AddValueToListBox(persons);
            

        }

       public void AddValueToListBox(List<Persons> persons)
        {

                personsList.ItemsSource = persons;

        }

        public int GetValueIdfromListBox()
        {
            try
            {
                Persons persons = personsList.SelectedItem as Persons;
                return persons.id;
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return 0;
        }
        public void  SetValuesToTexBoxs(Persons person)
        {
              tName.Text = person.name;
              tSurname.Text = person.surname;
              tCity.Text = person.city;
              tYear.Text = person.year.ToString();

           
              tBrand.Text = person.car.theCarBrand;
              tColor.Text = person.car.theCarBrand;
              tCarYer.Text = person.car.year.ToString();
              tMileage.Text = person.car.mileage.ToString();


              tAnimal.Text = person.pet.animal;
              tAnimalName.Text = person.pet.name;
              tAnimalYear.Text= person.pet.year.ToString();
        }

        private async void bGetOne_Click(object sender, RoutedEventArgs e)
        {
            int idValue = GetValueIdfromListBox();
            if (idValue != 0)
            {
                string path = "api/listitems/" + idValue.ToString();
                Persons person = await GetPersonAsync(path);
                SetValuesToTexBoxs(person);
            }
        }

        private async void bEdit_Click(object sender, RoutedEventArgs e)
        {

            int idValue = GetValueIdfromListBox();
            if (idValue != 0)
            {
                Persons person = SetPersonsValue();
                person.id = idValue;

                Persons updatedPerson = await UpdatePersonAsync(person);
                SetValuesToTexBoxs(updatedPerson);

                List<Persons> persons = await GetAllPersonsAsync();
                AddValueToListBox(persons);
            }
        }

        private async void bDelet_Click(object sender, RoutedEventArgs e)
        {
            int idValue = GetValueIdfromListBox();
            if (idValue != 0)
            {
                var statusCode = await DeletePersonAsync(idValue.ToString());

                tMessage.Text = $"Deleted (HTTP Status = {(int)statusCode})";

                List<Persons> persons = await GetAllPersonsAsync();
                AddValueToListBox(persons);
            }
        }
    }
}
