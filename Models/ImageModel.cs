using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UploadImage.Models
{
    public class ImageModel
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        [Column(TypeName="nvarchar(100)")]
        [DisplayName("ImageName")]
        public string Name { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
    }
}
