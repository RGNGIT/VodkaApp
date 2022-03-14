using System;
using System.Windows.Forms;

namespace VodkaApp
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        int bottleAmount { get; set; } = 100;
        float hours { get; set; } = 0.0f;
        float timeUntilVodka { get; set; } = -1;

        static int RndGen() => new Random().Next(1, 4);
        void Logger(string message) => listBoxLog.Items.Add($"[Час {hours.ToString("0.0")}] {message}");

        bool check = false;

        new void Update()
        {
            labelVodkaAmount.Text = $"Бутылок осталось: {bottleAmount}. ";
            DeliveryLogic();
            IncomeLogic();
        }

        void DeliveryLogic()
        {
            if (bottleAmount < 50)
            {
                if (!check)
                {
                    timeUntilVodka = hours + 5f;
                    check = true;
                } else if(hours >= timeUntilVodka)
                {
                    bottleAmount += 100;
                    check = false;
                }
                labelVodkaAmount.Text += $"У нас поставка через {Math.Abs(timeUntilVodka - hours).ToString("0.0")} часов.";
            }
        }

        void IncomeLogic()
        {

        }

        void ShopIter(int amount)
        {
            Logger($"Покупатель взял {amount} бутылок водки.");
            bottleAmount -= amount;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            timer.Start();
            buttonStart.Text = "Работаем...";
            buttonStart.Enabled = false;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            ShopIter(RndGen());
            hours += 0.1f;
            Update();
        }
    }
}
