﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobInit.JobManagementService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RunDetailInfo", Namespace="http://schemas.datacontract.org/2004/07/BLW.DataAccessService.DataContract")]
    [System.SerializableAttribute()]
    public partial class RunDetailInfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int AppIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime CreatedDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsDeletedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RunNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int RunStatusField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int AppId {
            get {
                return this.AppIdField;
            }
            set {
                if ((this.AppIdField.Equals(value) != true)) {
                    this.AppIdField = value;
                    this.RaisePropertyChanged("AppId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime CreatedDate {
            get {
                return this.CreatedDateField;
            }
            set {
                if ((this.CreatedDateField.Equals(value) != true)) {
                    this.CreatedDateField = value;
                    this.RaisePropertyChanged("CreatedDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ID {
            get {
                return this.IDField;
            }
            set {
                if ((this.IDField.Equals(value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsDeleted {
            get {
                return this.IsDeletedField;
            }
            set {
                if ((this.IsDeletedField.Equals(value) != true)) {
                    this.IsDeletedField = value;
                    this.RaisePropertyChanged("IsDeleted");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RunNumber {
            get {
                return this.RunNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.RunNumberField, value) != true)) {
                    this.RunNumberField = value;
                    this.RaisePropertyChanged("RunNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int RunStatus {
            get {
                return this.RunStatusField;
            }
            set {
                if ((this.RunStatusField.Equals(value) != true)) {
                    this.RunStatusField = value;
                    this.RaisePropertyChanged("RunStatus");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="JobManagementService.IJobManagementService")]
    public interface IJobManagementService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobManagementService/SaveRunNumber", ReplyAction="http://tempuri.org/IJobManagementService/SaveRunNumberResponse")]
        bool SaveRunNumber(out int id, JobInit.JobManagementService.RunDetailInfo runNumberInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobManagementService/GetLastRunNumber", ReplyAction="http://tempuri.org/IJobManagementService/GetLastRunNumberResponse")]
        string GetLastRunNumber();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IJobManagementServiceChannel : JobInit.JobManagementService.IJobManagementService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class JobManagementServiceClient : System.ServiceModel.ClientBase<JobInit.JobManagementService.IJobManagementService>, JobInit.JobManagementService.IJobManagementService {
        
        public JobManagementServiceClient() {
        }
        
        public JobManagementServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public JobManagementServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JobManagementServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JobManagementServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool SaveRunNumber(out int id, JobInit.JobManagementService.RunDetailInfo runNumberInfo) {
            return base.Channel.SaveRunNumber(out id, runNumberInfo);
        }
        
        public string GetLastRunNumber() {
            return base.Channel.GetLastRunNumber();
        }
    }
}
