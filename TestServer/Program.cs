using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestServer.Actors;

namespace TestServer
{
    class Program
    {
        public static ActorSystem MyActorSystem;


        static void Main(string[] args)
        {

            MyActorSystem = ActorSystem.Create("TestServer");
            MyActorSystem.ActorOf(Props.Create(() => new ServerActor()), "MyServerActor");

            Console.ReadLine();
        }
    }
}
