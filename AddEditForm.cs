namespace GradeManagementSystem;

public partial class AddEditForm : Form
{
    public string CRN
    {
        get => crnBox.Text;
        set => crnBox.Text = value;
    }

    public string Prefix
    {
        get => prefixBox.Text;
        set => prefixBox.Text = value;
    }

    public string Number
    {
        get => numberBox.Text;
        set => numberBox.Text = value;
    }

    public string Year
    {
        get => yearBox.Text;
        set => yearBox.Text = value;
    }

    public string Semester
    {
        get => semesterBox.Text;
        set => semesterBox.Text = value;
    }

    public string Grade
    {
        get => gradeBox.Text;
        set => gradeBox.Text = value;
    }

    public AddEditForm()
    {
        InitializeComponent();
    }

    private void saveButton_Click(object sender, EventArgs e)
    {
        bool invalid = false;
        if (string.IsNullOrWhiteSpace(CRN) || !int.TryParse(CRN, out _))
        {
            crnBox.Focus();
            invalid = true;
        }
        else if (string.IsNullOrWhiteSpace(Prefix))
        {
            crnBox.Focus();
            invalid = true;
        }
        else if (string.IsNullOrWhiteSpace(Number) || !int.TryParse(Number, out _))
        {
            numberBox.Focus();
            invalid = true;
        }
        else if (string.IsNullOrWhiteSpace(Year) || !int.TryParse(Year, out _))
        {
            yearBox.Focus();
            invalid = true;
        }
        else if (string.IsNullOrWhiteSpace(Semester))
        {
            semesterBox.Focus();
            invalid = true;
        }
        else if (Grade.Length != 1 || !GradeManagementSystem.Grade.ValidLetters.Contains(Grade[0]))
        {
            gradeBox.Focus();
            invalid = true;
        }

        if (invalid)
        {
            DialogResult = DialogResult.None;
        }
    }
}