using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.InputModels
{
    public class PropertyModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Description { get; set; }
        public bool SellMyself { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int NumberOfBedrooms { get; set; }
        [Required]
        public int NumberOfBathrooms { get; set; }
        public bool IsDraft { get; set; }
        public bool IsActive { get; set; }
        public bool IsForRent { get; set; }
        public bool IsForSale { get; set; }
        [Required]
        public int PropertyTypeId { get; set; }
        public List<MediaModel> MediaFiles { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }
        public string Area { get; set; }
        public bool IsRequest { get; set; }
        public string Comment { get; set; }
        public string Budget { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int PropertyRequestId { get; set; }
        public int TenantTypeId { get; set; }
        public int RentCollectionTypeId { get; set; }
        public int RequestId { get; set; }
        public string Bank {get;set;}
        public string AccountNumber { get; set; }
        public int PropertyRequestMatchId { get; set; }
        public string DocumentUrl { get; set; }
    }
}