using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalusMobileApp.Models
{
    public class CommentModel
    {
        public int id { get; set; }
        public int fromId { get; set; }
        public int toId { get; set; }
        public string email { get; set; }
        public string body { get; set; }
        public DateTime sendDate { get; set; }
        public CommentModel() { }
        public CommentModel(int id, int fromId, int toId, string body, DateTime sendDate)
        {
            this.id = id;
            this.fromId = fromId;
            this.toId = toId;
            this.body = body;
            this.sendDate = sendDate;
        }
        public CommentModel(string email, string body)
        {
            this.email = email;
            this.body = body;
        }
    }
}
