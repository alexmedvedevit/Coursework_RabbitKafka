using Receiver.Model;

namespace Receiver.Services
{
    public class FinanceService : IFinanceService
    {
        private IList<FinanceProgram> _finprogs;
        public FinanceService()
        {
            _finprogs = FinancePrograms.Programs;
        }

        public void AllocateFunds(Contract contract)
        {
            var program = _finprogs.Where(x => x.Id == contract.FinProgId).FirstOrDefault();
            if (contract.Price < program.Balance)
            {
                program.Balance -= contract.Price;
            }
        }

    }
}
