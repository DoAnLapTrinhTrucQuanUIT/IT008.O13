using Restaurant_Management.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Models
{
    public class Const : ViewModelBase
    {
        private static Const _instance;

        public string UserId { get; private set; }

        public static Const Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Const();
                }
                return _instance;
            }
        }

        public void SetUser(string userId)
        {
            UserId = userId;
        }
    }
}
