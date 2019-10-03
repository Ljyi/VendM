using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VendM.Service.EventModel;

namespace VendM.Service.EventHandler
{
    public class LogHandler : INotificationHandler<APILog>
    {
        public Task Handle(APILog notification, CancellationToken cancellationToken)
        {
            //添加操作
            return Task.FromResult(true);
        }
    }
}
