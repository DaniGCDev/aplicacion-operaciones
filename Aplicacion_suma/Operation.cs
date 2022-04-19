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
        private readonly string type;

        private const int spacing = 20;

        private readonly int number1 = 0;
        private readonly int number2 = 0;

        private readonly List<List<decimal>> multiplyResultNumbers = new List<List<decimal>>();
        private readonly List<decimal> resultNumbers = new List<decimal>();
        private int resultNumber = 0;
        private TextBox[] resultBoxes;

        private int initialY = 130;

        private readonly List<List<decimal>> haulages = new List<List<decimal>>();
        private TextBox[] haulagesBoxes;
        private int haulagesIndex = 0;

        private bool validatingPlus = false;

        public Operation(string type, int option)
        {
            this.type = type;

            InitializeComponent();

            // Generamos de forma aleatoria los números a sumar.
            // Si son iguales se vuelven a generar hasta que sean diferentes.
            while (number1 == number2)
            {
                number1 = GenerateRandom(option, new Random());
                number2 = GenerateRandom(option, new Random());
            }

            if (type == "plus")
            {
                // Sumamos los números generados y guardamos el resultado.
                resultNumber = number1 + number2;
            }
            else if (type == "multiply")
            {
                // Multiplicamos los números generados y guardamos el reusltado.
                resultNumber = number1 * number2;
            }

            // Creamos un vector con cada número del resultado.
            resultNumbers = resultNumber.ToString().ToCharArray().Select((x) => decimal.Parse(x.ToString())).ToList();

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

        private void GetHaulages()
        {
            // Convertimos el número 1 a String, se separa por caracter (número) y se coloca al revés.
            List<decimal> arrayNumber1 = number1.ToString().ToCharArray().Reverse().Select((x) => decimal.Parse(x.ToString())).ToList();

            // Convertimos el número 2 a String, se separa por caracter (número) y se coloca al revés.
            List<decimal> arrayNumber2 = number2.ToString().ToCharArray().Reverse().Select((x) => decimal.Parse(x.ToString())).ToList();

            // Guardamos los acarreos.
            List<decimal> haulagesInt = new List<decimal>();

            // Realizamos las sumas de cada unidad.
            for (int i = 0; i < arrayNumber2.Count(); i++)
            {
                if (type == "plus")
                {
                    decimal result = arrayNumber1[i] + arrayNumber2[i] + (i > 0 ? haulagesInt[i - 1] : 0);
                    if (result >= 10)
                    {
                        decimal haulage = result / 10;
                        haulagesInt.Add(Math.Truncate(haulage));
                    }
                    else
                    {
                        haulagesInt.Add(0);
                    }
                }
                else if (type == "multiply")
                {
                    List<decimal> haulageResult = new List<decimal>();

                    for (int j = 0; j < arrayNumber1.Count(); j++) {
                        decimal result = (arrayNumber1[j] * arrayNumber2[i]) + (j > 0 ? haulagesInt[j - 1] : 0);
                        if (result >= 10)
                        {
                            decimal haulage = result / 10;
                            decimal haulageInt = Math.Truncate(haulage);

                            haulageResult.Add((haulage - haulageInt) * 10);
                            if (j == arrayNumber1.Count() - 1)
                            {
                                haulageResult.Add(haulageInt);
                            }

                            haulagesInt.Add(haulageInt);
                        }
                        else
                        {
                            haulageResult.Add(result);
                            haulagesInt.Add(0);
                        }
                    }

                    haulageResult.Reverse();
                    multiplyResultNumbers.Add(haulageResult);

                    haulagesInt.Reverse();

                    // Agregamos los acarreos del número actual al vector de acarreos.
                    haulages.Add(haulagesInt);

                    // Limpiamos el vector interno de acarreos.
                    haulagesInt = new List<decimal>();
                }
            }

            if (type == "plus")
            {
                haulagesInt.Reverse();
                haulages.Add(haulagesInt);
            }
            else if (type == "multiply")
            {
                List<List<decimal>> plusNumbers = new List<List<decimal>>();
                for (int i = 0; i < haulages.Count(); i++)
                {
                    List<decimal> haulagesNumbers = new List<decimal>();

                    decimal initialSpacing = resultNumbers.Count() - (multiplyResultNumbers[i].Count() + i);
                    if (initialSpacing > 0)
                    {
                        for (int j = 0; j < initialSpacing; j++)
                        {
                            haulagesNumbers.Add(0);
                        }
                    }

                    haulagesNumbers.AddRange(multiplyResultNumbers[i]);

                    for (int j = 0; j < i; j++)
                    {
                        haulagesNumbers.Add(0);
                    }

                    haulagesNumbers.Reverse();
                    plusNumbers.Add(haulagesNumbers);
                }

                List<decimal> plusHaulages = new List<decimal>();
                for (int i = 0; i < plusNumbers[0].Count(); i++)
                {
                    decimal result = 0;
                    for (int j = 0; j < plusNumbers.Count(); j++)
                    {
                        result += plusNumbers[j][i];
                    }

                    if (i > 0)
                    {
                        result += plusHaulages[i - 1];
                    }

                    if (result >= 10)
                    {
                        plusHaulages.Add(Math.Truncate(result / 10));
                    }
                    else
                    {
                        plusHaulages.Add(0);
                    }
                }

                plusHaulages.Reverse();
                haulages.Add(plusHaulages);
            }
        }

        private int LoadPlus()
        {
            // Generamos los campos de texto necesarios para los acarreos.
            haulagesBoxes = haulages[0].Select((x, i) =>
            {
                if (x == 0) return null;

                return new TextBox
                {
                    Location = new Point((i == 0 ? 1 : (i + 1)) * spacing, 10),
                    MaxLength = 1,
                    Size = new Size(15, 15),
                };
            }).ToArray();

            // Añadimos los campos de los acarreos a la interfaz.
            Controls.AddRange(haulagesBoxes);

            // Generamos los campos de texto necesarios para el total.
            var resultNumberChars = resultNumber.ToString().ToCharArray();

            resultBoxes = resultNumberChars.Select((x, i) => new TextBox
            {
                Location = new Point((i + 1) * spacing, 100),
                MaxLength = 1,
                Size = new Size(15, 15),
            }).ToArray();

            // Añadimos los campos de los totales a la interfaz.
            Controls.AddRange(resultBoxes);

            return resultNumberChars.Length;
        }

        private void GenerateHaulages(int index)
        {
            // Eliminamos las cajas de texto 
            if (haulagesBoxes != null && haulagesBoxes.Count() > 0)
            {
                foreach (TextBox tb in haulagesBoxes)
                {
                    Controls.Remove(tb);
                }
            }

            List<TextBox> haulagesInt = new List<TextBox>();
            for (int i = 0; i < haulages[index].Count(); i++)
            {
                if (haulages[index][i] == 0)
                {
                    haulagesInt.Add(null);
                    continue;
                }

                haulagesInt.Add(new TextBox
                {
                    Location = new Point((resultNumbers.Count() - haulages[index].Count() + i) * spacing, 10),
                    MaxLength = 1,
                    Size = new Size(15, 15),
                });
            }

            haulagesBoxes = haulagesInt.ToArray();

            // Añadimos los campos de los acarreos a la interfaz.
            Controls.AddRange(haulagesBoxes);
        }

        private void GenerateResults(int count, int initialSpacing, int y)
        {
            List<TextBox> toData = new List<TextBox>();
            for (int i = 0; i < count; i++)
            {
                int x = initialSpacing + spacing * i;
                toData.Add(new TextBox
                {
                    Location = new Point(x, y),
                    MaxLength = 1,
                    Size = new Size(15, 15),
                });
            }

            resultBoxes = toData.ToArray();
            Controls.AddRange(resultBoxes);
        }

        private int LoadMultiply()
        {
            // Generamos los campos de texto necesarios para los acarreos.
            GenerateHaulages(0);

            // Obtenemos el espaciado inicial.
            int initialSpacing = multiplyResultNumbers.Count() > 0 ? resultNumbers.Count() - multiplyResultNumbers[0].Count() : 0;

            bool noHasPlusHaulages = haulages[haulages.Count() - 1].All((h) => h == 0);
            if (noHasPlusHaulages)
            {
                initialY = 100;
            }

            // Generamos el primer resultado de la multiplicación.
            GenerateResults(
                multiplyResultNumbers.Count() > 0 ? multiplyResultNumbers[0].Count() : 0,
                (initialSpacing + 1) * spacing,
                initialY
            );

            return resultNumbers.Count();
        }

        private void Operation_Load(object sender, EventArgs e)
        {
            int resultNumberChars = 0;
            if (type == "plus")
            {
                resultNumberChars = LoadPlus();
            }
            else if (type == "multiply")
            {
                resultNumberChars = LoadMultiply();
            }

            // Generamos los labels del número 1.
            var number1Chars = number1.ToString().ToCharArray();

            Controls.AddRange(
                number1Chars
                    .Select((x, i) => new Label {
                        Location = new Point((resultNumberChars - number1Chars.Length + 1 + i) * spacing, 40),
                        Size = new Size(15, 15),
                        Text = x.ToString(),
                    })
                    .ToArray()
            );

            // Generamos los labels del número 2.
            var number2Chars = ((type == "multiply" ? "x" : "+") + number2.ToString()).ToCharArray();

            Controls.AddRange(
                number2Chars
                    .Select((x, i) => new Label {
                        Location = new Point((resultNumberChars - number2Chars.Length + 1 + i) * spacing, 70),
                        Size = new Size(15, 15),
                        Text = x.ToString(),
                    })
                    .ToArray()
            );
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            new Home().Show();
            Hide();
        }

        private void ReplaceResults(bool withHaulages = false)
        {
            // Reemplazamos los campos de texto por labels.
            Controls.AddRange(
                resultBoxes
                    .Select((tb) => new Label
                    {
                        Location = tb.Location,
                        Size = new Size(15, 15),
                        Text = tb.Text,
                    })
                    .ToArray()
            );

            // Eliminamos los campos de texto anteriores.
            foreach (TextBox tb in resultBoxes)
            {
                Controls.Remove(tb);
            }

            // Reemplazamos los acarreos si se requiere.
            if (withHaulages)
            {
                // Eliminamos los campos de texto anteriores.
                foreach (TextBox tb in haulagesBoxes)
                {
                    Controls.Remove(tb);
                }
            }
        }

        private bool VerifyHaulages(int index)
        {
            bool hasError = false;
            foreach (var (textBox, i) in haulagesBoxes.Select((value, i) => (value, i)))
            {
                if (textBox != null)
                {
                    if (textBox.Text.Count() > 0)
                    {
                        decimal value = decimal.Parse(textBox.Text.Trim());
                        if (value != haulages[index][i] && !hasError)
                        {
                            hasError = true;
                            MessageBox.Show("El valor es incorrecto");
                            break;
                        }
                    }
                    else if (haulages[index][i] != 0 && !hasError)
                    {
                        hasError = true;
                        MessageBox.Show("Debes ingresar todos los campos");
                        break;
                    }
                }
            }

            return hasError;
        }

        private void VerifyButton_Click(object sender, EventArgs e)
        {
            // Validamos que los números en los campos de acarreo sean válidos.
            bool hasError = VerifyHaulages(haulagesIndex);

            if (!hasError) {
                if (type == "plus")
                {
                    foreach (var (textBox, i) in resultBoxes.Select((value, i) => (value, i)))
                    {
                        if (textBox != null)
                        {
                            if (textBox.Text.Count() > 0)
                            {
                                decimal value = decimal.Parse(textBox.Text.Trim());
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
                                MessageBox.Show("Debes ingresar todos los campos");
                                break;
                            }
                        }
                    }
                }
                else if (type == "multiply")
                {
                    if (haulagesIndex < multiplyResultNumbers.Count())
                    {
                        foreach (var (textBox, i) in resultBoxes.Select((value, i) => (value, i)))
                        {
                            if (textBox != null)
                            {
                                if (textBox.Text.Count() > 0)
                                {
                                    decimal value = decimal.Parse(textBox.Text.Trim());
                                    if (value != multiplyResultNumbers[haulagesIndex][i] && !hasError)
                                    {
                                        hasError = true;
                                        MessageBox.Show("El valor es incorrecto");
                                        break;
                                    }
                                }
                                else if (!hasError)
                                {
                                    hasError = true;
                                    MessageBox.Show("Debes ingresar todos los campos");
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (!hasError)
            {
                // Si se eligió realizar una multiplicación iremos evaluando cada resultado hasta llegar al último.
                if (type == "multiply")
                {
                    // Estamos evaluando el proceso de multiplicación del número 2 con el número 1.
                    if ((haulagesIndex + 1) < multiplyResultNumbers.Count())
                    {
                        ReplaceResults();

                        haulagesIndex++;
                        GenerateHaulages(haulagesIndex);

                        // Obtenemos el espaciado inicial de los resultados.
                        int resultCount = multiplyResultNumbers[haulagesIndex].Count();
                        int initialSpacing = (resultNumbers.Count() - (resultCount + haulagesIndex) + 1) * spacing;

                        // Obtenemos el valor de y.
                        int y = initialY + (haulagesIndex * 30);

                        // Generamos los nuevos campos de texto.
                        GenerateResults(resultCount, initialSpacing > 0 ? initialSpacing : spacing, y);

                        // Agregamos el signo de suma.
                        if (haulagesIndex == 1)
                        {
                            Controls.Add(
                                new Label
                                {
                                    Location = new Point(
                                        (initialSpacing > 0 ? initialSpacing : spacing) + resultCount * spacing,
                                        y
                                    ),
                                    Size = new Size(15, 15),
                                    Text = "+",
                                }
                            );
                        }

                        return;
                    }
                    else if (haulagesIndex == 0)
                    {
                        validatingPlus = true;
                    }
                    else if (haulagesIndex != haulages.Count() - 1)
                    {
                        haulagesIndex++;
                    }

                    if (validatingPlus)
                    {
                        hasError = VerifyHaulages(haulagesIndex);
                        if (!hasError)
                        {
                            for (int i = 0; i < resultNumbers.Count(); i++)
                            {
                                TextBox tb = resultBoxes[i];
                                if (tb != null)
                                {
                                    if (tb.Text.Count() == 1)
                                    {
                                        if (int.Parse(tb.Text) != resultNumbers[i])
                                        {
                                            MessageBox.Show("El valor es incorrecto");
                                            hasError = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Debes ingresar todos los campos");
                                        hasError = true;
                                        break;
                                    }
                                }
                            }

                            if (!hasError)
                            {
                                ReplaceResults(true);
                                MessageBox.Show("El resultado es correcto! 🥳");
                            }
                        }
                    }
                    else
                    {
                        ReplaceResults();

                        // Añadimos los campos de texto del resultado de la suma.
                        GenerateResults(resultNumbers.Count(), spacing, initialY + (haulagesIndex * 30));

                        foreach (TextBox tb in haulagesBoxes)
                        {
                            Controls.Remove(tb);
                        }

                        // Generamos los campos de texto necesarios para los acarreos.
                        List<TextBox> haulagesInt = new List<TextBox>();

                        List<decimal> plusHaulages = haulages[haulages.Count() - 1];
                        for (int i = 0; i < plusHaulages.Count(); i++)
                        {
                            if (plusHaulages[i] == 0)
                            {
                                haulagesInt.Add(null);
                                continue;
                            }

                            int x = i * spacing;

                            haulagesInt.Add(new TextBox
                            {
                                Location = new Point(x > 0 ? x : spacing, 100),
                                MaxLength = 1,
                                Size = new Size(15, 15),
                            });
                        }

                        haulagesBoxes = haulagesInt.ToArray();

                        // Añadimos los campos de los acarreos a la interfaz.
                        Controls.AddRange(haulagesBoxes);

                        validatingPlus = true;
                    }
                }
                else
                {
                    ReplaceResults(true);
                    MessageBox.Show("El resultado es correcto! 🥳");
                }
            }
        }
    }
}
