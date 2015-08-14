using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka;
using Akka.Actor;

namespace MessageClassLibrary
{

    #region 客户端连接信息
    public class ClientConnectRequest
    {
        public string ClientCN { get; private set; }
        public IActorRef Subscriber { get; private set; }

        public ClientConnectRequest(string clientCN,IActorRef subscriber)
        {
            ClientCN = clientCN;
            Subscriber = subscriber;
        }

    }

    public class ClientDisconnectRequest
    {
        
        public IActorRef Subscriber { get; private set; }

        public ClientDisconnectRequest(IActorRef subscriber)
        {
            
            Subscriber = subscriber;
        }

    }

    #endregion


    #region 订单数量
    public class NormalOrderNumberRequest  //请求订单数量
    {

    }

    public class NormalOrderNumMsg
    {
        public int NormalOrderNum { get; set; } //待填写发票号码及物流信息的订单

    }

    public class FailedOrderNumMsg
    {

        public int FailedOrderNum { get; set; } //未审核通过的订单

    }


    public class UnverifiedOrderNumMsg
    {

        public int UnverifiedOrderNum { get; set; } //付款信息需确认的订单

    }


    public class IncompleteOrderNumMsg
    {

        public int IncompleteOrderNum { get; set; } //未完成的订单信息
    } 

    public class GatherOrderNumber { }  //定时请求订单数量

    public class GatherOrderNumberFailed //定时请求获取订单数量失败
    {
        public string Reason { get; set; }
    } 


    public class OrderNumByType
    {
        //stage1
        public int UnfinishedOrder { get; set; }  //未完成

        //stage2
        public int PaidOrder { get; set; } //已交费
        public int WaitForVerifyOrder { get; set; } //待审核
        
        //stage3
        public int ReadyToDownloadOrder { get; set; } //待下载
        public int VerifyFailedOrder { get; set; } //审核失败

        //stage4
        public int DownloadedOrder { get; set; } //已下载

        //stage5
        public int FinishedOrder { get; set; } //已完成

        public int CancelledOrder { get; set; }//已取消
    }

    #endregion



    #region 订单详情
    public class OrderDetailRequest  //请求订单详情
    {
        public IActorRef Subscriber { get; private set; }
        public string OrderId { get; private set; }

        public OrderDetailRequest(string orderid,IActorRef subscriber)
        {
            OrderId = orderid;
            Subscriber = subscriber;
        }


    }

    public class OrderDetailRespond //订单详情 json
    {
        public IActorRef Subscriber { get; private set; }
        public string RequestState { get; private set; }
        public string OrderDetail { get; private set; }

        public OrderDetailRespond(string requestState,string orderDetail, IActorRef subscriber)
        {
            RequestState = requestState;
            OrderDetail = orderDetail;
            Subscriber = subscriber;
        }
    }

    
    
    public class SubmitOrderDetail
    {
        public IActorRef Subscriber { get; private set; }
        public string OrderDetail { get; private set; }

        public SubmitOrderDetail(string orderDetail, IActorRef subscriber)
        {
            OrderDetail = orderDetail;
            Subscriber = subscriber;
        }
    } 

    public class SubmitOrderDetailRespond
    {
        public IActorRef Subscriber { get; private set; }
        public string Result { get; private set; }

        public SubmitOrderDetailRespond(string result, IActorRef subscriber)
        {
            Result = result;
            Subscriber = subscriber;
        }
    }
    #endregion





}
