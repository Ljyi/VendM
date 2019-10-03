using MediatR;

namespace VendM.Service.EventModel
{
    public class APILog : INotification
    {
        public string APIName { get; set; }
        public string LogContent { get; set; }
    }
}
