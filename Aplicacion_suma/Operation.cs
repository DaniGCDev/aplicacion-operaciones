using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplicacion_suma
{
    public partial class Operation : Form
    {
        private int number1 = 0;
        private int number2 = 0;

        private int resultNumber = 0;
        private TextBox[] resultBoxes;

        private readonly List<decimal> haulages = new List<decimal>();
        private TextBox[] haulagesBoxes;

        public Operation(int option)
        {
            InitializeComponent();

            // Generamos de forma aleatoria los números a sumar.
            // Si son iguales se vuelven a generar hasta que sean diferentes.
            while (number1 == number2)
            {
                number1 = GenerateRandom(option, new Random());
                number2 = GenerateRandom(option, new Random());
            }

            // Sumamos los números generados y guardamos el resultado.
            resultNumber = number1 + number2;

            GetHaulages();
        }

        // Función para generar un número aleatorio según la opción seleccionada en
        // el formulario anterior.
        private int GenerateRandom(int option, Random r)
        {
            switch (option)
            {
                case 2: return r.Next(10, 100);
                case 3: return r.Next(100, 1000);
                case 4: return r.Next(1000, 10000);

                case 1:
                default:
                    return r.Next(1, 10);
            };
        }

        // Obtenemos los acarreos.
        private void GetHaulages()
        {
            // Convertimos el número 1 a String, se separa por caracter (número) y se coloca al revés.
            var arrayNumber1 = number1.ToString().ToCharArray().Reverse().Select((x) => int.Parse(x.ToString())).ToList();

            // Convertimos el número 2 a String, se separa por caracter (número) y se coloca al revés.
            var arrayNumber2 = number2.ToString().ToCharArray().Reverse().Select((x) => int.Parse(x.ToString())).ToList();

            // Realizamos las sumas de cada unidad.
            for (int i = 0; i < arrayNumber1.Count(); i++)
            {
                decimal resultado = arrayNumber1[i] + arrayNumber2[i] + (i > 0 ? haulages[i - 1] : 0);
                if (resultado >= 10)
                {
                    decimal acarreo = resultado / 10;
                    haulages.Add(Math.Truncate(acarreo));
                }
                else
                {
                    haulages.Add(0);
                }
            }

            haulages.Reverse();
        }

        private void Operation_Load(object sender, EventArgs e)
        {
            const int spacing = 20;

            // Generamos los campos de texto necesarios para los acarreos.
            int haulagesCount = 1;
            haulagesBoxes = haulages.Select((x) =>
            {
                haulagesCount++;

                if (x == 0) return null;

                return new TextBox {
                    Location = new Point(spacing * haulagesCount, 10),
                    MaxLength = 1,
                    Size = new Size(15, 15),
                };
            }).ToArray();

            // Añadimos los campos de los acarreos a la interfaz.
            Controls.AddRange(haulagesBoxes);

            // Generamos los campos de texto necesarios para el total.
            var resultNumberChars = resultNumber.ToString().ToCharArray();
            int resultCount = 1;

            resultBoxes = resultNumberChars.Select((x) => {
                resultCount++;

                return new TextBox
                {
                    Location = new Point(spacing * resultCount, 100),
                    MaxLength = 1,
                    Size = new Size(15, 15),
                };
            }).ToArray();

            // Añadimos los campos de los totales a la interfaz.
            Controls.AddRange(resultBoxes);

            // Generamos los labels del número 1.
            var number1Chars = number1.ToString().ToCharArray();
            int number1Count = resultNumberChars.Length - number1Chars.Length + 1;

            number1Chars.ToList().ForEach((x) => {
                number1Count++;

                Controls.Add(new Label
                {
                    Location = new Point(spacing * number1Count, 40),
                    Size = new Size(15, 15),
                    Text = x.ToString(),
                });
            });

            // Generamos los labels del número 2.
            var number2Chars = number2.ToString().ToCharArray();
            int number2Count = resultNumberChars.Length - number2Chars.Length + 1;

            number2Chars.ToList().ForEach((x) => {
                number2Count++;

                Controls.Add(new Label
                {
                    Location = new Point(spacing * number2Count, 70),
                    Size = new Size(15, 15),
                    Text = x.ToString() + ".",
                });
            });
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            new Home();
            Hide();
        }

        private void VerifyButton_Click(object sender, EventArgs e)
        {
            bool hasError = false;

            // Validamos que los números en los campos de acarreo sean válidos.
            foreach (var (textBox, i) in haulagesBoxes.Select((value, i) => (value, i)))
            {
                if (textBox != null)
                {
                    if (textBox.Text.Count() > 0)
                    {
                        decimal value = decimal.Parse(textBox.Text);
                        if (value != haulages[i] && !hasError)
                        {
                            hasError = true;
                            MessageBox.Show("El valor es incorrecto");
                            break;
                        }
                    }
                    else if (!hasError)
                    {
                        hasError = true;
                        MessageBox.Show("Debes ingresar todos los campos.");
                        break;
                    }
                }
            }

            if (!hasError) {
                var resultNumbers = resultNumber.ToString().ToCharArray().Select((x) => int.Parse(x.ToString())).ToList();
                foreach (var (textBox, i) in resultBoxes.Select((value, i) => (value, i)))
                {
                    if (textBox != null)
                    {
                        if (textBox.Text.Count() > 0)
                        {
                            decimal value = decimal.Parse(textBox.Text);
                            if (value != resultNumbers[i] && !hasError)
                            {
                                hasError = true;
                                MessageBox.Show("El valor es incorrecto");
                                break;
                            }
                        }
                        else if (!hasError)
                        {
                            hasError = true;
                            MessageBox.Show("Debes ingresar todos los campos.");
                            break;
                        }
                    }
                }
            }

            if (!hasError)
            {
                MessageBox.Show("El resultado es correcto! 🥳");
            }
        }
    }
}
