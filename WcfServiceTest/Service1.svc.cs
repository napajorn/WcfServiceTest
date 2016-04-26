using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Data.Entity;
using System.Text;




namespace WcfServiceTest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        registrationEntities reg;
  
        public Service1()  
        {
            reg = new registrationEntities();
        }

        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "data/{id}")]
        public buildingEntity[] GetData(string id)
        {
            //var query = (from r in reg.buildings select r);
            string connectionString = @"data source=MACBOOKPRO\SQL14BISRV;initial catalog=registration;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "select b.buildingID, b.buildingName, b.buildingNo, b.created, b.updated ";
            query += ", (select f.facultyName from registration.dbo.faculty f where f.facultyCode = b.facultyCode ) as facultyName ";
            query += "from registration.dbo.building b where b.active <> 'N'"; //where not buildingName like '%FAC%'";
            //buildingID, buildingName, buildingNo, FORMAT(created,'dd/MM/yyyy HH:ss') AS created, updated
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = CommandType.Text;


            cmd.Connection = conn;
            conn.Open();

            buildingEntity[] allRecords = null;
            using (var reader = cmd.ExecuteReader())
            {
                var list = new List<buildingEntity>();
                while (reader.Read())
                    list.Add(new buildingEntity
                    {
                        buildingID = (reader.IsDBNull(0) ? 0 : reader.GetInt32(0)),
                        buildingName = (reader.IsDBNull(1) ? "" : reader.GetString(1)),
                        buildingNo = (reader.IsDBNull(2) ? "" : reader.GetString(2)),
                        created = (reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3)), //(DateTime?)null
                        updated = (reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)),
                        facultyCode = (reader.IsDBNull(5) ? "" : reader.GetString(5))
                    });
                allRecords = list.ToArray();
            }
            

            conn.Close();

            return allRecords; //query.ToArray();
            
        }
        
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "updateBuilding/{id}/{field}/{value}")]
        public string updateBuilding(string id, string field, string value)
        {
            string connectionString = @"data source=MACBOOKPRO\SQL14BISRV;initial catalog=registration;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
            SqlConnection conn = new SqlConnection(connectionString);
            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string query = "UPDATE building SET ";
            query += field + " = N'" + value + "' , updated = '" + sqlFormattedDate + "' ";
            query += "where buildingID = " + id;
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = CommandType.Text;

            try{
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            } catch (SqlException ex) {
               return "there was an issue! "+ex;
            } catch (Exception ex) {
                return "there was another issue!" + ex;
            } finally {
                conn.Close();
            }
            return "updated successful";
        }

        [WebInvoke(Method = "POST",
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "insertBuilding/{buildingNo}/{buildingName}")]
        public string insertBuilding(string buildingNo, string buildingName)
        {
            string connectionString = @"data source=MACBOOKPRO\SQL14BISRV;initial catalog=registration;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "INSERT INTO building (buildingNo,buildingName) ";
            query += "VALUES( '" + buildingNo + "' , '" + buildingName + "' )";
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = CommandType.Text;

            try{
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            } catch (SqlException ex) {
               return "there was an issue! "+ex;
            } catch (Exception ex) {
                return "there was another issue!" + ex;
            } finally {
                conn.Close();
            }
            return "inserted successful";

        }

        [WebInvoke(Method = "POST",
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "deleteBuilding/{id}")]
        public string deleteBuilding(string id)
        {
            string connectionString = @"data source=MACBOOKPRO\SQL14BISRV;initial catalog=registration;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
            SqlConnection conn = new SqlConnection(connectionString);
            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string query = "UPDATE registration.dbo.building SET ";
            query += "active = 'N', updated = '" + sqlFormattedDate + "' ";
            query += "where buildingID = " + id;
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = CommandType.Text;

            try
            {
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                return "there was an issue! " + ex;
            }
            catch (Exception ex)
            {
                return "there was another issue!" + ex;
            }
            finally
            {
                conn.Close();
            }
            return "deleted successful";

        }
        
        /*
        public string GetBuilding()
        {
            building b = new building();
            b.buildingID = buildings;
            b.buildingName = buildings.au_fname;
            b.created = buildings.au_id;
            b.updated = buildings.au_lname;
            return b.ToString();
            //return string.Format("You entered: {0}", 0);
        }
        */

        /*
        public building[] GetAllBuildings()
        {

            return reg.buildings.ToArray();
        }
        */
        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
