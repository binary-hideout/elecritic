using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using CsvHelper;

using Elecritic.Database;
using Elecritic.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;

namespace Elecritic.Pages {
    // TODO: use roles

    [Authorize]
    public partial class UploadData {

        [Inject]
        private UploadDataContext UploadDataContext { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthStateTask { get; set; }

        private IBrowserFile FileEntry { get; set; }

        private Category NewCategory {get; set; }

        public UploadData() {
            NewCategory = new Category();
        }

        protected override async Task OnInitializedAsync() {
            var authState = await AuthStateTask;
            if (authState.User.Identity.IsAuthenticated) {
                var user = new User(authState.User);
                if (user.RoleId != 1) {
                    NavigationManager.NavigateTo("/");
                }
            }
            else {
                NavigationManager.NavigateTo("/login");
            }
        }

        private void OnFileUploaded(InputFileChangeEventArgs eventArgs) {
            FileEntry = eventArgs.GetMultipleFiles(1)[0];
        }

        /// <summary>
        /// Reads a CSV file which contains information of electronic devices,
        /// parses the data and uploads the products as records to the database.
        /// </summary>
        private async Task UploadFileContentsAsync() {
            var categories = await UploadDataContext.GetCategoriesAsync();
            var companies = await UploadDataContext.GetCompaniesAsync();

            using var reader = new StreamReader(FileEntry.OpenReadStream());
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

            await csvReader.ReadAsync();
            csvReader.ReadHeader();
            while (await csvReader.ReadAsync()) {
                var companyName = csvReader.GetField("Product Brand").ToLower();
                // make upper case the first letter of the string
                companyName = char.ToUpper(companyName[0]) + companyName.Substring(1);
                var company = companies.FirstOrDefault(c => c.Name == companyName);
                // if the company is not in the database
                if (company is null) {
                    company = new Company {
                        Name = companyName
                    };
                    // add it
                    await UploadDataContext.InsertCompanyAsync(company);
                    companies.Add(company);
                }

                // get category tag (id)
                var tag = csvReader.GetField<int>("Tag");
                // use the tag to get the category object
                var category = categories[tag - 1];

                var model = csvReader.GetField("Product Model").Split(' ');
                // first 3 words will be the name
                var name = string.Join(" ", model.Take(3));
                // the rest of the words will be the description
                var description = string.Join(" ", model.Skip(3));

                var imagePath = csvReader.GetField("ProductImg");

                var product = new Product {
                    Name = name,
                    Description = description,
                    ImagePath = imagePath,
                    Category = category,
                    Company = company
                };
                await UploadDataContext.InsertProductAsync(product);
            }
        }

        private async Task UploadNewCategoryAsync() {
            var category = new Category {
                Name = NewCategory.Name
            };
            await UploadDataContext.InsertCategoryAsync(category);
            NewCategory.Name = "";
        }
    }
}
