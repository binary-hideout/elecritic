using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using BlazorInputFile;

using CsvHelper;

using Elecritic.Database;
using Elecritic.Models;
using Elecritic.Services;

using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {

    public partial class UploadData {

        [Inject]
        private UploadDataContext UploadDataContext { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private UserService UserService { get; set; }

        private bool IsUserAllowed { get; set; } = false;

        private IFileListEntry FileEntry { get; set; }

        private Category NewCategory {get; set; } = new Category();

        protected override async Task OnInitializedAsync() {
            int[] allowedUsersIds = { 1, 2, 3 };
            int loggedUserId = UserService.LoggedUser.Id;
            IsUserAllowed = allowedUsersIds.Contains(loggedUserId);
            if (!IsUserAllowed) {
                NavigationManager.NavigateTo("/");
            }

            await base.OnInitializedAsync();
        }

        private void OnFileUploaded(IFileListEntry[] files) {
            FileEntry = files.FirstOrDefault();
        }

        /// <summary>
        /// Reads a CSV file which contains information of electronic devices,
        /// parses the data and uploads the products as records to the database.
        /// </summary>
        private async Task UploadFileContentsAsync() {
            var categories = await UploadDataContext.GetCategoriesAsync();
            var companies = await UploadDataContext.GetCompaniesAsync();

            using var reader = new StreamReader(FileEntry.Data);
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
