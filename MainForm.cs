using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GradeManagementSystem;

public partial class MainForm : Form
{
    private void importButton_Click(object sender, EventArgs e)
    {
        importDialog.InitialDirectory = Directory.GetCurrentDirectory();
        if (importDialog.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        string folder = importDialog.SelectedPath;
        // TODO
        throw new NotImplementedException();
    }

    public MainForm()
    {
        InitializeComponent();

        CreateTextColumn("CRN");
        CreateTextColumn("Prefix");
        CreateTextColumn("Number");
        CreateTextColumn("Year");
        CreateTextColumn("Semester");
        CreateTextColumn("Grade");
        CreateButtonColumn("Edit");
        CreateButtonColumn("Delete");
    }

    private void CreateTextColumn(string headerText)
    {
        DataGridViewTextBoxColumn column = new();
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        column.ReadOnly = true;
        column.Name = headerText;
        column.HeaderText = headerText;
        dataGrid.Columns.Add(column);
    }

    private void CreateButtonColumn(string buttonText)
    {
        DataGridViewButtonColumn column = new();
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        column.ReadOnly = true;
        column.UseColumnTextForButtonValue = true;
        column.Name = buttonText;
        column.Text = buttonText;
        dataGrid.Columns.Add(column);
    }

    private bool ValidateStudentID(out int id) => int.TryParse(searchBox.Text, out id) && id > 0;

    private bool _searching;

    private void Search()
    {
        if (_searching || !ValidateStudentID(out int id))
        {
            return;
        }

        _searching = true;
        searchButton.Text = @"Refresh";
        searchButton.Enabled = false;
        resultsLabel.Text = $@"Grabbing information for student #{id} . . .";
        addButton.Enabled = false;
        dataGrid.Rows.Clear();

        Student student = new(id);
        dataGrid.Tag = student;
        if (student.Existing)
        {
            foreach (Grade grade in student.GetGrades())
            {
                DataGridViewRow row = new();
                row.Tag = grade;

                row.CreateCells(dataGrid);
                row.Cells[dataGrid.Columns["Grade"]!.Index].Value = grade.Letter;
                row.Cells[dataGrid.Columns["CRN"]!.Index].Value = grade.Course.CRN;
                row.Cells[dataGrid.Columns["Prefix"]!.Index].Value = grade.Course.Prefix;
                row.Cells[dataGrid.Columns["Number"]!.Index].Value = grade.Course.Number;
                row.Cells[dataGrid.Columns["Year"]!.Index].Value = grade.Course.Year;
                row.Cells[dataGrid.Columns["Semester"]!.Index].Value = grade.Course.Semester;

                dataGrid.Rows.Add(row);
            }
        }

        _searching = false;
        searchButton.Enabled = true;
        resultsLabel.Text = $@"Selected {(student.Existing ? "existing" : "new")} student #{student.ID}" +
                            $@"{(student.Name is not null ? $" ({student.Name})" : "")}";
        addButton.Enabled = true;
    }

    private void searchBox_TextChanged(object sender, EventArgs e)
    {
        if (!ValidateStudentID(out int id))
        {
            searchButton.Enabled = false;
            searchButton.Text = @"Search";
            return;
        }

        searchButton.Enabled = true;

        if (dataGrid.Tag is Student student && student.ID == id)
        {
            searchButton.Text = @"Refresh";
            return;
        }

        searchButton.Text = @"Search";
    }

    private void searchBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar != (char)Keys.Enter)
        {
            return;
        }

        e.Handled = true;
        Search();
    }

    private void searchButton_Click(object sender, EventArgs e) => Search();

    private void addButton_Click(object sender, EventArgs e)
    {
        if (dataGrid.Tag is not Student student)
        {
            return;
        }

        // TODO

        throw new NotImplementedException();
    }
}