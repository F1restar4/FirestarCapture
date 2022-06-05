namespace FirestarCapture
{
	partial class KeyBind
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
			this.KeyBindLabel = new System.Windows.Forms.Label();
			this.ChangeBindings = new System.Windows.Forms.Button();
			this.SubmitButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// KeyBindLabel
			// 
			this.KeyBindLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.KeyBindLabel.AutoSize = true;
			this.KeyBindLabel.Location = new System.Drawing.Point(87, 67);
			this.KeyBindLabel.Name = "KeyBindLabel";
			this.KeyBindLabel.Size = new System.Drawing.Size(78, 15);
			this.KeyBindLabel.TabIndex = 0;
			this.KeyBindLabel.Text = "CTRL SHIFT C";
			// 
			// ChangeBindings
			// 
			this.ChangeBindings.Location = new System.Drawing.Point(37, 168);
			this.ChangeBindings.Name = "ChangeBindings";
			this.ChangeBindings.Size = new System.Drawing.Size(75, 23);
			this.ChangeBindings.TabIndex = 1;
			this.ChangeBindings.Text = "Change binding";
			this.ChangeBindings.UseVisualStyleBackColor = true;
			this.ChangeBindings.Click += new System.EventHandler(this.ChangeBindings_Click);
			// 
			// SubmitButton
			// 
			this.SubmitButton.Location = new System.Drawing.Point(148, 168);
			this.SubmitButton.Name = "SubmitButton";
			this.SubmitButton.Size = new System.Drawing.Size(75, 23);
			this.SubmitButton.TabIndex = 2;
			this.SubmitButton.Text = "Submit";
			this.SubmitButton.UseVisualStyleBackColor = true;
			this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
			// 
			// KeyBind
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(262, 232);
			this.Controls.Add(this.SubmitButton);
			this.Controls.Add(this.ChangeBindings);
			this.Controls.Add(this.KeyBindLabel);
			this.KeyPreview = true;
			this.Name = "KeyBind";
			this.Text = "FirestarCapture Key Binding";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyBind_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Label KeyBindLabel;
		private Button ChangeBindings;
		private Button SubmitButton;
	}
}