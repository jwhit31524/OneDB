using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace JamesWhittonCSCI342DatabaseWk6
{
  //[Table("VHT001_VEHICLE")]
    public class Vehicle
    {
          /*[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int VehicleNo { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "tinyint")]
        public string VehicleTypeCode { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string VehicleId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string VehicleColor { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "SMALLDATETIME")]
        public string VehicleAddDateTime { get; set; }

    */

        
        public int VehicleNo { get; set; }
        public byte VehicleTypeCode { get; set; }
        public string VehicleId { get; set; }
        public string VehicleColor { get; set; }
        public DateTime? VehicleAddDateTime { get; set; }

        

        // Audit information (not intended to be modified directly) 
       

        public Vehicle()
        {
            VehicleNo = 1;
            VehicleTypeCode = 1;
            VehicleId = "";
            VehicleColor = "";
            VehicleAddDateTime = null;
        }

        public Vehicle(int VehicleNo)
        {
            var v = VehicleDB.Inquire(VehicleNo);
            if (v != null)
            {
                this.VehicleNo = v.VehicleNo;
                VehicleTypeCode = v.VehicleTypeCode;
                VehicleId = v.VehicleId;
                VehicleColor = v.VehicleColor;
                VehicleAddDateTime = v.VehicleAddDateTime;
            }
        }

        public override string ToString()
        {
            return $"VehicleNo: {VehicleNo}\nVehicleTypeCode: {VehicleTypeCode}\nVehicleID: {VehicleId}\nVehicleColor: {VehicleColor}\nVehicleAddDateTime: {VehicleAddDateTime}";
        }
    }
}
