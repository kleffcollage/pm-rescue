using System;
using System.Collections.Generic;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.ViewModels
{
    public class PropertyView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public bool SellMyself { get; set; }
        public double Price { get; set; }
        public int NumberOfBedrooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public bool IsDraft { get; set; }
        public bool IsActive { get; set; }
        public string RejectionReason { get; set; }
        public bool IsForRent { get; set; }
        public bool IsForSale { get; set; }
        public string Area { get; set; }
        public string PropertyType { get; set; }
        public UserView CreatedByUser { get; set; }
        public MediaView[] MediaFiles { get; set; }
        public bool Verified { get; set; }
        public User Representative { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }
        public bool IsRequest { get; set; }
        public string Status { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; } 
        public int Views { get; set; }
        public int Enquiries { get; set; }
        public DateTime DateCreated { get; set; }
        public int PropertyTypeId { get; set; }
        public string DocumentUrl { get; set; }
    }
}