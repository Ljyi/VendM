using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VendM.Service.EventModel;

namespace VendM.Service.EventHandler
{
    public class EmailHandler : INotificationHandler<Email>
    {
        public Task Handle(Email notification, CancellationToken cancellationToken)
        {
            //发送邮件
            //Send email  
            return Task.Run(() =>
            {
                return true;
            });
            //   return Task.FromResult(true);
        }
    }
}