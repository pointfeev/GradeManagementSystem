using System.ComponentModel;

namespace GradeManagementSystem;

partial class AddEditForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
        fieldsPanel = new System.Windows.Forms.FlowLayoutPanel();
        crnPanel = new System.Windows.Forms.FlowLayoutPanel();
        crnLabel = new System.Windows.Forms.Label();
        crnBox = new System.Windows.Forms.TextBox();
        prefixPanel = new System.Windows.Forms.FlowLayoutPanel();
        prefixLabel = new System.Windows.Forms.Label();
        prefixBox = new System.Windows.Forms.TextBox();
        numberPanel = new System.Windows.Forms.FlowLayoutPanel();
        numberLabel = new System.Windows.Forms.Label();
        numberBox = new System.Windows.Forms.TextBox();
        yearPanel = new System.Windows.Forms.FlowLayoutPanel();
        yearLabel = new System.Windows.Forms.Label();
        yearBox = new System.Windows.Forms.TextBox();
        semesterPanel = new System.Windows.Forms.FlowLayoutPanel();
        semesterLabel = new System.Windows.Forms.Label();
        semesterBox = new System.Windows.Forms.TextBox();
        gradePanel = new System.Windows.Forms.FlowLayoutPanel();
        gradeLabel = new System.Windows.Forms.Label();
        gradeBox = new System.Windows.Forms.TextBox();
        saveButton = new System.Windows.Forms.Button();
        cancelButton = new System.Windows.Forms.Button();
        fieldsPanel.SuspendLayout();
        crnPanel.SuspendLayout();
        prefixPanel.SuspendLayout();
        numberPanel.SuspendLayout();
        yearPanel.SuspendLayout();
        semesterPanel.SuspendLayout();
        gradePanel.SuspendLayout();
        SuspendLayout();
        // 
        // fieldsPanel
        // 
        fieldsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
        fieldsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        fieldsPanel.Controls.Add(crnPanel);
        fieldsPanel.Controls.Add(prefixPanel);
        fieldsPanel.Controls.Add(numberPanel);
        fieldsPanel.Controls.Add(yearPanel);
        fieldsPanel.Controls.Add(semesterPanel);
        fieldsPanel.Controls.Add(gradePanel);
        fieldsPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
        fieldsPanel.Location = new System.Drawing.Point(12, 12);
        fieldsPanel.Name = "fieldsPanel";
        fieldsPanel.Size = new System.Drawing.Size(173, 210);
        fieldsPanel.TabIndex = 0;
        fieldsPanel.WrapContents = false;
        // 
        // crnPanel
        // 
        crnPanel.AutoSize = true;
        crnPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        crnPanel.Controls.Add(crnLabel);
        crnPanel.Controls.Add(crnBox);
        crnPanel.Location = new System.Drawing.Point(3, 3);
        crnPanel.Name = "crnPanel";
        crnPanel.Size = new System.Drawing.Size(167, 29);
        crnPanel.TabIndex = 1;
        crnPanel.WrapContents = false;
        // 
        // crnLabel
        // 
        crnLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom));
        crnLabel.AutoSize = true;
        crnLabel.Location = new System.Drawing.Point(3, 0);
        crnLabel.Name = "crnLabel";
        crnLabel.Size = new System.Drawing.Size(31, 29);
        crnLabel.TabIndex = 0;
        crnLabel.Text = "CRN";
        crnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // crnBox
        // 
        crnBox.Location = new System.Drawing.Point(40, 3);
        crnBox.Name = "crnBox";
        crnBox.Size = new System.Drawing.Size(124, 23);
        crnBox.TabIndex = 1;
        // 
        // prefixPanel
        // 
        prefixPanel.AutoSize = true;
        prefixPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        prefixPanel.Controls.Add(prefixLabel);
        prefixPanel.Controls.Add(prefixBox);
        prefixPanel.Location = new System.Drawing.Point(3, 38);
        prefixPanel.Name = "prefixPanel";
        prefixPanel.Size = new System.Drawing.Size(167, 29);
        prefixPanel.TabIndex = 2;
        prefixPanel.WrapContents = false;
        // 
        // prefixLabel
        // 
        prefixLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom));
        prefixLabel.AutoSize = true;
        prefixLabel.Location = new System.Drawing.Point(3, 0);
        prefixLabel.Name = "prefixLabel";
        prefixLabel.Size = new System.Drawing.Size(37, 29);
        prefixLabel.TabIndex = 0;
        prefixLabel.Text = "Prefix";
        prefixLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // prefixBox
        // 
        prefixBox.Location = new System.Drawing.Point(46, 3);
        prefixBox.Name = "prefixBox";
        prefixBox.Size = new System.Drawing.Size(118, 23);
        prefixBox.TabIndex = 2;
        // 
        // numberPanel
        // 
        numberPanel.AutoSize = true;
        numberPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        numberPanel.Controls.Add(numberLabel);
        numberPanel.Controls.Add(numberBox);
        numberPanel.Location = new System.Drawing.Point(3, 73);
        numberPanel.Name = "numberPanel";
        numberPanel.Size = new System.Drawing.Size(167, 29);
        numberPanel.TabIndex = 3;
        numberPanel.WrapContents = false;
        // 
        // numberLabel
        // 
        numberLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom));
        numberLabel.AutoSize = true;
        numberLabel.Location = new System.Drawing.Point(3, 0);
        numberLabel.Name = "numberLabel";
        numberLabel.Size = new System.Drawing.Size(51, 29);
        numberLabel.TabIndex = 0;
        numberLabel.Text = "Number";
        numberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // numberBox
        // 
        numberBox.Location = new System.Drawing.Point(60, 3);
        numberBox.Name = "numberBox";
        numberBox.Size = new System.Drawing.Size(104, 23);
        numberBox.TabIndex = 3;
        // 
        // yearPanel
        // 
        yearPanel.AutoSize = true;
        yearPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        yearPanel.Controls.Add(yearLabel);
        yearPanel.Controls.Add(yearBox);
        yearPanel.Location = new System.Drawing.Point(3, 108);
        yearPanel.Name = "yearPanel";
        yearPanel.Size = new System.Drawing.Size(167, 29);
        yearPanel.TabIndex = 4;
        yearPanel.WrapContents = false;
        // 
        // yearLabel
        // 
        yearLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom));
        yearLabel.AutoSize = true;
        yearLabel.Location = new System.Drawing.Point(3, 0);
        yearLabel.Name = "yearLabel";
        yearLabel.Size = new System.Drawing.Size(29, 29);
        yearLabel.TabIndex = 0;
        yearLabel.Text = "Year";
        yearLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // yearBox
        // 
        yearBox.Location = new System.Drawing.Point(38, 3);
        yearBox.Name = "yearBox";
        yearBox.Size = new System.Drawing.Size(126, 23);
        yearBox.TabIndex = 4;
        // 
        // semesterPanel
        // 
        semesterPanel.AutoSize = true;
        semesterPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        semesterPanel.Controls.Add(semesterLabel);
        semesterPanel.Controls.Add(semesterBox);
        semesterPanel.Location = new System.Drawing.Point(3, 143);
        semesterPanel.Name = "semesterPanel";
        semesterPanel.Size = new System.Drawing.Size(167, 29);
        semesterPanel.TabIndex = 5;
        semesterPanel.WrapContents = false;
        // 
        // semesterLabel
        // 
        semesterLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom));
        semesterLabel.AutoSize = true;
        semesterLabel.Location = new System.Drawing.Point(3, 0);
        semesterLabel.Name = "semesterLabel";
        semesterLabel.Size = new System.Drawing.Size(55, 29);
        semesterLabel.TabIndex = 0;
        semesterLabel.Text = "Semester";
        semesterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // semesterBox
        // 
        semesterBox.Location = new System.Drawing.Point(64, 3);
        semesterBox.Name = "semesterBox";
        semesterBox.Size = new System.Drawing.Size(100, 23);
        semesterBox.TabIndex = 5;
        // 
        // gradePanel
        // 
        gradePanel.AutoSize = true;
        gradePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        gradePanel.Controls.Add(gradeLabel);
        gradePanel.Controls.Add(gradeBox);
        gradePanel.Location = new System.Drawing.Point(3, 178);
        gradePanel.Name = "gradePanel";
        gradePanel.Size = new System.Drawing.Size(167, 29);
        gradePanel.TabIndex = 6;
        gradePanel.WrapContents = false;
        // 
        // gradeLabel
        // 
        gradeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom));
        gradeLabel.AutoSize = true;
        gradeLabel.Location = new System.Drawing.Point(3, 0);
        gradeLabel.Name = "gradeLabel";
        gradeLabel.Size = new System.Drawing.Size(38, 29);
        gradeLabel.TabIndex = 0;
        gradeLabel.Text = "Grade";
        gradeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // gradeBox
        // 
        gradeBox.Location = new System.Drawing.Point(47, 3);
        gradeBox.Name = "gradeBox";
        gradeBox.Size = new System.Drawing.Size(117, 23);
        gradeBox.TabIndex = 6;
        // 
        // saveButton
        // 
        saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
        saveButton.AutoSize = true;
        saveButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
        saveButton.Location = new System.Drawing.Point(12, 236);
        saveButton.Name = "saveButton";
        saveButton.Size = new System.Drawing.Size(41, 25);
        saveButton.TabIndex = 7;
        saveButton.Text = "Save";
        saveButton.UseVisualStyleBackColor = true;
        saveButton.Click += saveButton_Click;
        //
        // cancelButton
        // 
        cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
        cancelButton.AutoSize = true;
        cancelButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        cancelButton.Location = new System.Drawing.Point(132, 236);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new System.Drawing.Size(53, 25);
        cancelButton.TabIndex = 8;
        cancelButton.Text = "Cancel";
        cancelButton.UseVisualStyleBackColor = true;
        // 
        // AddEditForm
        // 
        AcceptButton = saveButton;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        AutoSize = true;
        AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        CancelButton = cancelButton;
        ClientSize = new System.Drawing.Size(197, 273);
        Controls.Add(cancelButton);
        Controls.Add(fieldsPanel);
        Controls.Add(saveButton);
        DoubleBuffered = true;
        MaximizeBox = false;
        SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Text = "AddEditForm";
        fieldsPanel.ResumeLayout(false);
        fieldsPanel.PerformLayout();
        crnPanel.ResumeLayout(false);
        crnPanel.PerformLayout();
        prefixPanel.ResumeLayout(false);
        prefixPanel.PerformLayout();
        numberPanel.ResumeLayout(false);
        numberPanel.PerformLayout();
        yearPanel.ResumeLayout(false);
        yearPanel.PerformLayout();
        semesterPanel.ResumeLayout(false);
        semesterPanel.PerformLayout();
        gradePanel.ResumeLayout(false);
        gradePanel.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Button cancelButton;

    private System.Windows.Forms.Button saveButton;

    private System.Windows.Forms.FlowLayoutPanel semesterPanel;
    private System.Windows.Forms.Label semesterLabel;
    private System.Windows.Forms.TextBox semesterBox;
    private System.Windows.Forms.FlowLayoutPanel gradePanel;
    private System.Windows.Forms.Label gradeLabel;
    private System.Windows.Forms.TextBox gradeBox;

    private System.Windows.Forms.FlowLayoutPanel numberPanel;
    private System.Windows.Forms.Label numberLabel;
    private System.Windows.Forms.TextBox numberBox;
    private System.Windows.Forms.FlowLayoutPanel yearPanel;
    private System.Windows.Forms.Label yearLabel;
    private System.Windows.Forms.TextBox yearBox;

    private System.Windows.Forms.FlowLayoutPanel prefixPanel;
    private System.Windows.Forms.Label prefixLabel;
    private System.Windows.Forms.TextBox prefixBox;

    private System.Windows.Forms.FlowLayoutPanel crnPanel;
    private System.Windows.Forms.Label crnLabel;
    private System.Windows.Forms.TextBox crnBox;

    private System.Windows.Forms.FlowLayoutPanel fieldsPanel;

    #endregion
}