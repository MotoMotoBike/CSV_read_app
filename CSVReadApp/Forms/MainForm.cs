using CSVReadApp.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CSVReadApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            openFileDialog.Filter = "CSV Files|*.CSV";
        }
        string fileName;

        List<string[]> csvFile;
        private void openFileButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;

                csvFile = CSVActions.ReadCSVToList(fileName);
                CSVActions.UpdateDepartmentsList(csvFile);
                CSVActions.UpdateCatalogAndRepoList(csvFile);

                MessageBox.Show("Данные успешно обновлены");
            }
            else
            {
                MessageBox.Show("Ошибка выбора файла");
            }
        }

        private void goToViewButton_Click(object sender, EventArgs e)
        {
            ViewForm viewForm = new ViewForm();
            viewForm.Show();
        }
    }
}
