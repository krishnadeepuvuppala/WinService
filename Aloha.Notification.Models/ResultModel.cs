using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aloha.Notification.Models
{
    public class ResultModel
    {
        public string Message { get; set; }
        public string MessageReason { get; set; }
        public bool IsSuccess { get; set; }
    }
}
