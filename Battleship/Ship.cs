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
    class Ship
    {
        Image[] Hits = {
            global::Battleship.Properties.Resources.shot,
            global::Battleship.Properties.Resources.hit,
            global::Battleship.Properties.Resources.ship,
            global::Battleship.Properties.Resources.dead
        };

        private int Health;
        private bool Life = true;


        public Ship(int Health)
        {
            this.Health = Health;
        }

        public Image Hit()
        {
            Health -= 1;
            if (Health == 0)
            {
                //MessageBox.Show("Убит");
                Life = false;
                return Hits[3];
            }
            else
            {
                //MessageBox.Show("Попал");
                return Hits[1];
            }
        }
        public bool CheckLife()
        {
            if (Life)
                return Life;
            else
                return Life;
        }
    }
}
