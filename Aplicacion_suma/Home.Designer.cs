namespace Aplicacion_suma
{
    partial class Home
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
            this.title = new System.Windows.Forms.Label();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Location = new System.Drawing.Point(31, 9);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(222, 13);
            this.title.TabIndex = 0;
            this.title.Text = "Elige la cantidad de digitos que quieres sumar";
            // 
            // SubmitButton
            // 
            this.SubmitButton.Location = new System.Drawing.Point(197, 226);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(75, 23);
            this.SubmitButton.TabIndex = 1;
            this.SubmitButton.Text = "Ir a la suma >>";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.submit_button_Click);
            // 
            // Home
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.title);
            this.Name = "Home";
            this.Text = "Inicio";
            this.Load += new System.EventHandler(this.Home_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Button SubmitButton;
    }
}