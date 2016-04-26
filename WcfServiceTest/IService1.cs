using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


namespace WcfServiceTest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        /*
        [OperationContract]
        string GetData(int value);
        */

        [OperationContract]
        buildingEntity[] GetData(string id);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        string updateBuilding(string id, string field, string value);

        [OperationContract]
        string insertBuilding(string buildingNo, string buildingName);

        [OperationContract]
        string deleteBuilding(string id);

        /*
        [OperationContract]
        [WebGet(UriTemplate = "building", ResponseFormat = WebMessageFormat.Json)]
        building[] GetAllBuildings();
        */


        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
    /*
    [DataContract]
    public class building
    {
        [DataMember]
        public int buildingID { get; set; }

        [DataMember]
        public string buildingNo { get; set; }

        [DataMember]
        public string buildingName { get; set; }

        [DataMember]
        public DateTime? created { get; set; }
        
        [DataMember]
        public DateTime? updated { get; set; }
    }
     **/
}
