using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResidentsDatabase.Models
{
    public enum OmanGovernorate
    {
        Muscat = 1,
        Dhofar = 2,
        Musandam = 3,
        AlBuraimi = 4,
        AdDakhiliyah = 5,
        NorthAlBatinah = 6,
        SouthAlBatinah = 7,
        SouthAshSharqiyah = 8,
        NorthAshSharqiyah = 9,
        AdDahirah = 10,
        AlWusta = 11
    }
    public class Resident
    {
        //hh
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsersId { get; set; }
        [Required,MinLength(8),MaxLength(8)]
        public string NationalId { get; set; }
        [Required]
        public string FirstName { get; set; } 
        [Required]
        public string MiddleName { get; set; } = string.Empty;
        [Required]
        public string ThirdName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public  OmanGovernorate City { get; set; }
        [Required, MaxLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;
        //add unique constraint to email
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        
    }
}
