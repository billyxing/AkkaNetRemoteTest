using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka;
using Akka.Actor;
using MessageClassLibrary;

namespace TestServer.Actors
{
    public class ServerActor:TypedActor,
        IHandle<NormalOrderNumberRequest>,
        IHandle<NormalOrderNumMsg>,
        IHandle<FailedOrderNumMsg>,
        IHandle<UnverifiedOrderNumMsg>,
        IHandle<IncompleteOrderNumMsg>,
        IHandle<OrderDetailRequest>,
        IHandle<OrderDetailRespond>,
        IHandle<SubmitOrderDetail>,
        IHandle<SubmitOrderDetailRespond>,
        IHandle<ClientConnectRequest>,
        IHandle<Terminated>

      

    {
        private readonly HashSet<IActorRef> _clients = new HashSet<IActorRef>();

        private IActorRef _selfDBActor;
        private IActorRef _selfDBScheduledActor;

        public ServerActor()
        {
            _selfDBActor = Context.ActorOf( Props.Create(() => new SelfDBActor()),"Self_DB");
            _selfDBScheduledActor = Context.ActorOf(Props.Create(() => new SelfDBScheduledActor()), "Self_DBSchelduled");
        }




        public void Handle(UnverifiedOrderNumMsg message)
        {
            
        }

        public void Handle(OrderDetailRequest message)
        {
            _selfDBActor.Tell(message, Self);
        }

        public void Handle(Terminated message)
        {
            //remove terminated actor from hashlist
            _selfDBScheduledActor.Tell(new ClientDisconnectRequest(message.ActorRef) );
        }

        public void Handle(SubmitOrderDetail message)
        {
            _selfDBActor.Tell(message, Self);
        }

        public void Handle(ClientConnectRequest message)
        {
            
            _selfDBScheduledActor.Tell(message, Self);
        }

        public void Handle(SubmitOrderDetailRespond message)
        {
            message.Subscriber.Tell(message);
        }

        public void Handle(OrderDetailRespond message)
        {
            message.Subscriber.Tell(message);
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
