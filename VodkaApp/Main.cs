using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace VodkaApp
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            chart1.Series.Clear();
            chart1.Series.Add(new Series("Фиксируем прибыль")
            {
                ChartType = SeriesChartType.Spline
            });
        }

        int bottleAmount { get; set; } = STARTER;
        int sold = 0;
        float income = 0;
        float hours { get; set; } = 0.0f;
        float timeUntilVodka { get; set; } = -1;

        const float VODKA_PRICE = 150f;
        const float PENALTY = 40f;
        const int STARTER = 100;

        static int RndGen() => new Random().Next(1, 4);
        void Logger(string message) => listBoxLog.Items.Add($"[Час {hours.ToString("0.0")}] {message}");
        float ProfFormula() => (VODKA_PRICE * sold) - (PENALTY * (STARTER - sold));
        float DifFormula() => (VODKA_PRICE * STARTER) - (PENALTY * (sold - STARTER));

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
                } else if (hours >= timeUntilVodka)
                {
                    bottleAmount += 100;
                    check = false;
                }
                labelVodkaAmount.Text += $"У нас поставка через {Math.Abs(timeUntilVodka - hours).ToString("0.0")} часов.";
            }
        }

        void IncomeLogic()
        {
            income += bottleAmount > 0 ? ProfFormula() : -DifFormula();
            chart1.Series["Фиксируем прибыль"].Points.AddXY(hours.ToString("0.0"), income);
            labelInfo.Text = $"Доход: {income}p";
        }

        void ShopIter(int amount)
        {
            Logger($"Покупатель взял {amount} бутылок водки.");
            sold += amount;
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
