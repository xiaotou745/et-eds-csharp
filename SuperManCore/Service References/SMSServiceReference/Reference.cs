﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18444
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SuperManCore.SMSServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SMSServiceReference.SmsSoap")]
    public interface SmsSoap {
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 HelloWorldResult 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/HelloWorld", ReplyAction="*")]
        SuperManCore.SMSServiceReference.HelloWorldResponse HelloWorld(SuperManCore.SMSServiceReference.HelloWorldRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/HelloWorld", ReplyAction="*")]
        System.Threading.Tasks.Task<SuperManCore.SMSServiceReference.HelloWorldResponse> HelloWorldAsync(SuperManCore.SMSServiceReference.HelloWorldRequest request);
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 mobile 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SendSmsSaveLog", ReplyAction="*")]
        SuperManCore.SMSServiceReference.SendSmsSaveLogResponse SendSmsSaveLog(SuperManCore.SMSServiceReference.SendSmsSaveLogRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SendSmsSaveLog", ReplyAction="*")]
        System.Threading.Tasks.Task<SuperManCore.SMSServiceReference.SendSmsSaveLogResponse> SendSmsSaveLogAsync(SuperManCore.SMSServiceReference.SendSmsSaveLogRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorld", Namespace="http://tempuri.org/", Order=0)]
        public SuperManCore.SMSServiceReference.HelloWorldRequestBody Body;
        
        public HelloWorldRequest() {
        }
        
        public HelloWorldRequest(SuperManCore.SMSServiceReference.HelloWorldRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class HelloWorldRequestBody {
        
        public HelloWorldRequestBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorldResponse", Namespace="http://tempuri.org/", Order=0)]
        public SuperManCore.SMSServiceReference.HelloWorldResponseBody Body;
        
        public HelloWorldResponse() {
        }
        
        public HelloWorldResponse(SuperManCore.SMSServiceReference.HelloWorldResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class HelloWorldResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string HelloWorldResult;
        
        public HelloWorldResponseBody() {
        }
        
        public HelloWorldResponseBody(string HelloWorldResult) {
            this.HelloWorldResult = HelloWorldResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SendSmsSaveLogRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SendSmsSaveLog", Namespace="http://tempuri.org/", Order=0)]
        public SuperManCore.SMSServiceReference.SendSmsSaveLogRequestBody Body;
        
        public SendSmsSaveLogRequest() {
        }
        
        public SendSmsSaveLogRequest(SuperManCore.SMSServiceReference.SendSmsSaveLogRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class SendSmsSaveLogRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string mobile;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string content;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public int supplierId;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string smsSource;
        
        public SendSmsSaveLogRequestBody() {
        }
        
        public SendSmsSaveLogRequestBody(string mobile, string content, int supplierId, string smsSource) {
            this.mobile = mobile;
            this.content = content;
            this.supplierId = supplierId;
            this.smsSource = smsSource;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SendSmsSaveLogResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SendSmsSaveLogResponse", Namespace="http://tempuri.org/", Order=0)]
        public SuperManCore.SMSServiceReference.SendSmsSaveLogResponseBody Body;
        
        public SendSmsSaveLogResponse() {
        }
        
        public SendSmsSaveLogResponse(SuperManCore.SMSServiceReference.SendSmsSaveLogResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class SendSmsSaveLogResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string SendSmsSaveLogResult;
        
        public SendSmsSaveLogResponseBody() {
        }
        
        public SendSmsSaveLogResponseBody(string SendSmsSaveLogResult) {
            this.SendSmsSaveLogResult = SendSmsSaveLogResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SmsSoapChannel : SuperManCore.SMSServiceReference.SmsSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SmsSoapClient : System.ServiceModel.ClientBase<SuperManCore.SMSServiceReference.SmsSoap>, SuperManCore.SMSServiceReference.SmsSoap {
        
        public SmsSoapClient() {
        }
        
        public SmsSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SmsSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SmsSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SmsSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SuperManCore.SMSServiceReference.HelloWorldResponse SuperManCore.SMSServiceReference.SmsSoap.HelloWorld(SuperManCore.SMSServiceReference.HelloWorldRequest request) {
            return base.Channel.HelloWorld(request);
        }
        
        public string HelloWorld() {
            SuperManCore.SMSServiceReference.HelloWorldRequest inValue = new SuperManCore.SMSServiceReference.HelloWorldRequest();
            inValue.Body = new SuperManCore.SMSServiceReference.HelloWorldRequestBody();
            SuperManCore.SMSServiceReference.HelloWorldResponse retVal = ((SuperManCore.SMSServiceReference.SmsSoap)(this)).HelloWorld(inValue);
            return retVal.Body.HelloWorldResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SuperManCore.SMSServiceReference.HelloWorldResponse> SuperManCore.SMSServiceReference.SmsSoap.HelloWorldAsync(SuperManCore.SMSServiceReference.HelloWorldRequest request) {
            return base.Channel.HelloWorldAsync(request);
        }
        
        public System.Threading.Tasks.Task<SuperManCore.SMSServiceReference.HelloWorldResponse> HelloWorldAsync() {
            SuperManCore.SMSServiceReference.HelloWorldRequest inValue = new SuperManCore.SMSServiceReference.HelloWorldRequest();
            inValue.Body = new SuperManCore.SMSServiceReference.HelloWorldRequestBody();
            return ((SuperManCore.SMSServiceReference.SmsSoap)(this)).HelloWorldAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SuperManCore.SMSServiceReference.SendSmsSaveLogResponse SuperManCore.SMSServiceReference.SmsSoap.SendSmsSaveLog(SuperManCore.SMSServiceReference.SendSmsSaveLogRequest request) {
            return base.Channel.SendSmsSaveLog(request);
        }
        
        public string SendSmsSaveLog(string mobile, string content, int supplierId, string smsSource) {
            SuperManCore.SMSServiceReference.SendSmsSaveLogRequest inValue = new SuperManCore.SMSServiceReference.SendSmsSaveLogRequest();
            inValue.Body = new SuperManCore.SMSServiceReference.SendSmsSaveLogRequestBody();
            inValue.Body.mobile = mobile;
            inValue.Body.content = content;
            inValue.Body.supplierId = supplierId;
            inValue.Body.smsSource = smsSource;
            SuperManCore.SMSServiceReference.SendSmsSaveLogResponse retVal = ((SuperManCore.SMSServiceReference.SmsSoap)(this)).SendSmsSaveLog(inValue);
            return retVal.Body.SendSmsSaveLogResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SuperManCore.SMSServiceReference.SendSmsSaveLogResponse> SuperManCore.SMSServiceReference.SmsSoap.SendSmsSaveLogAsync(SuperManCore.SMSServiceReference.SendSmsSaveLogRequest request) {
            return base.Channel.SendSmsSaveLogAsync(request);
        }
        
        public System.Threading.Tasks.Task<SuperManCore.SMSServiceReference.SendSmsSaveLogResponse> SendSmsSaveLogAsync(string mobile, string content, int supplierId, string smsSource) {
            SuperManCore.SMSServiceReference.SendSmsSaveLogRequest inValue = new SuperManCore.SMSServiceReference.SendSmsSaveLogRequest();
            inValue.Body = new SuperManCore.SMSServiceReference.SendSmsSaveLogRequestBody();
            inValue.Body.mobile = mobile;
            inValue.Body.content = content;
            inValue.Body.supplierId = supplierId;
            inValue.Body.smsSource = smsSource;
            return ((SuperManCore.SMSServiceReference.SmsSoap)(this)).SendSmsSaveLogAsync(inValue);
        }
    }
}
