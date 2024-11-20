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
    private bool _searching;

    private static readonly Dictionary<string, int> Columns = new()
    {
        ["CRN"] = 0,
        ["Prefix"] = 1,
        ["Number"] = 2,
        ["Year"] = 3,
        ["Semester"] = 4,
        ["Grade"] = 5
    };

    public MainForm()
    {
        InitializeComponent();

        foreach ((string text, int _) in Columns)
        {
            CreateTextColumn(text);
        }

        CreateButtonColumn("Edit");
        CreateButtonColumn("Delete");
    }

    private void CreateTextColumn(string headerText)
    {
        DataGridViewTextBoxColumn column = new();
        column.HeaderText = headerText;
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        column.ReadOnly = true;
        dataGrid.Columns.Add(column);
    }

    private void CreateButtonColumn(string buttonText)
    {
        DataGridViewButtonColumn column = new();
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        column.ReadOnly = true;
        column.Text = buttonText;
        column.UseColumnTextForButtonValue = true;
        dataGrid.Columns.Add(column);
    }

    private void Search()
    {
        if (_searching)
        {
            return;
        }

        if (!int.TryParse(searchBox.Text, out int id) || id < 1)
        {
            MessageBox.Show(@"Invalid student ID!", @"ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        _searching = true;
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
                row.Cells[Columns["Grade"]].Value = grade.Letter;
                row.Cells[Columns["CRN"]].Value = grade.Course.CRN;
                row.Cells[Columns["Prefix"]].Value = grade.Course.Prefix;
                row.Cells[Columns["Number"]].Value = grade.Course.Number;
                row.Cells[Columns["Year"]].Value = grade.Course.Year;
                row.Cells[Columns["Semester"]].Value = grade.Course.Semester;

                dataGrid.Rows.Add(row);
            }
        }

        _searching = false;
        resultsLabel.Text = $@"Selected {(student.Existing ? "existing" : "new")} student #{student.ID}" +
                            $@"{(student.Name is not null ? $" ({student.Name})" : "")}";
        addButton.Enabled = true;
    }

    private void searchButton_Click(object sender, EventArgs e) => Search();

    private void searchBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar != (char)Keys.Enter)
        {
            return;
        }

        e.Handled = true;
        Search();
    }

    private void importButton_Click(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void addButton_Click(object sender, EventArgs e)
    {
        if (dataGrid.Tag is not Student student)
        {
            addButton.Enabled = false;
            return;
        }

        // TODO

        throw new NotImplementedException();
    }
}