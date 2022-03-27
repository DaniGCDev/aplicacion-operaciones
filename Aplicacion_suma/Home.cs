using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Aplicacion_suma
{
    public partial class Home : Form
    {
        private readonly List<RadioButton> options = new List<RadioButton>();

        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            // Creamos los datos de los 5 botones.
            for (int i = 1; i < 5; i++)
            {
                RadioButton optionButton = new RadioButton
                {
                    Location = new Point(100, 20 + i * 25),
                    Text = i + " digito" + (i > 1 ? "s" : ""),
                };

                // Añadimos el botón a la lista de botones de la vista.
                options.Add(optionButton);
            }

            // Añadimos los botones a la interfaz.
            Controls.AddRange(options.ToArray());
        }

        private void submit_button_Click(object sender, EventArgs e)
        {
            int i = 1;
            foreach (RadioButton option in options)
            {
                if (option.Checked)
                {
                    new Operation(i).Show();
                    Hide();
                    break;
                }

                i++;
            }
        }
    }
}
