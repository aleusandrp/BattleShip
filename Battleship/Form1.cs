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

        Random rand = new Random();


        ///////////////
        private bool move = true;
        int Gx;
        int Gy;
        int state = 0;
        ///////////////









        private int P4 = 1;
        private int P3 = 2;
        private int P2 = 3;
        private int P1 = 4;

        
        RadioButton CheckedShip;

        // Кол-во столбцов/строк
        private const int
           Col = 10,
           Row = 10;
        // Массив картинок поля компьютера
        PictureBox[,] CompField = new PictureBox[Col, Row];
        // Игровое поле компьютера
        Ship[,] CompShips = new Ship[Col, Row];
        // Массив картинок поля пользователя
        PictureBox[,] UserField = new PictureBox[Col, Row];
        // Игровое поле пользователя
        Ship[,] UserShips = new Ship[Col, Row];
        // Массив картинок
        Image[] Images =
        {
            global::Battleship.Properties.Resources.sea,
            global::Battleship.Properties.Resources.shot,
            global::Battleship.Properties.Resources.hit,
            global::Battleship.Properties.Resources.ship,
            global::Battleship.Properties.Resources.dead
        };

        public Form1()
        {
            InitializeComponent();
        }

        private bool ChangePSum()
        {
            if(P1+P2+P3+P4 == 0)
            {
                MessageBox.Show("Все корабли расставлены, можете начать игру");
                Vect_checkBox.Enabled = false;
                return true;
            }
            return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            VisibleClearUserField();
            StopGame_button.Enabled = false;
        }
        // Установка буфферной зоны после убийства корабля
        private void Buffer(int x, int y)
        {
            // углы точки
            if (y != 0 && x != 0)
                CompField[x - 1, y - 1].Image = Images[1];
            if (y != 9 && x != 9)
                CompField[x + 1, y + 1].Image = Images[1];
            if (y != 0 && x != 9)
                CompField[x + 1, y - 1].Image = Images[1];
            if (y != 9 && x != 0)
                CompField[x - 1, y + 1].Image = Images[1];
            // боковины точки
            if (x != 9)
                if (CompShips[x + 1, y] == null)
                    CompField[x + 1, y].Image = Images[1];

            if (x != 0)
                if (CompShips[x - 1, y] == null)
                    CompField[x - 1, y].Image = Images[1];

            if (y != 0)
                if (CompShips[x, y - 1] == null)
                    CompField[x, y - 1].Image = Images[1];

            if (y != 9)
                if (CompShips[x, y + 1] == null)
                    CompField[x, y + 1].Image = Images[1];
        }
        // Если после выстрела корабль убит, обновляем поле рисуем мертвый корабль
        private void UpdateField()
        {
            for (int x = 0; x < Col; x++)
            {
                for (int y = 0; y < Row; y++)
                {
                    if (CompShips[x, y] != null)
                        if (!CompShips[x, y].CheckLife())
                        {
                            Buffer(x, y);
                            CompField[x, y].Image = Images[4];
                        }
                }
            }
        }
        // Обработчик клика по полю компьютера
        private void ClickCompField(Object sender, EventArgs e)
        {
            if (!move)
                return;

            PictureBox SenderPicture = sender as PictureBox;

            int x = Convert.ToInt32(SenderPicture.Name.Substring(0, 1));
            int y = Convert.ToInt32(SenderPicture.Name.Substring(2, 1));
            if (CompField[x, y].Image != Images[0])
            {
                MessageBox.Show("В эту клетку уже стреляли");
                return;
            }
            if (CompShips[x, y] != null)
            {
                SenderPicture.Image = CompShips[x, y].Hit();
                if (!CompShips[x, y].CheckLife())
                    UpdateField();
                return;
            }
            else
                SenderPicture.Image = Images[1];
            move = false;
            CompMove(state);
        }

        public void CompMove(int action)
        {
            switch (action)
            {
                case 0:
                    Shot();
                    return;
            }
        }
        public void Shot()
        {
            Gx = rand.Next(0, 10);
            Gy = rand.Next(0, 10);

            if (UserShips[Gx, Gy] == null)
            {
                UserField[Gx, Gy].Image = Images[1];
                //MessageBox.Show(UserField[Gx, Gy].Image.ToString());
                move = true;
                return;
            }

            UserField[Gx, Gy].Image = Images[2];
            //MessageBox.Show(UserField[Gx, Gy].Image.ToString());

            CompMove(state);
        }
        // Рисуем поле компьютера (чистое море)
        private void CreateField()
        {
            Comp_panel.Controls.Clear();
            // обнуляем массив картинок
            CompField = new PictureBox[Col, Row];

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
                        SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
                    };
                    Comp_panel.Controls.Add(Picture);
                    PosX += 20;
                    //
                    CompField[x, y] = Picture;

                    Picture.Click += ClickCompField;
                }
                PosX = 0;
                PosY += 20;
            }
        }
        // Проверка координаты выданной Ships Generate
        private bool CheckXY(string ChooseShip, int x, int y, int Length, int vect)
        {
            Ship[,] Choosen = new Ship[Row, Col];
            if (ChooseShip == "Comp")
                Choosen = CompShips;
            else
                Choosen = UserShips;

            if (vect == 1)
            {
                for(int i = 0; i < Length; i++)
                {
                    // чек вниз тела
                    if (Choosen[x + i, y] != null)
                        return false;
                    // если не касается боком правого края поля
                    if (y != 9)
                    {
                        if (Choosen[x + i, y + 1] != null)
                            return false;
                    }
                    // если не касается боком левого края поля
                    if(y != 0)
                    {
                        if (Choosen[x + i, y - 1] != null)
                            return false;
                    }
                }
                // точка выше координаты 
                if (x != 0)
                    if (Choosen[x - 1, y] != null) 
                        return false;
                // точка ниже координаты 
                if (x != 10 - Length)
                    if (Choosen[x + Length, y] != null)
                        return false;
                // верхняя левая точка
                if (y != 0 && x != 0)
                    if (Choosen[x - 1, y - 1] != null)
                        return false;
                // верхняя правая точка
                if (y != 9 && x != 0)
                    if (Choosen[x - 1, y + 1] != null)
                        return false;
                // нижняя левая точка
                if (y != 0 && x != 10 - Length)
                    if (Choosen[x + Length, y - 1] != null)
                        return false;
                // нижняя правая точка
                if (y != 9 && x != 10 - Length)
                    if (Choosen[x + Length, y + 1] != null)
                        return false;
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    // чек вправо тела
                    if (Choosen[x, y + i] != null)
                        return false;
                    // если не касается боком верхнего края поля
                    if (x != 0)
                    {
                        if (Choosen[x - 1 , y + i] != null)
                            return false;
                    }
                    // если не касается боком нижнего края поля
                    if (x != 9)
                    {
                        if (Choosen[x + 1, y + i] != null)
                            return false;
                    }
                }
                // точка левее координаты 
                if (y != 0)
                    if (Choosen[x, y - 1] != null)
                        return false;
                // точка правее координаты 
                if (y != 10 - Length)
                    if (Choosen[x, y + Length] != null)
                        return false;
                // левая верхняя точка
                if (y != 0 && x != 0)
                    if (Choosen[x - 1, y - 1] != null)
                        return false;
                // левая нижняя точка
                if (y != 0 && x != 9)
                    if (Choosen[x + 1, y - 1] != null)
                        return false;
                // правая верхняя точка
                if (x != 0 && y != 10 - Length)
                    if (Choosen[x - 1, y + Length] != null)
                        return false;
                // правая нижняя точка
                if (x != 9 && y != 10 - Length)
                    if (Choosen[x + 1, y + Length] != null)
                        return false;
            }
            return true;
        }
        // Генерация кораблей в массивы UserShips/CompShips
        private void ShipsGenerate(string ChooseShip, int Health, Ship Ship)
        {
            Ship[,] Choosen = new Ship[Row,Col];
            if (ChooseShip == "Comp")
                Choosen = CompShips;
            else
                Choosen = UserShips;

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
                    x = rand.Next(0, 11-Length);

                    nice = CheckXY(ChooseShip, x, y, Length, vect);
                    if (nice)
                    {
                        for (int i = 0; i < Length; i++)
                        {
                            Choosen[x + i, y] = Ship;
                        }
                    }
                    
                }
                else
                {
                    y = rand.Next(0, 11-Length);
                    x = rand.Next(0, 10);

                    nice = CheckXY(ChooseShip, x, y, Length, vect);
                    if (nice)
                    {
                        for (int i = 0; i < Length; i++)
                        {
                            Choosen[x, y + i] = Ship;
                        }
                    }
                }
            }
        }
        //Ставим корабль в выюранную клетку
        private void ShipsGenerate(int Length, int x, int y, int vect, Ship Ship)
        {
            if (vect == 1)
            {
                for (int i = 0; i < Length; i++)
                {
                    UserShips[x + i, y] = Ship;
                }
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    UserShips[x, y + i] = Ship;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Gen_checkBox.Checked || ChangePSum())
            {
                StartGame_button.Enabled = false;
                StopGame_button.Enabled = true;
                Ships_groupBox.Enabled = false;
                GameLog_listBox.Items.Add("Игра начата");
                CreateRound();
                CreateField();
                StopGame_button.Focus();
            }
            else
            {
                MessageBox.Show("Не все корабли расставлены\nРасставьте их в ручную или выберите автоматическую генерацию", "Ошибка");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        // Создание кораблей компьютера
        private void CreateCompShips()
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

            ShipsGenerate("Comp", 4, Ship4_1);

            ShipsGenerate("Comp", 3, Ship3_1);
            ShipsGenerate("Comp", 3, Ship3_2);

            ShipsGenerate("Comp", 2, Ship2_1);
            ShipsGenerate("Comp", 2, Ship2_2);
            ShipsGenerate("Comp", 2, Ship2_3);

            ShipsGenerate("Comp", 1, Ship1_1);
            ShipsGenerate("Comp", 1, Ship1_2);
            ShipsGenerate("Comp", 1, Ship1_3);
            ShipsGenerate("Comp", 1, Ship1_4);
        }
        // Действие при поражении, очистка полей и панелей
        private void FallGame()
        {
            // Обнуляем массив картинок компьютера
            CompField = new PictureBox[Col, Row];
            // Обнуляем игровое поле компьютера
            CompShips = new Ship[Col, Row];
            // Обнуляем массив картинок пользователя
            UserField = new PictureBox[Col, Row];
            // Обнуляем игровое поле пользователя
            UserShips = new Ship[Col, Row];
            Comp_panel.Controls.Clear();
            User_panel.Controls.Clear();
        }

        private void StopGame_button_Click(object sender, EventArgs e)
        {
            FallGame();
            StopGame_button.Enabled = false;
            StartGame_button.Enabled = true;
            Ships_groupBox.Enabled = true;
            GameLog_listBox.Items.Clear();
            VisibleClearUserField();
            MessageBox.Show("Вы досрочно закончили игру, можете начать новую.", "Игра завершена");
        }
        // Создание кораблей игрока (при выборе кнопки сгенерировать)
        private void CreateUserShips()
        {
            // 4 палубники
            Ship Ship4_11 = new Ship(4);
            // 3 палубники
            Ship Ship3_11 = new Ship(3);
            Ship Ship3_21 = new Ship(3);
            // 2 палубники
            Ship Ship2_11 = new Ship(2);
            Ship Ship2_21 = new Ship(2);
            Ship Ship2_31 = new Ship(2);
            // 1 палубники
            Ship Ship1_11 = new Ship(1);
            Ship Ship1_21 = new Ship(1);
            Ship Ship1_31 = new Ship(1);
            Ship Ship1_41 = new Ship(1);

            ShipsGenerate("User", 4, Ship4_11);

            ShipsGenerate("User", 3, Ship3_11);
            ShipsGenerate("User", 3, Ship3_21);

            ShipsGenerate("User", 2, Ship2_11);
            ShipsGenerate("User", 2, Ship2_21);
            ShipsGenerate("User", 2, Ship2_31);

            ShipsGenerate("User", 1, Ship1_11);
            ShipsGenerate("User", 1, Ship1_21);
            ShipsGenerate("User", 1, Ship1_31);
            ShipsGenerate("User", 1, Ship1_41);

        }

        public void ShowShip(int Length, int x, int y, int vect)
        {
            if (vect == 1)
            {
                for (int i = 0; i < Length; i++)
                {
                    UserField[x + i, y].Image = Images[3];
                }
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    UserField[x, y + i].Image = Images[3];
                }
            }
        }

        // Обработчик клика по полю пользователя
        private void ClickUserField(Object sender, EventArgs e)
        {
            PictureBox SenderPicture = sender as PictureBox;

            int x = Convert.ToInt32(SenderPicture.Name.Substring(0, 1));
            int y = Convert.ToInt32(SenderPicture.Name.Substring(2, 1));



            try {
                
                int CheckLX = x + Convert.ToInt32(CheckedShip.Name[1].ToString());
                int CheckLY = y + Convert.ToInt32(CheckedShip.Name[1].ToString());
                int Length = Convert.ToInt32(CheckedShip.Name[1].ToString());
                int vect = Vect_checkBox.Checked ? 1 : 0;

                if (!Vect_checkBox.Checked && CheckLY > 10)
                {
                    MessageBox.Show("Корабль не влeзет");
                    return;
                }
                else if (Vect_checkBox.Checked && CheckLX > 10)
                {
                    MessageBox.Show("Корабль не влeзет");
                    return;
                }
                else if (!CheckXY("User", x, y, Length, vect))
                {
                    MessageBox.Show("Занято другим кораблем");
                    return;
                }

                Ship Ship = new Ship(Length);

                switch (CheckedShip.Name)
                {
                    case "P4_radioButton":
                        {
                            ShipsGenerate(Length, x, y, vect, Ship);
                            ShowShip(Length, x, y, vect);
                            //VisibleUserShips();
                            P4--;
                            if (P4 == 0)
                            {
                                P4_radioButton.Enabled = false;
                                P4_radioButton.Checked = false;
                                CheckedShip = null;
                            }
                            ChangePSum();
                            break;
                        }
                    case "P3_radioButton":
                        {
                            ShipsGenerate(Length, x, y, vect, Ship);
                            ShowShip(Length, x, y, vect);
                            //VisibleUserShips();
                            P3--;
                            if (P3 == 0)
                            {
                                P3_radioButton.Enabled = false;
                                P3_radioButton.Checked = false;
                                CheckedShip = null;
                            }
                            ChangePSum();
                            break;
                        }
                    case "P2_radioButton":
                        {
                            ShipsGenerate(Length, x, y, vect, Ship);
                            ShowShip(Length, x, y, vect);
                            //VisibleUserShips();
                            P2--;
                            if (P2 == 0)
                            {
                                P2_radioButton.Enabled = false;
                                P2_radioButton.Checked = false;
                                CheckedShip = null;
                            }
                            ChangePSum();
                            break;
                        }
                    case "P1_radioButton":
                        {
                            ShipsGenerate(Length, x, y, vect, Ship);
                            ShowShip(Length, x, y, vect);
                            //VisibleUserShips();
                            P1--;
                            if (P1 == 0)
                            {
                                P1_radioButton.Enabled = false;
                                P1_radioButton.Checked = false;
                                CheckedShip = null;
                            }
                            ChangePSum();
                            break;
                        }
                }
            }
            catch
            {
                MessageBox.Show("Выберите тип корабля");
            }
        }
        // Рисуем корабли юзура
        private void VisibleUserShips()
        {
            int PosY = 0;
            for (int x = 0; x < Col; x++)
            {
                int PosX = 0;
                for (int y = 0; y < Row; y++)
                {
                    Image Img;
                    if (UserShips[x, y] != null)
                        Img = Images[3];
                    else
                        Img = Images[0];

                    PictureBox Picture = new PictureBox()
                    {
                        Image = Img,
                        Location = new System.Drawing.Point(PosX, PosY),
                        Name = x + ";" + y,
                        Size = new System.Drawing.Size(20, 20),
                        SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
                    };

                    User_panel.Controls.Add(Picture);

                    Picture.Click += ClickUserField;
                    PosX += 20;
                }
                PosX = 0;
                PosY += 20;
            }




        }
        // Рисуем чистое поле юзеру при старте игры
        private void VisibleClearUserField()
        {
            int PosY = 0;
            for (int x = 0; x < Col; x++)
            {
                int PosX = 0;
                for (int y = 0; y < Row; y++)
                {
                    Image Img;
                    if (UserShips[x, y] != null)
                        Img = Images[3];
                    else
                        Img = Images[0];

                    PictureBox Picture = new PictureBox()
                    {
                        Image = Img,
                        Location = new System.Drawing.Point(PosX, PosY),
                        Name = x + ";" + y,
                        Size = new System.Drawing.Size(20, 20),
                        SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
                    };

                    User_panel.Controls.Add(Picture);
                    PosX += 20;
                    //
                    UserField[x, y] = Picture;

                    Picture.Click += ClickUserField;
                }
                PosX = 0;
                PosY += 20;
            }




        }
        // Изменение кнопки "Сгенерировать"
        private void Gen_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Gen_checkBox.Checked)
            {
                UserShips = new Ship[Col, Row];
                User_panel.Controls.Clear();
                CreateUserShips();
                VisibleUserShips();
                HandGen_panel.Hide();
            }
            else
            {
                UserShips = new Ship[Col, Row];
                User_panel.Controls.Clear();
                HandGen_panel.Show();

                VisibleClearUserField();
                P4 = 1;
                P3 = 2;
                P2 = 3;
                P1 = 4;
                P1_radioButton.Enabled = true;
                P2_radioButton.Enabled = true;
                P3_radioButton.Enabled = true;
                P4_radioButton.Enabled = true;
                Vect_checkBox.Enabled = true;
            }
        }


        private void P4_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckedShip = (RadioButton)sender;
        }

        private void CompDataClear()
        {
            // Обнуляем массив картинок компьютера
            CompField = new PictureBox[Col, Row];
            // Обнуляем игровое поле компьютера
            CompShips = new Ship[Col, Row];
            Comp_panel.Controls.Clear();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(Properties.Resources.HandGen);
        }

        private void Refresh_button_Click(object sender, EventArgs e)
        {
            UserShips = new Ship[Col, Row];
            UserField = new PictureBox[Col, Row];
            User_panel.Controls.Clear();
            HandGen_panel.Show();

            VisibleClearUserField();
            P4 = 1;
            P3 = 2;
            P2 = 3;
            P1 = 4;
            P1_radioButton.Enabled = true;
            P2_radioButton.Enabled = true;
            P3_radioButton.Enabled = true;
            P4_radioButton.Enabled = true;
            Vect_checkBox.Enabled = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            move = true;
        }

        private void CreateRound()
        {
            CompDataClear();
            CreateCompShips();
        }
    }
}