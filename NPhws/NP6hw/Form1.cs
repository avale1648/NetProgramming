namespace NP6hw
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer.Interval = 300_000;//обновлять раз в пять минут (5 мин = 5 * 60 с = 5 * 60 * 1000 мс);
            timer_Tick(sender, e);//первичное получение информации;
            timer.Start();//запуск таймера;
        }
        private async void timer_Tick(object sender, EventArgs e)
        {
            await Weather.GetDataFromOpenWeatherMap();//получение информации о погоде с OpenWeatherMap;
            pictureBoxIcon.ImageLocation = Weather.Icon;//передача информации об адресе картинки;
            label.Text = $"{Weather.Description}, {Weather.Temperature}";//передача информации о погоде в лейбл;
            Text = "Погода в Москве, " + label.Text;//передача информации о погоде в заголовок формы;
        }
    }
}