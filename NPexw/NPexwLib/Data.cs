namespace NPexwLib
{
    //класс для передачи информации по сети
    public class Data//класс для будущей передачи данных между клиентами и сервером;
    {
        public bool IsWon { get; set; } = false;
        public Sign Sign { get; set; } = Sign.Cross;

        public string Status = "Crosses\' Turn";
        //public Cell Cell { get; set; } = new Cell(0, 0, 0);
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Value = 0;
    }
}