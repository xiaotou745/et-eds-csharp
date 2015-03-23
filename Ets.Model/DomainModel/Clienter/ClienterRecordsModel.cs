using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Clienter
{
    public class ClienterRecordsModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreateTime { get; set; }
        public string UserName { get; set; }
        public string AdminName { get; set; }
    }

    public class ClienterRecordsListModel
    {
        public ClienterModel clienterModel { get; set; }
        public IList<ClienterRecordsModel> listClienterRecordsModel { get; set; }
    }


}
