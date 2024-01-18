using Receiver.Model;

namespace Receiver.Services
{
    public interface IFinanceService
    {
        void AllocateFunds(Contract contract);
    }
}
