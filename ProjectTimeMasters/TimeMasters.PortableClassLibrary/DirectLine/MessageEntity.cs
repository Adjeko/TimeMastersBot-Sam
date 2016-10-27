using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMasters.PortableClassLibrary.DirectLine
{
    public class MessageEntity
    {
        //private string _author;
        private string _message;


        public MessageEntity()
        {

        }

        //public string Author
        //{
        //    get { return _author; }
        //    set { _author = Author; }
        //}

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }
}
