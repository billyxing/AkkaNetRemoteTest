using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using MessageClassLibrary;
using TestServer.Control;
using System.Configuration;

namespace TestServer.Actors
{
    public class SelfDBActor:TypedActor,
        IHandle<NormalOrderNumberRequest>,
        IHandle<NormalOrderNumMsg>,
        IHandle<FailedOrderNumMsg>,
        IHandle<UnverifiedOrderNumMsg>,
        IHandle<IncompleteOrderNumMsg>,
        IHandle<OrderDetailRequest>,
        IHandle<SubmitOrderDetail>,
        IHandle<SubmitOrderDetailRespond>
    {

        public void Handle(UnverifiedOrderNumMsg message)
        {
            throw new NotImplementedException();
        }

        public void Handle(OrderDetailRequest message)
        {
            string dbcon = ConfigurationManager.ConnectionStrings["OracleDBStr"].ConnectionString;
            string sqlstr = "select * from self_orderinfo a where a.order_id = '{0}'";
            string result = "";
            DBInternalMessages qResult = OracleHelper.DBQuery(dbcon, string.Format(sqlstr, message.OrderId),out result);
            if(qResult== DBInternalMessages.DB_QuerySuccess)
            {
                Sender.Tell(new OrderDetailRespond("Success", result,message.Subscriber) , Self);
            }
            else
            {
                //error or no data
                Sender.Tell(new OrderDetailRespond("Failed", qResult.ToString(), message.Subscriber), Self);
            }
        }

        public void Handle(SubmitOrderDetail message)
        {
            
            throw new NotImplementedException();
        }

        public void Handle(SubmitOrderDetailRespond message)
        {
            throw new NotImplementedException();
        }

        public void Handle(OrderDetailRespond message)
        {
            throw new NotImplementedException();
        }

        public void Handle(IncompleteOrderNumMsg message)
        {
            throw new NotImplementedException();
        }

        public void Handle(FailedOrderNumMsg message)
        {
            throw new NotImplementedException();
        }

        public void Handle(NormalOrderNumMsg message)
        {
            throw new NotImplementedException();
        }

        public void Handle(NormalOrderNumberRequest message)
        {
            throw new NotImplementedException();
        }








    }
}
