using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PropertyMataaz.Utilities.Constants;

namespace PropertyMataaz.Models.AppModels
{
    public class Property : BaseModel
    {
        [Required]
        [MaxLength(ModelConstants.MAX_LENGTH_60)]
        [Column(TypeName = "varchar")]
        public string Name { get; set; }
        [Required]
        [MaxLength(ModelConstants.MAX_LENGTH_60)]
        [Column(TypeName = "varchar")]
        public string Title { get; set; }
        [Required]
        [MaxLength(ModelConstants.MAX_LENGTH_200)]
        [Column(TypeName = "varchar")]
        public string Address { get; set; }
        [Required]
        [MaxLength(ModelConstants.MAX_LENGTH_200)]
        [Column(TypeName = "varchar")]
        public string Description { get; set; }
        [Required]
        [MaxLength(ModelConstants.MAX_LENGTH_200)]
        [Column(TypeName = "varchar")]
        public string state { get; set; }
        [Required]
        [MaxLength(ModelConstants.MAX_LENGTH_200)]
        [Column(TypeName = "varchar")]
        public string LGA { get; set; }
        public bool SellMyself { get; set; }
        public double Price { get; set; }
        [NotMapped]
        public string FormattedPrice {get {return string.Format("{0:C}", Price);}}
        public int NumberOfBedrooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public bool IsDraft { get; set; }
        public bool IsActive { get; set; }
        public bool IsForRent { get; set; }
        public bool IsForSale { get; set; }

        [MaxLength(ModelConstants.MAX_LENGTH_200)]
        public string Area { get; set; }
        public bool Verified { get; set; }
        public int? RepresentativeId { get; set; }
        public User Representative { get; set; }
        public int PropertyTypeId { get; set; }
        public PropertyType PropertyType { get; set; }
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public List<Media> MediaFiles { get; set; }
        public bool IsRequest { get; set; }
        public int? StatusId { get; set; }
        public Status Status { get; set; }
        public IEnumerable<Report> Reports { get; set; }
        public int Views { get; set; }
        public int Enquiries { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string RejectionReason { get; set; }
        public IEnumerable<UserEnquiry> UserEnquiries { get; set; }
        public int? TenantTypeId { get; set; }
        public TenantType TenantType { get; set; }
        public RentCollectionType RentCollectionType { get; set; }
        public int? RentCollectionTypeId { get; set; }
        public IEnumerable<Inspections> Inspections { get; set; }
        public string DocumentUrl { get; set; }
    }
}