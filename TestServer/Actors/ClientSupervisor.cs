using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using MessageClassLibrary;

namespace TestServer.Actors
{
    public class ClientSupervisor : TypedActor,
        IHandle<ClientConnectRequest>,
        IHandle<ClientDisconnectRequest>,
        IHandle<Terminated>,
        IHandle<GatherOrderNumberFailed>,
        IHandle<OrderNumByType>,
        IHandle<OrderDetailRequest>,
        IHandle<OrderDetailRespond>,
        IHandle<SubmitOrderDetail>,
        IHandle<SubmitOrderDetailRespond>

    {

        private readonly HashSet<ClientConnectRequest> _subscriptions = new HashSet<ClientConnectRequest>();

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(10,TimeSpan.FromSeconds(5),x=>
            {
                return Directive.Stop;
            });
        }

        public void Handle(OrderNumByType message)
        {
            foreach(ClientConnectRequest ccr in _subscriptions)
            {
                ccr.Subscriber.Tell(message);
            }
        }

        public void Handle(GatherOrderNumberFailed message)
        {
            foreach (ClientConnectRequest ccr in _subscriptions)
            {
                ccr.Subscriber.Tell(message);
            }
        }

        public void Handle(OrderDetailRequest message)
        {
            ActorSelection _dbactor = Context.ActorSelection("../Self_DB");
            Task<OrderDetailRespond> result = _dbactor.Ask<OrderDetailRespond>(message);
            message.Subscriber.Tell(result.Result);
        }


        public void Handle(OrderDetailRespond message)
        {
            throw new NotImplementedException();
        }

        public void Handle(SubmitOrderDetailRespond message)
        {
            throw new NotImplementedException();
        }
        
        public void Handle(SubmitOrderDetail message)
        {
            throw new NotImplementedException();
        }

       

        public void Handle(ClientDisconnectRequest message)
        {
            Console.WriteLine("receive unsubscription:" + message.ToString());
            _subscriptions.RemoveWhere(p => p.Subscriber == message.Subscriber);
            Console.WriteLine(message.ToString());
            Context.Unwatch(message.Subscriber);
            Context.Stop(message.Subscriber);
        }
        
        public void Handle(ClientConnectRequest message)
        {
            _subscriptions.Add(message);
            Console.WriteLine("add subscription:" + message.ClientCN);

            Context.Watch(message.Subscriber);
        }

        public void Handle(Terminated message)
        {
            Console.WriteLine("receive Terminated Msg:" + message.ToString());
            _subscriptions.RemoveWhere(p => p.Subscriber == message.ActorRef);
            Console.WriteLine(message.ToString());
            Context.Unwatch(message.ActorRef);
            Context.Stop(message.ActorRef);
            
        }
    }
}
