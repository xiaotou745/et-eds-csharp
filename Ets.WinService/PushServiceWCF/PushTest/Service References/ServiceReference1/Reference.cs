﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace PushTest.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IPushSer")]
    public interface IPushSer {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPushSer/DoWork", ReplyAction="http://tempuri.org/IPushSer/DoWorkResponse")]
        void DoWork();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPushSer/DoWork", ReplyAction="http://tempuri.org/IPushSer/DoWorkResponse")]
        System.Threading.Tasks.Task DoWorkAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPushSer/PushForMobile", ReplyAction="http://tempuri.org/IPushSer/PushForMobileResponse")]
        void PushForMobile(string msginfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPushSer/PushForMobile", ReplyAction="http://tempuri.org/IPushSer/PushForMobileResponse")]
        System.Threading.Tasks.Task PushForMobileAsync(string msginfo);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPushSerChannel : PushTest.ServiceReference1.IPushSer, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PushSerClient : System.ServiceModel.ClientBase<PushTest.ServiceReference1.IPushSer>, PushTest.ServiceReference1.IPushSer {
        
        public PushSerClient() {
        }
        
        public PushSerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PushSerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PushSerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PushSerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void DoWork() {
            base.Channel.DoWork();
        }
        
        public System.Threading.Tasks.Task DoWorkAsync() {
            return base.Channel.DoWorkAsync();
        }
        
        public void PushForMobile(string msginfo) {
            base.Channel.PushForMobile(msginfo);
        }
        
        public System.Threading.Tasks.Task PushForMobileAsync(string msginfo) {
            return base.Channel.PushForMobileAsync(msginfo);
        }
    }
}
