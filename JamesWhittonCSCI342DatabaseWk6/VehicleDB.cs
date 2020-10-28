using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesWhittonCSCI342DatabaseWk6
{
    public class VehicleDB
    {
        static readonly string cnFactoryDB = ConfigurationManager.ConnectionStrings["FactoryDB"].ConnectionString;

        static readonly string sqlSelect = @"SELECT 
VehicleNo,
VehicleTypeCode,
VehicleId,
VehicleColor,
VehicleAddDateTime
FROM VHT001_VEHICLE
WHERE VehicleNo = @VehicleNo;";

        private static readonly string sqlInsert = @"
DECLARE @RC INT = 0;                -- this value holds the return code, which will be 0 (success) or -1 (failure) 

BEGIN TRY


BEGIN TRANSACTION

SET @VehicleNo = (SELECT ISNULL(MAX(VehicleNo), 0) FROM VHT001_VEHICLE); 
SET @VehicleNo = @VehicleNo + 1; 

INSERT INTO VHT001_VEHICLE
(
VehicleTypeCode,
VehicleId,
VehicleColor,
VehicleAddDateTime
)
VALUES
(
@VehicleTypeCode,
@VehicleId,
@VehicleColor,
@VehicleAddDateTime
);

--SET @RC = SCOPE_IDENTITY();

COMMIT TRANSACTION 



END TRY

BEGIN CATCH 

IF @@TRANCOUNT > 0
   ROLLBACK TRANSACTION;

 -- there has been an error so set the return code to -1
SET @RC = -1;

-- Uncomment this to cause an error to be thrown in the calling (C#) code 
THROW;  

END CATCH
SELECT @RC;
";

        private static readonly string sqlUpdate = @"
DECLARE @RC INT = 0;                -- this value holds the return code, which will be 0 (success) or -1 (failure) 

BEGIN TRY

--SET @StudentMajorCode = UPPER(@StudentMajorCode); -- force this field to be uppercase 

UPDATE VHT001_VEHICLE
SET
VehicleTypeCode = @VehicleTypeCode,
VehicleId = @VehicleId,
VehicleColor = @VehicleColor,
VehicleAddDateTime = @VehicleAddDateTime
WHERE VehicleNo = @VehicleNo; 

END TRY

BEGIN CATCH 

 -- there has been an error so set the return code to -1
SET @RC = -1;

-- Uncomment this to cause an error to be thrown in the calling (C#) code 
 THROW;  

END CATCH
";

        private static readonly string sqlDelete = @"
DECLARE @RC INT = 0;                -- this value holds the return code, which will be 0 (success) or -1 (failure) 

BEGIN TRY

DELETE FROM VHT001_VEHICLE
WHERE VehicleNo = @VehicleNo; 

END TRY

BEGIN CATCH 

 -- there has been an error so set the return code to -1
SET @RC = -1;

-- Uncomment this to cause an error to be thrown in the calling (C#) code 
 THROW;  

END CATCH
";

        public static Vehicle Inquire(int VehicleNo)
        {
            Vehicle vehicle = null;

            using (SqlConnection cn = new SqlConnection(cnFactoryDB))
            {
                using (SqlCommand cm = new SqlCommand(sqlSelect, cn))
                {
                    SqlParameter pm = new SqlParameter("@VehicleNo", System.Data.SqlDbType.Int, 4);
                    pm.Direction = System.Data.ParameterDirection.Input;
                    pm.Value = VehicleNo;
                    cm.Parameters.Add(pm);

                    cn.Open();
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())  // Read() returns true if there is a record to read; false otherwise
                        {
                            vehicle = new Vehicle
                            {
                                VehicleNo = (int)dr["VehicleNo"],
                                VehicleTypeCode = (byte)dr["VehicleTypeCode"], //tinyint
                                VehicleId = dr["VehicleId"] as string, //Varchar 50
                                VehicleColor = dr["VehicleColor"] as string, //varchar15
                                VehicleAddDateTime = (DateTime)dr["VehicleAddDateTime"],
                                
                            };
                        }
                    }
                }
            }

            return vehicle;
        }

        public async static Task<Vehicle> InquireAsync(int VehicleNo)
        {
            Vehicle vehicle = null;

            using (SqlConnection cn = new SqlConnection(cnFactoryDB))
            {
                using (SqlCommand cm = new SqlCommand(sqlSelect, cn))
                {
                    SqlParameter pm = new SqlParameter("@VehicleNo", System.Data.SqlDbType.Int, 4);
                    pm.Direction = System.Data.ParameterDirection.Input;
                    pm.Value = VehicleNo;
                    cm.Parameters.Add(pm);

                    await cn.OpenAsync().ConfigureAwait(false);
                    using (SqlDataReader dr = await cm.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        if (await dr.ReadAsync().ConfigureAwait(false))  // Read() returns true if there is a record to read; false otherwise
                        {
                            vehicle = new Vehicle
                            {
                                VehicleNo = (int)dr["VehicleNo"],
                                VehicleTypeCode = (byte)dr["VehicleTypeCode"], //tinyint
                                VehicleId = dr["VehicleId"] as string, //Varchar 50
                                VehicleColor = dr["VehicleColor"] as string, //varchar15
                                VehicleAddDateTime = (DateTime)dr["VehicleAddDateTime"],

                            };
                        }
                    }

                }
            }

            return vehicle;
        }



        public static int Add(Vehicle vehicle)
        {
            int returnValue = 0;

            if (vehicle == null) throw new ArgumentNullException("Vehicle cannot be null");

            using (SqlConnection cn = new SqlConnection(cnFactoryDB))
            {
                using (SqlCommand cm = new SqlCommand(sqlInsert, cn))
                {
                    SqlParameter pm = new SqlParameter("@VehicleNo", SqlDbType.Int, 1);
                    pm.Direction = ParameterDirection.Output;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleTypeCode", SqlDbType.TinyInt, 3);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleTypeCode;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleId", SqlDbType.VarChar, 50);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleId;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleColor", SqlDbType.VarChar, 15);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleColor;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleAddDateTime", SqlDbType.DateTime);
                    pm.Direction = ParameterDirection.Input;
                    pm.Precision = 3;   // total digits
                    pm.Scale = 2;       // digits to the right of the decimal point 
                    pm.Value = vehicle.VehicleAddDateTime;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@RC", SqlDbType.Int, 4);
                    pm.Direction = ParameterDirection.ReturnValue;
                    cm.Parameters.Add(pm);

                    cn.Open();
                    cm.ExecuteNonQuery();

                    returnValue = (int)cm.Parameters["@RC"].Value;
                    if (returnValue == 0)
                    {
                        returnValue = (int)cm.Parameters["@VehicleNo"].Value;
                    }
                }
            }

            return returnValue;
        }

     
        public async static Task<int> AddAsync(Vehicle vehicle)
        {
            int returnValue = 0;

            if (vehicle == null) throw new ArgumentNullException("Vehicle cannot be null");

            using (SqlConnection cn = new SqlConnection(cnFactoryDB))
            {
                using (SqlCommand cm = new SqlCommand(sqlInsert, cn))
                {
                    SqlParameter pm = new SqlParameter("@VehicleNo", SqlDbType.Int, 1);
                    pm.Direction = ParameterDirection.Output;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleTypeCode", SqlDbType.TinyInt, 3);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleTypeCode;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleId", SqlDbType.VarChar, 50);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleId;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleColor", SqlDbType.VarChar, 15);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleColor;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleAddDateTime", SqlDbType.DateTime);
                    pm.Direction = ParameterDirection.Input;
                    pm.Precision = 3;   // total digits
                    pm.Scale = 2;       // digits to the right of the decimal point 
                    pm.Value = vehicle.VehicleAddDateTime;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@RC", SqlDbType.Int, 4);
                    pm.Direction = ParameterDirection.ReturnValue;
                    cm.Parameters.Add(pm);

                    //cn.Open();
                    //cm.ExecuteNonQuery();
                    // await cm.ExecuteNonQueryAsync();

                    // returnValue = (int)cm.Parameters["@RC"].Value;

                    //returnValue = (int)cm.ExecuteScalar();

                    await cn.OpenAsync();
                    await cm.ExecuteNonQueryAsync();

                    returnValue = (int)cm.Parameters["@RC"].Value;
                    if (returnValue == 0)
                    {
                        returnValue = (int)cm.Parameters["@VehicleNo"].Value;
                    }
                }


                //if (returnValue == 0)
                //{
                //    returnValue = (int)cm.Parameters["@VehicleNo"].Value;
                //}
            
            }

            return returnValue;
        }

      
        public static int Update(Vehicle vehicle)
        {
            int returnValue = 0;

            if (vehicle == null) throw new ArgumentNullException("Vehicle cannot be null");

            using (SqlConnection cn = new SqlConnection(cnFactoryDB))
            {
                using (SqlCommand cm = new SqlCommand(sqlUpdate, cn))
                {
                    SqlParameter pm = new SqlParameter("@VehicleNo", SqlDbType.Int, 1);
                    pm.Direction = ParameterDirection.Output;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleTypeCode", SqlDbType.TinyInt, 3);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleTypeCode;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleId", SqlDbType.VarChar, 50);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleId;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleColor", SqlDbType.VarChar, 15);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleColor;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleAddDateTime", SqlDbType.DateTime);
                    pm.Direction = ParameterDirection.Input;
                    pm.Precision = 3;   // total digits
                    pm.Scale = 2;       // digits to the right of the decimal point 
                    pm.Value = vehicle.VehicleAddDateTime;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@RC", SqlDbType.Int, 4);
                    pm.Direction = ParameterDirection.ReturnValue;
                    cm.Parameters.Add(pm);

                    cn.Open();
                    cm.ExecuteNonQuery();

                    returnValue = (int)cm.Parameters["@RC"].Value;
                }
            }

            return returnValue;
        }


        public async static Task<int> UpdateAsync(Vehicle vehicle)
        {
            int returnValue = 0;

            if (vehicle == null) throw new ArgumentNullException("Vehicle cannot be null");

            using (SqlConnection cn = new SqlConnection(cnFactoryDB))
            {
                using (SqlCommand cm = new SqlCommand(sqlUpdate, cn))
                {
                    SqlParameter pm = new SqlParameter("@VehicleNo", SqlDbType.Int, 1);
                    pm.Direction = ParameterDirection.Output;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleTypeCode", SqlDbType.TinyInt, 3);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleTypeCode;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleId", SqlDbType.VarChar, 50);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleId;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleColor", SqlDbType.VarChar, 15);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleColor;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@VehicleAddDateTime", SqlDbType.DateTime);
                    pm.Direction = ParameterDirection.Input;
                    pm.Precision = 3;   // total digits
                    pm.Scale = 2;       // digits to the right of the decimal point 
                    pm.Value = vehicle.VehicleAddDateTime;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@RC", SqlDbType.Int, 4);
                    pm.Direction = ParameterDirection.ReturnValue;
                    cm.Parameters.Add(pm);

                    await cn.OpenAsync().ConfigureAwait(false);
                    await cm.ExecuteNonQueryAsync().ConfigureAwait(false);

                    returnValue = (int)cm.Parameters["@RC"].Value;
                }
            }

            return returnValue;
        }


        public static int Delete(Vehicle vehicle)
        {
            int returnValue = 0;

            if (vehicle == null) throw new ArgumentNullException("Vehicle cannot be null");

            using (SqlConnection cn = new SqlConnection(cnFactoryDB))
            {
                using (SqlCommand cm = new SqlCommand(sqlDelete, cn))
                {
                    SqlParameter pm = new SqlParameter("@VehicleNo", SqlDbType.Int, 4);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleNo;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@RC", SqlDbType.Int, 4);
                    pm.Direction = ParameterDirection.ReturnValue;
                    cm.Parameters.Add(pm);

                    cn.Open();
                    cm.ExecuteNonQuery();

                    returnValue = (int)cm.Parameters["@RC"].Value;
                }
            }

            return returnValue;
        }

        public async static Task<int> DeleteAsync(Vehicle vehicle)
        {
            int returnValue = 0;

            if (vehicle == null) throw new ArgumentNullException("Vehicle cannot be null");

            using (SqlConnection cn = new SqlConnection(cnFactoryDB))
            {
                using (SqlCommand cm = new SqlCommand(sqlDelete, cn))
                {
                    SqlParameter pm = new SqlParameter("@VehicleNo", SqlDbType.Int, 4);
                    pm.Direction = ParameterDirection.Input;
                    pm.Value = vehicle.VehicleNo;
                    cm.Parameters.Add(pm);

                    pm = new SqlParameter("@RC", SqlDbType.Int, 4);
                    pm.Direction = ParameterDirection.ReturnValue;
                    cm.Parameters.Add(pm);

                    await cn.OpenAsync().ConfigureAwait(false);
                    await cm.ExecuteNonQueryAsync().ConfigureAwait(false);

                    returnValue = (int)cm.Parameters["@RC"].Value;
                }
            }

            return returnValue;
        }
    }
}
