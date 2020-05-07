using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DroPoker
{
    class GameForm : Form
    {
        Table table;
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private List<int> replaceCardArray;
        private string winner;
        private int sum;

        public GameForm(DirectoryInfo imagesDirectory = null)
        {
            table = new Table();

            ClientSize = new Size(1280, 720);
            FormBorderStyle = FormBorderStyle.FixedDialog;

            if (imagesDirectory == null)
                imagesDirectory = new DirectoryInfo("Resources");
            foreach (var e in imagesDirectory.GetFiles("*.jpg"))
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);
            Paint += PaintTable;
            MaximizeBox = false;

            table.GameFinish += FinishGame;
            CreateControlers();
        }

        private void PaintTable(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.DarkGreen, 0, 0, 1280, 720);
            DrawWhiteRectangle(e);
            if (table.PlayerCard[0] != null)
            {
                for (var i = 0; i < 5; i++)
                    e.Graphics.DrawImage(bitmaps[table.PlayerCard[i].Item2.ToString() + table.PlayerCard[i].Item1.ToString() + ".jpg"], new Point(506 + i * 58, 645));
            }
            e.Graphics.DrawString(table.PlayerMoney.ToString(), new Font("Arial", 16), Brushes.Black, new Point(400, 645));
            e.Graphics.DrawString(table.ComputerMoney.ToString(), new Font("Arial", 16), Brushes.Black, new Point(400, 20));
        }

        private void PaintFinish(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.DarkGreen, 0, 0, 1280, 720);
            DrawWhiteRectangle(e);
            e.Graphics.DrawString(winner + ": " + sum.ToString(), new Font("Arial", 16), Brushes.Black, new Point(600, 300));
            e.Graphics.DrawString(table.AnanlizeCombination(table.PlayerCard).ToString(), new Font("Arial", 16), Brushes.Black, new Point(800, 644));
            e.Graphics.DrawString(table.AnanlizeCombination(table.ComputerCard).ToString(), new Font("Arial", 16), Brushes.Black, new Point(800, 20));
            if (table.PlayerCard[0] != null)
            {
                for (var i = 0; i < 5; i++)
                {
                    e.Graphics.DrawImage(bitmaps[table.ComputerCard[i].Item2.ToString() + table.ComputerCard[i].Item1.ToString() + ".jpg"], new Point(506 + i * 58, 21));
                    e.Graphics.DrawImage(bitmaps[table.PlayerCard[i].Item2.ToString() + table.PlayerCard[i].Item1.ToString() + ".jpg"], new Point(506 + i * 58, 645));
                }
            }
        }

        private void PaintPlayerWin(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.DarkGreen, 0, 0, 1280, 720);
            e.Graphics.DrawString("Вы выиграли", new Font("Arial", 16), Brushes.Black, new Point(600, 300));
        }

        private void PaintCompWin(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.DarkGreen, 0, 0, 1280, 720);
            e.Graphics.DrawString("Вы проиграли", new Font("Arial", 16), Brushes.Black, new Point(600, 300));
        }

        private void DrawWhiteRectangle(PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.White, new Rectangle(new Point(621, 644), new Size(38, 56)));
            e.Graphics.DrawRectangle(Pens.White, new Rectangle(new Point(563, 644), new Size(38, 56)));
            e.Graphics.DrawRectangle(Pens.White, new Rectangle(new Point(505, 644), new Size(38, 56)));
            e.Graphics.DrawRectangle(Pens.White, new Rectangle(new Point(679, 644), new Size(38, 56)));
            e.Graphics.DrawRectangle(Pens.White, new Rectangle(new Point(737, 644), new Size(38, 56)));
            e.Graphics.DrawRectangle(Pens.White, new Rectangle(new Point(621, 20), new Size(38, 56)));
            e.Graphics.DrawRectangle(Pens.White, new Rectangle(new Point(563, 20), new Size(38, 56)));
            e.Graphics.DrawRectangle(Pens.White, new Rectangle(new Point(505, 20), new Size(38, 56)));
            e.Graphics.DrawRectangle(Pens.White, new Rectangle(new Point(679, 20), new Size(38, 56)));
            e.Graphics.DrawRectangle(Pens.White, new Rectangle(new Point(737, 20), new Size(38, 56)));
        }

        private void CreateControlers()
        {
            #region контроллеры
            var buttonBid = new Button();
            var textBid = new TextBox();
            var checkBox1 = new CheckBox();
            var checkBox2 = new CheckBox();
            var checkBox3 = new CheckBox();
            var checkBox4 = new CheckBox();
            var checkBox5 = new CheckBox();


            checkBox1.Location = new Point(519, 630);
            checkBox1.Size = new Size(10, 10);

            Controls.Add(checkBox1);

            checkBox2.Location = new Point(577, 630);
            checkBox2.Size = new Size(10, 10);
            Controls.Add(checkBox2);

            checkBox3.Location = new Point(635, 630);
            checkBox3.Size = new Size(10, 10);
            Controls.Add(checkBox3);

            checkBox4.Location = new Point(693, 630);
            checkBox4.Size = new Size(10, 10);
            Controls.Add(checkBox4);

            checkBox5.Location = new Point(751, 630);
            checkBox5.Size = new Size(10, 10);
            Controls.Add(checkBox5);

            buttonBid.Location = new Point(1180, 680);
            buttonBid.Size = new Size(100, 40);
            buttonBid.Text = "Сделать ставку";
            buttonBid.Click += (sender, e) =>
            {
                if (!int.TryParse(textBid.Text, out int result) || result < 0)
                {
                    textBid.Text = "Неверная ставка";
                    return;
                }
                if (result > table.ComputerMoney)
                {
                    textBid.Text = "У ПК нет денег";
                    return;
                }
                if (result > table.PlayerMoney)
                {
                    textBid.Text = "У вас нет денег";
                    return;
                }
                replaceCardArray = new List<int>();
                if (checkBox1.Checked) replaceCardArray.Add(0);
                if (checkBox2.Checked) replaceCardArray.Add(1);
                if (checkBox3.Checked) replaceCardArray.Add(2);
                if (checkBox4.Checked) replaceCardArray.Add(3);
                if (checkBox5.Checked) replaceCardArray.Add(4);

                table.MovePlayer(result, replaceCardArray);
                Invalidate();
            };
            Controls.Add(buttonBid);

            textBid.Location = new Point(1180, 660);
            textBid.Size = new Size(100, 20);
            textBid.Text = "Введите ставку";
            Controls.Add(textBid);
            #endregion
        }

        private void FinishGame(string winner, int sum)
        {
            this.winner = winner;
            this.sum = sum;
            Controls.Clear();
            if (table.PlayerMoney == 0)
            {
                Controls.Clear();
                var button = new Button();
                button.Location = new Point(1180, 680);
                button.Size = new Size(100, 40);
                button.Text = "Начать заново";
                button.Click += (sender, e) =>
                {
                    Paint -= PaintCompWin;
                    table = new Table();
                    table.GameFinish += FinishGame;
                    Controls.Clear();
                    CreateControlers();
                    Invalidate();
                };
                Controls.Add(button);
                Paint += PaintCompWin;
                Invalidate();
                return;
            }
            else if (table.ComputerMoney == 0)
            {
                Controls.Clear();
                var button = new Button();
                button.Location = new Point(1180, 680);
                button.Size = new Size(100, 40);
                button.Text = "Начать заново";
                button.Click += (sender, e) =>
                {
                    Paint -= PaintPlayerWin;
                    table = new Table();
                    table.GameFinish += FinishGame;
                    Controls.Clear();
                    CreateControlers();
                    Invalidate();
                };
                Controls.Add(button);
                Paint += PaintPlayerWin;
                Invalidate();
                return;
            }
            var butNextTable = new Button();
            butNextTable.Location = new Point(1180, 680);
            butNextTable.Size = new Size(100, 40);
            butNextTable.Text = "Следующая партия";
            butNextTable.Click += (sender, e) =>
            {
                Paint -= PaintFinish;
                table.NewRound();
                Controls.Clear();
                CreateControlers();
                Invalidate();
            };
            Controls.Add(butNextTable);
            Paint += PaintFinish;
            Invalidate();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "Poker";
            DoubleBuffered = true;
        }
    }
}
