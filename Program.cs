using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
//using static System.Net.Mime.MediaTypeNames;

namespace Movimiento_de_boton
{
    internal class Program
    {
        static Random rnd = new Random();
        static Button originalButton;
        static bool buttonCloned = false;
        static int formCounter = 0;
        static int clonedButtonCounter = 0;
        static List<Form> formList = new List<Form>();
        static List<Button> clonedButtonList = new List<Button>();
         int xe = 10;
         int ye = 10;
       

        public void Main(string[] args)
        {
            // Declaración de variables estáticas para ser accesibles desde varios métodos
            //Se encarga del movimiento aleatorio del boton
            Form formulario = new Form();
            formulario.Text = "Movimiento de botón aleatorio";
            formulario.Size = new Size(600, 400);
            formulario.BackColor = Color.CadetBlue;

            //Caracteristicas del boton
            originalButton = new Button();
            originalButton.Text = "Presionar";
            int centerX = (formulario.ClientSize.Width - originalButton.Width) / 2;
            int centerY = (formulario.ClientSize.Height - originalButton.Height) / 2;
            originalButton.Location = new Point(centerX, centerY);
            formulario.Controls.Add(originalButton);

            //Caracteristicas del timer
            Timer timer = new Timer();
            timer.Interval = 40;
            timer.Tick += (sender, e) => MoverBotonAleatoriamente(originalButton, formulario);
            timer.Start();

            //Al dar clicl a la foma esta se reinicia
            formulario.Click += (sender, e) =>
            {
                ReiniciarPrograma();
            };
            //Al presionar el boton se cierra la forma
            originalButton.Click += (sender, e) =>
            {
                Application.Exit();
            };

            Application.Run(formulario);
        }

        public void MoverBotonAleatoriamente(Button boton, Form formulario)
        {

            //int x = boton.Location.X + rnd.Next(-10, 11);
            //int y = boton.Location.Y + rnd.Next(-10, 11);
            int x = boton.Location.X + xe;
            int y = boton.Location.Y + ye;


            // Obtener la ubicación actual del botón
            Point currentLocation = boton.Location;

            // Calcular la nueva ubicación del botón
            Point newLocation = new Point(currentLocation.X + xe, currentLocation.Y + ye);



            if (x < 0)
                x = 0;
            else if (x > formulario.ClientSize.Width - boton.Width)
                x = formulario.ClientSize.Width - boton.Width;

            if (y < 0)
                y = 0;
            else if (y > formulario.ClientSize.Height - boton.Height)
                y = formulario.ClientSize.Height - boton.Height;

            boton.Location = new Point(x, y);

            //Verifica si el boton toca el lado izquiero
            if (x <= 0 && x > -boton.Width && y >= 0 && y <= formulario.ClientSize.Height - boton.Height)
            {
                //Limita a 3 formas
                if (formCounter < 3)
                {
                    //Crea la forma clon
                    Form clonedForm = new Form();
                    clonedForm.Text = "Forma Clon " + (formCounter + 1);
                    clonedForm.Size = formulario.Size;
                    clonedForm.BackColor = formulario.BackColor;
                    clonedForm.StartPosition = FormStartPosition.Manual;
                    clonedForm.Location = new Point(formulario.Location.X + formulario.Width + 10 * (formCounter + 1), formulario.Location.Y);
                    formList.Add(clonedForm);
                    clonedForm.Show();
                    //Lleva el siguimiento de de las formas para que no pase de 3
                    formCounter++;
                }
            }
            //Verifica si el boton toca el lado derecho
            else if (x + boton.Width >= formulario.ClientSize.Width && !buttonCloned)
            {
                ye = -ye;

                // Establecer la nueva ubicación del botón
                boton.Location = newLocation;

                if (clonedButtonCounter < 3)
                {
                    //Crea el boton clon
                    Button clonedButton = new Button();
                    clonedButton.Text = "Nos rendimos";
                    clonedButton.Size = boton.Size;
                    clonedButton.BackColor = boton.BackColor;
                    clonedButton.Location = new Point(rnd.Next(0, formulario.ClientSize.Width - boton.Width), rnd.Next(0, formulario.ClientSize.Height - boton.Height));
                    formulario.Controls.Add(clonedButton);
                    //agrega el boton clon al formulario original
                    clonedButtonList.Add(clonedButton);
                    //Agrega el boton clon a la lista para no crear mas
                    clonedButtonCounter++;
                    //Cambia el estado a true para que indique que se ha clonado el boton
                    buttonCloned = true;
                }
            }
            //verifica si el boton toca la parte superior
            else if (y <= 0 && formCounter > 0)
            {
                //Elimina la forma clon
                Form lastForm = formList[formList.Count - 1];
                formList.Remove(lastForm);
                lastForm.Close();
                //se encarga de quitar el boton clon 
                formCounter--;
            }
           
            //checa si el boton toca la parte inferior de la forma original y si hay botones clones
            else if (y + boton.Height >= formulario.ClientSize.Height && clonedButtonList.Count > 0)
            {

               xe = -xe;

                // Establecer la nueva ubicación del botón
                boton.Location = newLocation;
                //elimina el utlimo boton 
                Button lastClonedButton = clonedButtonList[clonedButtonList.Count - 1];
                formulario.Controls.Remove(lastClonedButton);
                clonedButtonList.Remove(lastClonedButton);
                clonedButtonCounter--;
                buttonCloned = false;
            }

            // Mover el botón clon si está creado
            if (buttonCloned && clonedButtonList.Count > 0)
            {

                Button clonedButton = clonedButtonList[clonedButtonList.Count - 1];
                clonedButton.Location = new Point(rnd.Next(0, formulario.ClientSize.Width - clonedButton.Width), rnd.Next(0, formulario.ClientSize.Height - clonedButton.Height));
            }
        }

        //reinicia la forma
        static void ReiniciarPrograma()
        {
            Application.Restart();
        }
    }
}

