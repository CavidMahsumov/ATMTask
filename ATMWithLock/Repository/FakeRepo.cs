using ATMWithLock.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMWithLock.Repository
{
    public class FakeRepo
    {
        public List<Card> GetAll()
        {
            return new List<Card>
            {
                new Card
                {
                  CardNumber="2222333344445555",
                  Username="Username",
                  Balance="566233.221"
                },
                new Card
                {
                  CardNumber="7777888899990000",
                  Username="Username1",
                  Balance="40.5"
                }
            };
        }
    }
}
