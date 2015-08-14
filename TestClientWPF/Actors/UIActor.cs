using Akka.Actor;
using MessageClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestClientWPF.Actors
{
    public class UIActor : TypedActor,
         IHandle<OrderNumByType>
    {
        private readonly TextBox _textbox;
        private readonly string ExampleOutput = "UnfinishedOrder: {0}; PaidOrder: {1}; WaitForVerifyOrder: {2}; ReadyToDownloadOrder: {3}; VerifyFailedOrder: {4}; DownloadedOrder: {5}; FinishedOrder: {6}; CancelledOrder: {7}; ";



        public UIActor(TextBox textBox)
        {
            _textbox = textBox;
              
        }


        public void Handle(OrderNumByType message)
        {
            string msg = string.Format(ExampleOutput, message.UnfinishedOrder, message.PaidOrder, message.WaitForVerifyOrder, message.ReadyToDownloadOrder, message.VerifyFailedOrder, message.DownloadedOrder, message.FinishedOrder, message.CancelledOrder);

            if (string.IsNullOrEmpty(_textbox.Text))
            {
                _textbox.Text = msg;
            }
            else
            {
                _textbox.Text = _textbox.Text+Environment.NewLine + msg;
            }
            _textbox.ScrollToEnd();
        }

    }
}
