﻿using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Elecritic.Models;
using Elecritic.Services;

using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Radzen;
using OfficeOpenXml;
using System.IO;

namespace Elecritic.Pages {

    public partial class UploadFiles {


        public List<Product> ReadFile(string productTable) {

            List<Product> productsInFile = new List<Product>();

            string FilePath = productTable;
            System.IO.FileInfo existingFile = new System.IO.FileInfo(FilePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                int colCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;

                for (int row = 0; row < rowCount; row++) {

                    Product product = new Product();

                    for (int col = 0; col < colCount; col++) {
                        if (col == 1) product.ImagePath = worksheet.Cells[row, col].Value.ToString();
                        else if (col == 2) {
                            product.Name = worksheet.Cells[row, col].Value.ToString();
                        }else if(col == 3) {
                            product.Description = worksheet.Cells[row, col].Value.ToString();
                        }
                            
                    }
                    productsInFile.Add(product);
                }
            }


            return productsInFile;

        }
    }

}
