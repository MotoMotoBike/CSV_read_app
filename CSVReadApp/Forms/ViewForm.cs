using CSVReadApp.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CSVReadApp.Forms
{
    public partial class ViewForm : Form
    {
        public ViewForm()
        {
            InitializeComponent();
        }

        private void ViewForm_Load(object sender, EventArgs e)
        {
            textBox.Text = GetDataString();
        }

        string GetDataString()
        {
            DatabaseContext context = DatabaseContext.GetInstance();
            var catalog = context.Catalog.ToArray();
            var repo = context.Repository.ToArray();
            var department = context.Department.ToArray();
            var repositoryDepartment = context.RepositoryDepartment.ToArray();
            string res ="";

            foreach (var cat in catalog)
            {
                var indentCalculationInCat = cat;

                while (indentCalculationInCat.CatalogId != null)
                {
                    indentCalculationInCat = catalog.Where(en => en.IdKey == indentCalculationInCat.CatalogId).FirstOrDefault();
                    res += "   ";
                }

                res += cat.IdKey + " " + cat.Name + "\r\n";

                foreach (var r in repo.Where(en => en.CatalogId == cat.IdKey).ToArray())
                {
                    var indentCalculationInRepo = r.Catalog;
                    while (indentCalculationInRepo.CatalogId != null)
                    {
                        indentCalculationInRepo = catalog.Where(en => en.IdKey == indentCalculationInRepo.CatalogId).FirstOrDefault();
                        res += "   ";
                    }
                    res += r.IdKey + " " + r.Name;
                    foreach (var rd in repositoryDepartment.Where(en => en.RepositoryId == r.IdKey))
                    {
                        res += "|" + rd.Department.Name;
                    }
                    res += "\r\n";
                }
            }

            return res;
        }
    }
}
