using System;
using System.ComponentModel.DataAnnotations;

public class ToDo
{
    [Key]
    public int TaskId { get; set; }

    [Required(ErrorMessage = "Titlul este obligatoriu")]
    public string? Title { get; set; }

    public string Description { get; set; }

    [Display(Name = "Data completării")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    [Required(ErrorMessage = "Data completării este obligatorie")]
    public DateTime? DateCompleted { get; set; }

    [Display(Name = "Este complet")]
    public bool IsCompleted { get; set; }

    public DateTime? DateClosed { get; set; }
    

}
