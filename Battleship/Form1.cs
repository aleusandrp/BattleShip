using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship
{
    public partial class Form1 : Form
    {
        // 4 палубники
        Ship Ship4_1 = new Ship(4);
        // 3 палубники
        Ship Ship3_1 = new Ship(3);
        Ship Ship3_2 = new Ship(3);
        // 2 палубники
        Ship Ship2_1 = new Ship(2);
        Ship Ship2_2 = new Ship(2);
        Ship Ship2_3 = new Ship(2);
        // 1 палубники
        Ship Ship1_1 = new Ship(1);
        Ship Ship1_2 = new Ship(1);
        Ship Ship1_3 = new Ship(1);
        Ship Ship1_4 = new Ship(1);

        Random rand = new Random();
        private const int
           Col = 10,
           Row = 10;

        // Массив расположения спрайтов
        PictureBox[,] Field = new PictureBox[Col,Row];
        // Игровое поле
        Ship[,] Ships = new Ship[Col,Row];
        // Массив картинок
        Image[] Images =
        {
            global::Battleship.Properties.Resources.sea,
            global::Battleship.Properties.Resources.shot,
            global::Battleship.Properties.Resources.hit,
            global::Battleship.Properties.Resources.ship,
            global::Battleship.Properties.Resources.dead
        };
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void новаяИграToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateField();
            StartGame();
        }

        private void Buffer(int x, int y)
        {
            // углы точки
            if (y != 0 && x != 0)
                Field[x - 1, y - 1].Image = Images[1];
            if (y != 9 && x != 9)
                Field[x + 1, y + 1].Image = Images[1];
            if (y != 0 && x != 9)
                Field[x + 1, y - 1].Image = Images[1];
            if (y != 9 && x != 0)
                Field[x - 1, y + 1].Image = Images[1];
            // боковины точки
            if (x != 9)
                if (Ships[x + 1, y] == null)
                    Field[x + 1, y].Image = Images[1];

            if (x != 0)
                if (Ships[x - 1, y] == null)
                    Field[x - 1, y].Image = Images[1];

            if (y != 0)
                if (Ships[x, y - 1] == null)
                    Field[x, y - 1].Image = Images[1];

            if (y != 9)
                if (Ships[x, y + 1] == null)
                    Field[x, y + 1].Image = Images[1];
        }

        private void UpdateField()
        {
            for (int x = 0; x < Col; x++)
            {
                for (int y = 0; y < Row; y++)
                {
                    if (Ships[x, y] != null)
                        if (!Ships[x, y].CheckLife())
                        {
                            Buffer(x, y);
                            Field[x, y].Image = Images[4];
                        }
                }
            }
        }

        private void ClickPicture(Object sender, EventArgs e)
        {
            PictureBox SenderPicture = sender as PictureBox;

            int x = Convert.ToInt32(SenderPicture.Name.Substring(0, 1));
            int y = Convert.ToInt32(SenderPicture.Name.Substring(2, 1));
            if (Field[x, y].Image != Images[0])
            {
                MessageBox.Show("В эту клетку уже стреляли");
                return;
            }
            if (Ships[x, y] != null)
            {
                SenderPicture.Image = Ships[x, y].Hit();
                if (!Ships[x, y].CheckLife())
                    UpdateField();
            }
            else
                SenderPicture.Image = Images[1];
        }

        private void CreateField()
        {
            panel1.Controls.Clear();
            panel2.Controls.Clear();
            Field = new PictureBox[Col, Row];

            Ships = new Ship[Col, Row];

            int PosY = 0;
            for (int x = 0; x < Col; x++)
            {
                int PosX = 0;
                for (int y = 0; y < Row; y++)
                {
                    PictureBox Picture = new PictureBox()
                    {
                        Image = Images[0],
                        Location = new System.Drawing.Point(PosX, PosY),
                        Name = x + ";" + y,
                        Size = new System.Drawing.Size(20, 20),
                        SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
                    };
                    panel1.Controls.Add(Picture);
                    PosX += 20;
                    //
                    Field[x, y] = Picture;

                    Picture.Click += ClickPicture;
                }
                PosX = 0;
                PosY += 20;
            }
        }

        private bool CheckXY(int x, int y, int Length, int vect)
        {
            if(vect == 1)
            {
                for(int i = 0; i < Length; i++)
                {
                    // чек вниз тела
                    if (Ships[x + i, y] != null)
                        return false;
                    // если не касается боком правого края поля
                    if (y != 9)
                    {
                        if (Ships[x + i, y + 1] != null)
                            return false;
                    }
                    // если не касается боком левого края поля
                    if(y != 0)
                    {
                        if (Ships[x + i, y - 1] != null)
                            return false;
                    }
                }
                // точка выше координаты 
                if (x != 0)
                    if (Ships[x - 1, y] != null) 
                        return false;
                // точка ниже координаты 
                if (x != 10 - Length)
                    if (Ships[x + Length, y] != null)
                        return false;
                // верхняя левая точка
                if (y != 0 && x != 0)
                    if (Ships[x - 1, y - 1] != null)
                        return false;
                // верхняя правая точка
                if (y != 9 && x != 0)
                    if (Ships[x - 1, y + 1] != null)
                        return false;
                // нижняя левая точка
                if (y != 0 && x != 10 - Length)
                    if (Ships[x + Length, y - 1] != null)
                        return false;
                // нижняя правая точка
                if (y != 9 && x != 10 - Length)
                    if (Ships[x + Length, y + 1] != null)
                        return false;
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    // чек вправо тела
                    if (Ships[x, y + i] != null)
                        return false;
                    // если не касается боком верхнего края поля
                    if (x != 0)
                    {
                        if (Ships[x - 1 , y + i] != null)
                            return false;
                    }
                    // если не касается боком нижнего края поля
                    if (x != 9)
                    {
                        if (Ships[x + 1, y + i] != null)
                            return false;
                    }
                }
                // точка левее координаты 
                if (y != 0)
                    if (Ships[x, y - 1] != null)
                        return false;
                // точка правее координаты 
                if (y != 10 - Length)
                    if (Ships[x, y + Length] != null)
                        return false;
                // левая верхняя точка
                if (y != 0 && x != 0)
                    if (Ships[x - 1, y - 1] != null)
                        return false;
                // левая нижняя точка
                if (y != 0 && x != 9)
                    if (Ships[x + 1, y - 1] != null)
                        return false;
                // правая верхняя точка
                if (x != 0 && y != 10 - Length)
                    if (Ships[x - 1, y + Length] != null)
                        return false;
                // правая нижняя точка
                if (x != 9 && y != 10 - Length)
                    if (Ships[x + 1, y + Length] != null)
                        return false;
            }
            return true;
        }

        private void ShipsGenerate(int Health, Ship Ship)
        {
            int Length = Health;
            int x, y;
            int vect;
            bool nice = false;

            while (!nice)
            {
                vect = rand.Next(0, 2);
                if (vect == 1)
                {
                    y = rand.Next(0, 10);
                    x = rand.Next(0, 7);

                    nice = CheckXY(x, y, Length, vect);
                    if (nice)
                    {
                        for (int i = 0; i < Length; i++)
                        {
                            Ships[x + i, y] = Ship;
                        }
                    }
                    
                }
                else
                {
                    y = rand.Next(0, 7);
                    x = rand.Next(0, 10);

                    nice = CheckXY(x, y, Length, vect);
                    if (nice)
                    {
                        for (int i = 0; i < Length; i++)
                        {
                            Ships[x, y + i] = Ship;
                        }
                    }
                }
            }
        }

        private void морскойБойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateField();
            StartGame();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void StartGame()
        {
            ShipsGenerate(4, Ship4_1);

            ShipsGenerate(3, Ship3_1);
            ShipsGenerate(3, Ship3_2);

            ShipsGenerate(2, Ship2_1);
            ShipsGenerate(2, Ship2_2);
            ShipsGenerate(2, Ship2_3);

            ShipsGenerate(1, Ship1_1);
            ShipsGenerate(1, Ship1_2);
            ShipsGenerate(1, Ship1_3);
            ShipsGenerate(1, Ship1_4);
        }
    }
}
