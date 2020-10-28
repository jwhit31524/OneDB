using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace JamesWhittonCSCI342DatabaseWk6
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            int VehicleNo = 0;
            int returnCode = 0;

            Vehicle vehicle = new Vehicle
            {
                VehicleTypeCode = 4,
                VehicleId = "111",
                VehicleColor = "c",
                VehicleAddDateTime = DateTime.Now
            };

            // Note that this will add a new record every time the program is executed 
            VehicleNo = await VehicleDB.AddAsync(vehicle).ConfigureAwait(false);

            if (VehicleNo > 0)
            {
                vehicle = await VehicleDB.InquireAsync(VehicleNo).ConfigureAwait(false);
                //Console.WriteLine(vehicle.ToString());

                vehicle.VehicleColor = "blue";
                vehicle.VehicleId = "456";

                returnCode = await VehicleDB.UpdateAsync(vehicle).ConfigureAwait(false);
                if (returnCode == 0)
                {
                    Console.WriteLine("\nAfter Updating");
                    Console.WriteLine(vehicle.ToString());

                    VehicleNo = vehicle.VehicleNo; // save the Id 
                    Console.WriteLine($"\nDelete VehicleNo: {VehicleNo} from the database");
                    returnCode = await VehicleDB.DeleteAsync(vehicle).ConfigureAwait(false);
                    if (returnCode == 0)
                    {
                        vehicle = await VehicleDB.InquireAsync(vehicle.VehicleNo).ConfigureAwait(false);
                        if (vehicle == null)
                            Console.WriteLine($"VehicleNo: {VehicleNo} Not found in the database");
                        else
                            Console.WriteLine($"Hmmm... delete returned success yet the vehicle record {VehicleNo} still exists...");
                    }
                    else
                        Console.WriteLine($"There was an error deleting the vehicle from the database");
                }
                else
                    Console.WriteLine($"There was an error updating the vehicle in the database");
            }
            else
                Console.WriteLine($"There was an error adding the vehicle to the database");


            Console.ReadLine();
            //int VehicleNo = 0;
            // int returnCode = 0;
            // add vehicle
            // console.log new vehicle validation
            // update vehicle
            // console.log update validation
            // delete the vehicle
            // console.log delete validation

            // Vehicle vehicle = await AddNewVehicle();

            //await ChangeVehicleColor(vehicle);

            //await DeleteVehicle(vehicle);

        }

       /* private static async Task DeleteVehicle(Vehicle vehicle)
        {
            var VehicleNo = vehicle.VehicleNo; // save the Id 
            Console.WriteLine($"\nDelete VehicleNo: {vehicle.VehicleNo} from the database");
            var returnCode = await VehicleDB.DeleteAsync(vehicle).ConfigureAwait(false);
            if (returnCode == 0)
            {
                vehicle = await VehicleDB.InquireAsync(vehicle.VehicleNo).ConfigureAwait(false);
                if (vehicle == null)
                    Console.WriteLine($"VehicleNo: {VehicleNo} Not found in the database");
                else
                    Console.WriteLine($"Hmmm... delete returned success yet the student record {VehicleNo} still exists...");
            }
            else
                Console.WriteLine($"There was an error deleting the student from the database");
        }

            private static async Task ChangeVehicleColor(Vehicle vehicle)
        {
            vehicle = await VehicleDB.InquireAsync(vehicle.VehicleNo).ConfigureAwait(false);
            Console.WriteLine(vehicle.ToString());

            vehicle.VehicleId = "1";
            vehicle.VehicleColor = "green";

            var returnCode = await VehicleDB.UpdateAsync(vehicle).ConfigureAwait(false);

            if (returnCode <= 0)
            {
                Console.WriteLine($"There was an error updating the vehicle in the database");

                throw new Exception();
            }

           
        }

        private static async Task <Vehicle> AddNewVehicle()
        {

            Vehicle vehicle = new Vehicle
            {
                VehicleTypeCode = 1,
                VehicleId = "123",
                VehicleColor = "red",
                VehicleAddDateTime = DateTime.Now
            };

            // Note that this will add a new record every time the program is executed 
            var vehicleNo = await VehicleDB.AddAsync(vehicle).ConfigureAwait(false);
            if (vehicleNo <= -2)
            {
                Console.WriteLine($"There was an error adding the vehicle to the database");

                throw new Exception();
            }
            return vehicle;
        }
       */
    }
}
