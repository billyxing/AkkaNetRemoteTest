using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using MessageClassLibrary;
using TestServer.Control;
using System.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace TestServer.Actors
{
    public class SelfDBScheduledActor:TypedActor,
        IHandle<GatherOrderNumber>,
        IHandle<ClientConnectRequest>,
        IHandle<Terminated>
    {
        private readonly ICancelable _cancelPublishing;
        private readonly HashSet<ClientConnectRequest> _subscriptions = new HashSet<ClientConnectRequest>();


        protected override void PreStart()
        {
            //create a new instance of performance counter
           
            Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromMinutes(0.5), TimeSpan.FromMinutes(0.5),
                Self, new GatherOrderNumber(), Self, _cancelPublishing);
        }

        protected override void PostStop()
        {
            try
            {
                //terminate the scheduled task
                _cancelPublishing.Cancel(false);
                
            }
            catch
            {

            }
            finally
            {
                base.PostStop();
            }
        }

        


        public void Handle(GatherOrderNumber message)
        {
            string sqlstr = "select a.order_state, count(*) from self_orderinfo a group by a.order_state";
            string dbcon = ConfigurationManager.ConnectionStrings["OracleDBStr"].ConnectionString;
            string result = "";
            DBInternalMessages qresult= OracleHelper.DBQuery(dbcon, sqlstr, out result);
            if(qresult == DBInternalMessages.DB_QuerySuccess)
            {
                OrderNumByType _msg = new OrderNumByType();
                DataSet _dataset= JsonConvert.DeserializeObject<DataSet>(result);
                DataTable _dt = _dataset.Tables[0];
                foreach(DataRow dr in _dt.Rows)
                {
                    if(dr[0].ToString()=="未完成")
                    {
                        _msg.UnfinishedOrder = int.Parse(dr[1].ToString());
                    }
                    else if (dr[0].ToString()=="已交费")
                    {
                        _msg.PaidOrder = int.Parse(dr[1].ToString());
                    }
                    else if (dr[0].ToString() == "待审核")
                    {
                        _msg.WaitForVerifyOrder = int.Parse(dr[1].ToString());
                    }
                    else if (dr[0].ToString() == "待下载")
                    {
                        _msg.ReadyToDownloadOrder = int.Parse(dr[1].ToString());
                    }
                    else if (dr[0].ToString() == "审核失败")
                    {
                        _msg.VerifyFailedOrder = int.Parse(dr[1].ToString());
                    }
                    else if (dr[0].ToString() == "已下载")
                    {
                        _msg.DownloadedOrder = int.Parse(dr[1].ToString());
                    }
                    else if (dr[0].ToString() == "已完成")
                    {
                        _msg.FinishedOrder = int.Parse(dr[1].ToString());
                    }
                    else if (dr[0].ToString() == "已取消")
                    {
                        _msg.CancelledOrder = int.Parse(dr[1].ToString());
                    }
                }

                foreach(ClientConnectRequest ccr in _subscriptions)
                {
                    ccr.Subscriber.Tell(_msg, Self);
                }

                
            }
            else 
            {
                foreach (ClientConnectRequest ccr in _subscriptions)
                {
                    ccr.Subscriber.Tell(new GatherOrderNumberFailed() { Reason = qresult.ToString() }, Self);
                }
            }

        }

        public void Handle(ClientConnectRequest message)
        {
            _subscriptions.Add(message);
            Console.WriteLine("add subscription:" + message.ClientCN);
            
            Context.Watch(message.Subscriber);
        }

        public void Handle(Terminated message)
        {
            Console.WriteLine("receive unsubscription:" + message.ToString());
            _subscriptions.RemoveWhere(p => p.Subscriber == message.ActorRef);
            Console.WriteLine(message.ToString());
        }
    }
}
