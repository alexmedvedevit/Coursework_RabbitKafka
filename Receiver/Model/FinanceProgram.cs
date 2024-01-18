namespace Receiver.Model
{
    public class FinanceProgram
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Balance { get; set; }
    }

    public static class FinancePrograms
    {
        public static IList<FinanceProgram> Programs = new List<FinanceProgram>
            {
               new FinanceProgram{ Id = 104, Title= "Обеспечение ОМСУ техникой и поддержание её работоспособности", Balance = 1000000.00 },
               new FinanceProgram{ Id = 102, Title= "Обеспечение ОМСУ телематической связью и интернет-соединением", Balance = 2520000.48},
               new FinanceProgram{ Id = 201, Title= "Информационная безопасность", Balance = 1400000.00},
               new FinanceProgram{ Id = 301, Title= "Обеспечение ОМСУ программным обеспечением", Balance = 300000.00},
            };
    }
}
