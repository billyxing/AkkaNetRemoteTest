using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using MessageClassLibrary;
using System.Windows.Controls;

namespace TestClientWPF.Actors
{
    public class ClientActor : TypedActor,
        IHandle<OrderNumByType>
    {

        private readonly TextBox _textbox;
        private readonly ActorSelection _serverActor;
        private readonly IActorRef _uiactor;

        public ClientActor(ActorSelection actorSelection, IActorRef uiActor)
        {
            _uiactor = uiActor;
            _serverActor = actorSelection;
            ActorInitial();
        }

        private void ActorInitial()
        {
            _serverActor.Tell(new ClientConnectRequest(Guid.NewGuid().ToString(), Self), Self);
        }

       

        public void Handle(OrderNumByType message)
        {
            _uiactor.Tell(message);
        }

       


    }
}
