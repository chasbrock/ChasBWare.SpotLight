using System.ComponentModel.DataAnnotations.Schema;

namespace ChasBWare.SpotLight.Domain.Entities;

public class LibraryItem
{
    [ForeignKey("Playlist")]
    public string? Id { get; set; }
}
