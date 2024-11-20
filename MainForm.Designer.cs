namespace GradeManagementSystem;

partial class MainForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
        searchBox = new System.Windows.Forms.TextBox();
        importButton = new System.Windows.Forms.Button();
        searchButton = new System.Windows.Forms.Button();
        dataGrid = new System.Windows.Forms.DataGridView();
        resultsLabel = new System.Windows.Forms.Label();
        addButton = new System.Windows.Forms.Button();
        importDialog = new System.Windows.Forms.FolderBrowserDialog();
        ((System.ComponentModel.ISupportInitialize)dataGrid).BeginInit();
        SuspendLayout();
        // 
        // searchBox
        // 
        searchBox.Font = new System.Drawing.Font("Segoe UI", 10F);
        searchBox.Location = new System.Drawing.Point(12, 13);
        searchBox.Name = "searchBox";
        searchBox.PlaceholderText = "Student ID";
        searchBox.Size = new System.Drawing.Size(100, 25);
        searchBox.TabIndex = 0;
        searchBox.TextChanged += searchBox_TextChanged;
        searchBox.KeyPress += searchBox_KeyPress;
        // 
        // importButton
        // 
        importButton.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
        importButton.AutoSize = true;
        importButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        importButton.Location = new System.Drawing.Point(474, 12);
        importButton.Name = "importButton";
        importButton.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
        importButton.Size = new System.Drawing.Size(98, 27);
        importButton.TabIndex = 2;
        importButton.Text = "Import Grades";
        importButton.UseVisualStyleBackColor = true;
        importButton.Click += importButton_Click;
        // 
        // searchButton
        // 
        searchButton.AutoSize = true;
        searchButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        searchButton.Enabled = false;
        searchButton.Location = new System.Drawing.Point(118, 12);
        searchButton.Name = "searchButton";
        searchButton.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
        searchButton.Size = new System.Drawing.Size(58, 27);
        searchButton.TabIndex = 1;
        searchButton.Text = "Search";
        searchButton.UseVisualStyleBackColor = true;
        searchButton.Click += searchButton_Click;
        // 
        // dataGrid
        // 
        dataGrid.AllowUserToAddRows = false;
        dataGrid.AllowUserToDeleteRows = false;
        dataGrid.AllowUserToResizeColumns = false;
        dataGrid.AllowUserToResizeRows = false;
        dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
        dataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
        dataGrid.BackgroundColor = System.Drawing.SystemColors.Control;
        dataGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
        dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
        dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
        dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        dataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
        dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
        dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
        dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
        dataGrid.DefaultCellStyle = dataGridViewCellStyle2;
        dataGrid.Location = new System.Drawing.Point(12, 78);
        dataGrid.MultiSelect = false;
        dataGrid.Name = "dataGrid";
        dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
        dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
        dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
        dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        dataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
        dataGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        dataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
        dataGrid.Size = new System.Drawing.Size(560, 371);
        dataGrid.TabIndex = 5;
        dataGrid.CellClick += dataGrid_CellClick;
        // 
        // resultsLabel
        // 
        resultsLabel.AutoSize = true;
        resultsLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
        resultsLabel.Location = new System.Drawing.Point(12, 51);
        resultsLabel.Margin = new System.Windows.Forms.Padding(3);
        resultsLabel.Name = "resultsLabel";
        resultsLabel.Size = new System.Drawing.Size(115, 15);
        resultsLabel.TabIndex = 3;
        resultsLabel.Text = "No student selected.";
        // 
        // addButton
        // 
        addButton.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
        addButton.AutoSize = true;
        addButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        addButton.Enabled = false;
        addButton.Location = new System.Drawing.Point(493, 45);
        addButton.Name = "addButton";
        addButton.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
        addButton.Size = new System.Drawing.Size(79, 27);
        addButton.TabIndex = 4;
        addButton.Text = "Add Grade";
        addButton.UseVisualStyleBackColor = true;
        addButton.Click += addButton_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.SystemColors.Control;
        ClientSize = new System.Drawing.Size(584, 461);
        Controls.Add(addButton);
        Controls.Add(resultsLabel);
        Controls.Add(dataGrid);
        Controls.Add(searchButton);
        Controls.Add(importButton);
        Controls.Add(searchBox);
        DoubleBuffered = true;
        MinimumSize = new System.Drawing.Size(320, 180);
        Text = "Grade Management System";
        ((System.ComponentModel.ISupportInitialize)dataGrid).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.FolderBrowserDialog importDialog;

    private System.Windows.Forms.Button addButton;

    private System.Windows.Forms.Label resultsLabel;

    private System.Windows.Forms.DataGridView dataGrid;

    private System.Windows.Forms.Button searchButton;

    private System.Windows.Forms.Button importButton;

    private System.Windows.Forms.TextBox searchBox;

    #endregion
}