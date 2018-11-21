using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aloha.Notification.Models
{
    public class SubscriberDetailsModel
    {
        public Guid SubscriptionId { get; set; }
        public string UserDbServer { get; set; }
        public string SubscriberUrl { get; set; }
        public string UserDb { get; set; }
        public string ActivityDbServer { get; set; }
        public string ActivityDb { get; set; }
    }
}
