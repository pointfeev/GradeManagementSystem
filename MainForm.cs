﻿using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace GradeManagementSystem;

public partial class MainForm : Form
{
    public static void DisplayError(string text)
    {
        MessageBox.Show(text, @"ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private const string ImportFolderNameExample = "\n\nCorrect format example:\nGrades 2024 Fall";
    private const string ImportFileNameExample = "\n\nCorrect format example:\nCSC 440 2024 Fall 12345";

    private static void ImportGrades(string folder)
    {
        string folderName = Path.GetFileName(folder);
        string[] folderParams = folderName.Split(' ');
        if (folderParams.Length < 1 || folderParams[0] != "Grades")
        {
            DisplayError($"Invalid folder name \"{folderName}\"{ImportFolderNameExample}");
            return;
        }

        if (folderParams.Length < 2 || !int.TryParse(folderParams[1], out int year))
        {
            DisplayError($"Invalid year in folder name \"{folderName}\"{ImportFolderNameExample}");
            return;
        }

        if (folderParams.Length < 3)
        {
            DisplayError($"Invalid semester in folder name \"{folderName}\"{ImportFolderNameExample}");
            return;
        }

        string semester = folderParams[2];

        foreach (string file in Directory.EnumerateFiles(folder, "*.xlsx"))
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string[] fileParams = fileName.Split(' ');
            if (fileParams.Length < 1)
            {
                DisplayError($"Invalid file name \"{fileName}\"{ImportFileNameExample}");
                return;
            }

            string prefix = fileParams[0];

            if (fileParams.Length < 2 || !int.TryParse(fileParams[1], out int number))
            {
                DisplayError($"Invalid prefix in file name \"{fileName}\"{ImportFileNameExample}");
                return;
            }

            /*if (fileParams.Length < 3 || !int.TryParse(fileParams[2], out int year))
            {
                DisplayError($"Invalid year in file name \"{fileName}\"{ImportFileNameExample}");
                return;
            }

            if (fileParams.Length < 4)
            {
                DisplayError($"Invalid semester in file name \"{fileName}\"{ImportFileNameExample}");
                return;
            }

            string semester = fileParams[3];*/

            if (fileParams.Length < 5 || !int.TryParse(fileParams[4], out int crn))
            {
                DisplayError($"Invalid CRN in file name \"{fileName}\"{ImportFileNameExample}");
                return;
            }

            using FileStream stream = new(file, FileMode.Open, FileAccess.Read);
            using SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false);
            WorkbookPart workbookPart = document.WorkbookPart!;

            List<OpenXmlElement> sharedStrings = [];
            foreach (SharedStringTablePart sharedStringTablePart in
                     workbookPart.GetPartsOfType<SharedStringTablePart>())
            {
                sharedStrings.AddRange(sharedStringTablePart.SharedStringTable.ChildElements);
            }

            foreach (WorksheetPart worksheetPart in workbookPart.WorksheetParts)
            {
                Dictionary<int, string> columns = new();

                Worksheet worksheet = worksheetPart.Worksheet;
                Row[] rows = worksheet.Descendants<Row>().ToArray();
                for (int rowIndex = 0; rowIndex < rows.Length; rowIndex++)
                {
                    string? name = null;
                    int? id = null;
                    char? letterGrade = null;

                    Row row = rows[rowIndex];
                    Cell[] cells = row.Elements<Cell>().ToArray();
                    for (int cellIndex = 0; cellIndex < cells.Length; cellIndex++)
                    {
                        Cell cell = cells[cellIndex];
                        string cellText;
                        if (cell.DataType is not null && cell.DataType.Value == CellValues.SharedString)
                        {
                            if (!int.TryParse(cell.InnerText, out int sharedStringId))
                            {
                                continue;
                            }

                            cellText = sharedStrings[sharedStringId].InnerText;
                        }
                        else
                        {
                            cellText = cell.InnerText;
                        }

                        cellText = cellText.Trim();
                        if (cellText.Length == 0)
                        {
                            continue;
                        }

                        if (rowIndex == 0)
                        {
                            columns[cellIndex] = cellText.ToLower();
                            continue;
                        }

                        string column = columns[cellIndex];
                        switch (column)
                        {
                            case "name":
                            {
                                if (cellText.Length == 0)
                                {
                                    DisplayError(
                                        $"Invalid value for column \"name\" in file {fileName} on row {rowIndex + 1}");
                                    return;
                                }

                                name = cellText;
                                break;
                            }
                            case "id":
                            {
                                if (!int.TryParse(cellText, out int idValue))
                                {
                                    DisplayError(
                                        $"Invalid value for column \"id\" in file {fileName} on row {rowIndex + 1}");
                                    return;
                                }

                                id = idValue;
                                break;
                            }
                            case "grade":
                            {
                                string gradeString = cellText.ToUpper();
                                if (gradeString.Length != 1 || !Grade.ValidLetters.Contains(gradeString[0]))
                                {
                                    DisplayError(
                                        $"Invalid value for column \"grade\" in file {fileName} on row {rowIndex + 1}");
                                    return;
                                }

                                letterGrade = gradeString[0];
                                break;
                            }
                            default:
                                DisplayError(
                                    $"Invalid column \"{column}\" in file {fileName}");
                                return;
                        }
                    }

                    if (rowIndex == 0)
                    {
                        continue;
                    }

                    if (name is null)
                    {
                        DisplayError($"Missing column \"name\" in file {fileName}");
                        return;
                    }

                    if (id is null)
                    {
                        DisplayError($"Missing column \"id\" in file {fileName}");
                        return;
                    }

                    if (letterGrade is null)
                    {
                        DisplayError($"Missing column \"grade\" in file {fileName}");
                        return;
                    }

                    Student student = new(id.Value)
                    {
                        Name = name
                    };

                    Grade? grade = student.Grades.Find(grade => grade.Course.CRN == crn);
                    if (grade is not null)
                    {
                        grade.Letter = letterGrade.Value;
                    }
                    else
                    {
                        _ = new Grade
                        {
                            Student = student,
                            Letter = letterGrade.Value,
                            Course = new()
                            {
                                CRN = crn,
                                Prefix = prefix,
                                Number = number,
                                Year = year,
                                Semester = semester
                            }
                        };
                    }

                    student.CalculateGPA();
                    if (!student.Commit())
                    {
                        return;
                    }
                }
            }
        }
    }

    private async void importButton_Click(object sender, EventArgs e)
    {
        importDialog.InitialDirectory = Directory.GetCurrentDirectory();
        if (importDialog.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        string folder = importDialog.SelectedPath;
        string folderName = Path.GetFileName(folder);

        searchBox.Enabled = false;
        searchButton.Text = @"Search";
        searchButton.Enabled = false;
        resultsLabel.Text = $"""Importing grades in folder "{folderName}" . . .""";
        addButton.Enabled = false;
        dataGrid.Enabled = false;
        dataGrid.Rows.Clear();

        bool search = dataGrid.Tag is Student student && ValidateStudentID(out int id) && id == student.ID;
        dataGrid.Tag = null;

        await Task.Run(() => ImportGrades(importDialog.SelectedPath));

        searchBox.Enabled = true;
        searchButton.Enabled = true;
        resultsLabel.Text = @"No student selected.";
        dataGrid.Enabled = true;

        if (search)
        {
            await Task.Run(Search);
        }
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
        dataGrid.CellClick += dataGrid_CellClick;
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

    private bool ValidateStudentID(out int id) => int.TryParse(searchBox.Text.Trim(), out id) && id > 0;

    private void Search()
    {
        if (!searchButton.Enabled || !ValidateStudentID(out int id))
        {
            return;
        }

        searchButton.Text = @"Refresh";
        searchButton.Enabled = false;
        resultsLabel.Text = $@"Grabbing information for student #{id} . . .";
        addButton.Enabled = false;
        dataGrid.Enabled = false;
        dataGrid.Rows.Clear();

        Student student = new(id);
        dataGrid.Tag = student;
        foreach (Grade grade in student.Grades)
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

        searchButton.Enabled = true;
        resultsLabel.Text = $@"Selected {(student.Existing ? "existing" : "new")} student #{student.ID}" +
                            $@"{(student.Name is not null ? $" ({student.Name})" : "")}";
        addButton.Enabled = true;
        dataGrid.Enabled = true;
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

    private async void searchBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar != (char)Keys.Enter)
        {
            return;
        }

        e.Handled = true;
        await Task.Run(Search);
    }

    private async void searchButton_Click(object sender, EventArgs e) => await Task.Run(Search);

    private void addButton_Click(object sender, EventArgs e)
    {
        if (dataGrid.Tag is not Student student)
        {
            return;
        }

        // TODO: create a form that can be used for add and edit functionality
        throw new NotImplementedException();
    }

    private async void dataGrid_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
        {
            return;
        }

        if (e.ColumnIndex == dataGrid.Columns["Edit"]!.Index)
        {
            // TODO: create a form that can be used for add and edit functionality
            throw new NotImplementedException();
        }
        else if (e.ColumnIndex == dataGrid.Columns["Delete"]!.Index)
        {
            DataGridViewRow row = dataGrid.Rows[e.RowIndex];
            if (row.Tag is not Grade grade || await Task.Run(grade.Delete))
            {
                dataGrid.Rows.RemoveAt(e.RowIndex);
            }
        }
    }
}