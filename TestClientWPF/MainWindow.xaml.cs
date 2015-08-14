using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Akka.Actor;
using Akka;
using TestClientWPF.Actors;

namespace TestClientWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }


        private ActorSystem system;
        private IActorRef clientActor;
        private ActorSelection serverActor;
        private IActorRef uiActor;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            system = ActorSystem.Create("MyClientSystem");
            serverActor = system.ActorSelection("akka.tcp://TestServer@localhost:8081/user/MyServerActor");
            uiActor = system.ActorOf(Props.Create(() => new UIActor(this.textBox)), "MyClient");
            clientActor = system.ActorOf(Props.Create(() => new ClientActor(serverActor, uiActor)), Guid.NewGuid().ToString());
            



        }
    }
}
