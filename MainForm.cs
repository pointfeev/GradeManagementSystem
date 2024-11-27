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

    private const string ImportFolderNameExample =
        "\n\nCorrect format:\nGrades [Year] [Semester]\nex. Grades 2024 Fall";

    private const string ImportFileNameExample =
        "\n\nCorrect format example:\n[Prefix] [Number] [Year] [Semester] [CRN]\nex. CSC 440 2024 Fall 12345";

    private static void ImportGrades(string folder)
    {
        /*string folderName = Path.GetFileName(folder);
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

            if (fileParams.Length < 3 || !int.TryParse(fileParams[2], out _)) //, out int year))
            {
                DisplayError($"Invalid year in file name \"{fileName}\"{ImportFileNameExample}");
                return;
            }

            if (fileParams.Length < 4)
            {
                DisplayError($"Invalid semester in file name \"{fileName}\"{ImportFileNameExample}");
                return;
            }

            // string semester = fileParams[3];

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
        }*/
    }

    private void UpdateResultsLabel()
    {
        if (Student.ID is null)
        {
            resultsLabel.Text = @"No student selected.";
            return;
        }

        resultsLabel.Text = $@"Selected {(Student.Existing ? "existing" : "new")} student #{Student.ID}" +
                            (Student.GPA is not null
                                ? $@" ({(Student.Name is not null
                                    ? $"{Student.Name}, "
                                    : string.Empty)}GPA: {Student.GPA:0.00})"
                                : string.Empty);
    }

    private async void importButton_Click(object sender, EventArgs e)
    {
        throw new NotImplementedException();

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

        bool search = Student.ID is not null && ValidateStudentID(out int id) && id == Student.ID;

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

    private void UpdateRow(DataGridViewRow row,
        (int? id, char letter, (int crn, string prefix, int number, int year, string semester) course) grade)
    {
        row.Cells[dataGrid.Columns["CRN"]!.Index].Value = grade.course.crn;
        row.Cells[dataGrid.Columns["Prefix"]!.Index].Value = grade.course.prefix;
        row.Cells[dataGrid.Columns["Number"]!.Index].Value = grade.course.number;
        row.Cells[dataGrid.Columns["Year"]!.Index].Value = grade.course.year;
        row.Cells[dataGrid.Columns["Semester"]!.Index].Value = grade.course.semester;
        row.Cells[dataGrid.Columns["Grade"]!.Index].Value = grade.letter;
    }

    private void CreateRow(
        (int? id, char letter, (int crn, string prefix, int number, int year, string semester) course) grade)
    {
        DataGridViewRow row = new();
        row.CreateCells(dataGrid);
        UpdateRow(row, grade);
        dataGrid.Rows.Add(row);
    }

    private void UpdateRows()
    {
        dataGrid.Rows.Clear();
        foreach ((int? id, char letter, (int crn, string prefix, int number, int year, string semester) course)
                 grade in Student.Grades)
        {
            CreateRow(grade);
        }
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

        Student.Get(id);
        UpdateRows();

        searchButton.Enabled = true;
        UpdateResultsLabel();
        addButton.Enabled = true;
        printButton.Enabled = Student.Grades.Count > 0;
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

        if (Student.ID is not null && Student.ID == id)
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
        if (Student.ID is null)
        {
            return;
        }

        AddEditForm form = new();
        form.Text = @"Add";
        if (form.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        (int? id, char letter, (int crn, string prefix, int number, int year, string semester) course)
            grade = (null, form.Grade[0],
                (int.Parse(form.CRN),
                    form.Prefix,
                    int.Parse(form.Number),
                    int.Parse(form.Year),
                    form.Semester));
        Student.Grades.Add(grade);

        await Task.Run(Student.CalculateGPA);
        if (!await Task.Run(Student.Commit))
        {
            return;
        }

        UpdateRows();
        UpdateResultsLabel();
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
        if (Student.ID is null)
        {
            return;
        }

        MemoryStream stream = new();
        using StreamWriter writer = new(stream);

        writer.WriteLine($"Student #{Student.ID}");
        writer.WriteLine($"{Student.Name}");
        writer.WriteLine($"GPA: {Student.GPA:0.00}");

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
        rows.AddRange(from grade in Student.Grades
            let course = grade.course
            select (List<string>)
            [
                course.crn.ToString(),
                course.prefix,
                course.number.ToString(),
                course.year.ToString(),
                course.semester,
                grade.letter.ToString()
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
        if (Student.ID is null || e.RowIndex < 0)
        {
            return;
        }

        int gradeIndex = e.RowIndex;
        DataGridViewRow row = dataGrid.Rows[gradeIndex];
        (int? id, char letter, (int crn, string prefix, int number, int year, string semester) course)
            grade = Student.Grades[gradeIndex];

        if (e.ColumnIndex == dataGrid.Columns["Edit"]!.Index)
        {
            AddEditForm form = new();
            form.Text = @"Edit";

            form.CRN = grade.course.crn.ToString();
            form.Prefix = grade.course.prefix;
            form.Number = grade.course.number.ToString();
            form.Year = grade.course.year.ToString();
            form.Semester = grade.course.semester;
            form.Grade = grade.letter.ToString();
            if (form.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            (int? id, char letter, (int crn, string prefix, int number, int year, string semester) course)
                updatedGrade = (grade.id, form.Grade[0],
                    (int.Parse(form.CRN),
                        form.Prefix,
                        int.Parse(form.Number),
                        int.Parse(form.Year),
                        form.Semester));
            Student.Grades[gradeIndex] = updatedGrade;
            await Task.Run(Student.CalculateGPA);
            if (!await Task.Run(Student.Commit))
            {
                return;
            }

            UpdateResultsLabel();
            UpdateRow(row, updatedGrade);
        }
        else if (e.ColumnIndex == dataGrid.Columns["Delete"]!.Index)
        {
            if (grade.id is { } gradeId && !await Task.Run(() => Grade.Delete(gradeId, grade.course.crn)))
            {
                return;
            }

            if (await Task.Run(Student.Delete))
            {
                UpdateResultsLabel();
                dataGrid.Rows.Clear();
                printButton.Enabled = false;
                return;
            }

            await Task.Run(Student.GetGrades);
            await Task.Run(Student.CalculateGPA);
            if (!await Task.Run(Student.Commit))
            {
                return;
            }

            UpdateRows();
            printButton.Enabled = Student.Grades.Count > 0;
        }
    }
}