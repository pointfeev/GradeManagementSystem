using System.Drawing.Printing;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Font = System.Drawing.Font;
using FontFamily = System.Drawing.FontFamily;

namespace GradeManagementSystem;

public partial class MainForm : Form
{
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

        // replace the print preview's print button with one that opens a printer selection dialog
        ToolStrip printPreviewDialogToolStrip = (ToolStrip)printPreviewDialog.Controls[1];
        ToolStripButton printDialogButton = new();
        printDialogButton.Image = printPreviewDialogToolStrip.ImageList?.Images[0];
        printDialogButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
        printDialogButton.Click += printPreviewDialog_PrintClick;
        printPreviewDialogToolStrip.Items.RemoveAt(0);
        printPreviewDialogToolStrip.Items.Insert(0, printDialogButton);
    }

    public static void DisplayError(string text)
    {
        MessageBox.Show(ActiveForm, text, @"ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        Dictionary<int, Student> importedStudents = [];

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

                    if (!importedStudents.TryGetValue(id.Value, out Student? student))
                    {
                        student = new(id.Value)
                        {
                            Name = name
                        };
                        importedStudents.Add(id.Value, student);
                    }

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
                }
            }
        }

        foreach ((_, Student student) in importedStudents)
        {
            student.CalculateGPA();
            if (!student.Commit())
            {
                return;
            }
        }
    }

    private void UpdateResultsLabel(Student? student = null)
    {
        if (student is null)
        {
            resultsLabel.Text = @"No student selected.";
            return;
        }

        resultsLabel.Text = $@"Selected {(student.Existing ? "existing" : "new")} student #{student.ID}" +
                            $@"{(student.Name is not null ? $" ({student.Name})" : "")}";
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
        printButton.Enabled = false;
        dataGrid.Enabled = false;
        dataGrid.Rows.Clear();

        bool search = dataGrid.Tag is Student student && ValidateStudentID(out int id) && id == student.ID;
        dataGrid.Tag = null;

        await Task.Run(() => ImportGrades(importDialog.SelectedPath));

        searchBox.Enabled = true;
        searchButton.Enabled = true;
        UpdateResultsLabel();
        dataGrid.Enabled = true;

        if (search)
        {
            await Task.Run(Search);
        }
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

    private void UpdateRow(DataGridViewRow row, Grade grade)
    {
        row.Cells[dataGrid.Columns["CRN"]!.Index].Value = grade.Course.CRN;
        row.Cells[dataGrid.Columns["Prefix"]!.Index].Value = grade.Course.Prefix;
        row.Cells[dataGrid.Columns["Number"]!.Index].Value = grade.Course.Number;
        row.Cells[dataGrid.Columns["Year"]!.Index].Value = grade.Course.Year;
        row.Cells[dataGrid.Columns["Semester"]!.Index].Value = grade.Course.Semester;
        row.Cells[dataGrid.Columns["Grade"]!.Index].Value = grade.Letter;
    }

    private void CreateRow(Grade grade)
    {
        DataGridViewRow row = new();
        row.Tag = grade;
        row.CreateCells(dataGrid);
        UpdateRow(row, grade);
        dataGrid.Rows.Add(row);
    }

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
        printButton.Enabled = false;
        dataGrid.Enabled = false;
        dataGrid.Rows.Clear();

        Student student = new(id);
        dataGrid.Tag = student;
        foreach (Grade grade in student.Grades)
        {
            CreateRow(grade);
        }

        searchButton.Enabled = true;
        UpdateResultsLabel(student);
        addButton.Enabled = true;
        printButton.Enabled = student.Grades.Count > 0;
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

    private async void addButton_Click(object sender, EventArgs e)
    {
        if (dataGrid.Tag is not Student student)
        {
            return;
        }

        AddEditForm form = new();
        form.Text = @"Add";
        if (form.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        Grade grade = new()
        {
            Student = student,
            Letter = form.Grade[0],
            Course = new()
            {
                CRN = int.Parse(form.CRN),
                Prefix = form.Prefix,
                Number = int.Parse(form.Number),
                Year = int.Parse(form.Year),
                Semester = form.Semester
            }
        };

        await Task.Run(student.CalculateGPA);
        if (!await Task.Run(student.Commit))
        {
            return;
        }

        UpdateResultsLabel(student);
        CreateRow(grade);
        printButton.Enabled = true;
    }

    private StreamReader? _printStream;

    private readonly Font[] _printFonts =
    [
        new(FontFamily.GenericMonospace, 20, FontStyle.Bold),
        new(FontFamily.GenericMonospace, 12, FontStyle.Regular)
    ];

    private void printButton_Click(object sender, EventArgs e)
    {
        if (dataGrid.Tag is not Student student)
        {
            return;
        }

        MemoryStream stream = new();
        using StreamWriter writer = new(stream);

        writer.WriteLine($"Student #{student.ID}");
        writer.WriteLine($"{student.Name}");
        writer.WriteLine($"GPA: {student.GPA:0.00}");

        List<List<string>> rows =
        [
            [
                "Course CRN",
                "Prefix",
                "Number",
                "Year",
                "Semester",
                "Grade"
            ]
        ];
        rows.AddRange(from grade in student.Grades
            let course = grade.Course
            select (List<string>)
            [
                course.CRN.ToString(),
                course.Prefix,
                course.Number.ToString(),
                course.Year.ToString(),
                course.Semester,
                grade.Letter.ToString()
            ]);

        Dictionary<int, int> columnLength = new();
        foreach (List<string> row in rows)
        {
            for (int i = 0; i < row.Count; i++)
            {
                if (!columnLength.TryGetValue(i, out int length))
                {
                    columnLength[i] = row[i].Length;
                }

                columnLength[i] = Math.Max(length, row[i].Length);
            }
        }

        writer.WriteLine();
        for (int row = 0; row < rows.Count; row++)
        {
            List<string> currentRow = rows[row];

            // write the lines at the top of the table and in-between rows
            writer.Write("{1}" + // use the font at _printFonts index 1
                         (row == 0
                             ? '\u250c' // ┌
                             : '\u251c')); // ├
            for (int column = 0; column < currentRow.Count; column++)
            {
                writer.Write(new string('\u2500', columnLength[column] + 2) +
                             (column == currentRow.Count - 1
                                 ? row == 0
                                     ? '\u2510' // ┐
                                     : '\u2524' // ┤
                                 : row == 0
                                     ? '\u252c' // ┬
                                     : '\u253c')); // ┼
            }

            writer.WriteLine();

            // write the row data
            writer.Write("{1}"); // use the font at _printFonts index 1
            for (int column = 0; column < currentRow.Count; column++)
            {
                writer.Write((column == 0
                                 ? '\u2502' // │
                                 : string.Empty) +
                             " {0,-" + columnLength[column] + "} " +
                             '\u2502', // │
                    currentRow[column]);
            }

            writer.WriteLine();
        }

        // write the lines at the bottom of the table
        writer.Write("{1}" + // use the font at _printFonts index 1
                     '\u2514'); // └
        const int useRow = 0;
        List<string> useCurrentRow = rows[useRow];
        for (int column = 0; column < useCurrentRow.Count; column++)
        {
            writer.Write(new string('\u2500', // ─
                             columnLength[column] + 2) +
                         (column == useCurrentRow.Count - 1
                             ? '\u2518' // ┘
                             : '\u2534')); // ┴
        }

        writer.Flush();
        stream.Position = 0;
        using (_printStream = new(stream))
        {
            printPreviewDialog.ShowDialog();
        }
    }

    private void printPreviewDialog_PrintClick(object? sender, EventArgs e)
    {
        // reset _printStream to the beginning, because printPreviewDialog will read it to the end
        _printStream!.BaseStream.Position = 0;
        _printStream.DiscardBufferedData();

        if (printDialog.ShowDialog() == DialogResult.OK)
        {
            printDocument.Print();
        }
    }

    private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
        Rectangle pageRectangle = e.MarginBounds;
        float topMargin = pageRectangle.Top;
        float leftMargin = pageRectangle.Left;
        float pageHeight = pageRectangle.Height;
        Graphics graphics = e.Graphics!;

        string? line;
        float currentHeight = 0;
        while ((line = _printStream!.ReadLine()) is not null)
        {
            int fontIndex = 0;
            int parseFontStart = line.IndexOf('{');
            if (parseFontStart != -1)
            {
                int parseFontEnd = line.IndexOf('}', parseFontStart);
                if (parseFontEnd != -1)
                {
                    fontIndex = int.Parse(line.Substring(parseFontStart + 1, parseFontEnd - parseFontStart - 1));
                    line = line.Remove(parseFontStart, parseFontEnd + 1 - parseFontStart);
                }
            }

            Font font = _printFonts[fontIndex];
            graphics.DrawString(line, font, Brushes.Black,
                leftMargin, topMargin + currentHeight, new());
            currentHeight += font.GetHeight(graphics);
            if (currentHeight >= pageHeight)
            {
                break;
            }
        }

        e.HasMorePages = line is not null;
    }

    private async void dataGrid_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (dataGrid.Tag is not Student student || e.RowIndex < 0)
        {
            return;
        }

        DataGridViewRow row = dataGrid.Rows[e.RowIndex];
        if (row.Tag is not Grade grade)
        {
            dataGrid.Rows.RemoveAt(e.RowIndex);
            return;
        }

        if (e.ColumnIndex == dataGrid.Columns["Edit"]!.Index)
        {
            AddEditForm form = new();
            form.Text = @"Edit";

            Course course = grade.Course;
            form.CRN = course.CRN.ToString();
            form.Prefix = course.Prefix;
            form.Number = course.Number.ToString();
            form.Year = course.Year.ToString();
            form.Semester = course.Semester;
            form.Grade = grade.Letter.ToString();
            if (form.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            course.CRN = int.Parse(form.CRN);
            course.Prefix = form.Prefix;
            course.Number = int.Parse(form.Number);
            course.Year = int.Parse(form.Year);
            course.Semester = form.Semester;
            grade.Letter = form.Grade[0];
            await Task.Run(student.CalculateGPA);
            if (!await Task.Run(student.Commit))
            {
                return;
            }

            UpdateResultsLabel(student);
            UpdateRow(row, grade);
        }
        else if (e.ColumnIndex == dataGrid.Columns["Delete"]!.Index)
        {
            if (!await Task.Run(grade.Delete))
            {
                return;
            }

            if (await Task.Run(student.Delete))
            {
                student.Name = null;
                student.GPA = null;
                student.NeedsCommit = false;
                student.Existing = false;
                student.Grades.Clear();

                UpdateResultsLabel(student);
                dataGrid.Rows.Clear();
                printButton.Enabled = false;
                return;
            }

            await Task.Run(student.CalculateGPA);
            if (!await Task.Run(student.Commit))
            {
                return;
            }

            dataGrid.Rows.RemoveAt(e.RowIndex);
            printButton.Enabled = student.Grades.Count > 0;
        }
    }
}